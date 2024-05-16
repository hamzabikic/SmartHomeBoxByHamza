using Duende.IdentityServer.ResponseHandling;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.EntityFrameworkCore;
using SmartHomeApi.Data;
using SmartHomeApi.Data.Tablice;
using SmartHomeApi.Helpers;
using System.ComponentModel;
using System.Security.Cryptography;

namespace SmartHomeApi.Controllers
{
    [ApiController]
    [Route("[action]")]
    public class KorisnikController
    {
        private readonly MyDBContext db;
        private readonly IHttpContextAccessor context;
        private readonly AuthService auth;
        private readonly EmailSender emailSender;
        public KorisnikController (MyDBContext _db, IHttpContextAccessor _context, AuthService _auth,
            EmailSender _emailSender)
        {
            db = _db;
            context = _context;
            auth = _auth;
            emailSender = _emailSender;
        }
        [HttpPost]
        private void dodajKorisnika ()
        {
            Korisnik novi = new Korisnik
            {
                Ime = "Ime",
                Prezime = "Prezime",
                BrojTelefona = "38766261961",
                Email = "hamza.bikic@edu.fit.ba"
            };
            db.Korisnici.Add(novi);
            db.SaveChanges();
        }
        [HttpPost]
        public async Task<KorisnikEditResponse> editujKorisnika(KorisnikEdit edit)
        {
            var prijavainfo = await auth.getInfo();
            if (!prijavainfo.jeLogiran) return new KorisnikEditResponse
            {
                Editovan = false,
                Greska = "You don't have permission!"
            };
            if(await db.Korisnici.Where(k=> k.Id != prijavainfo.Prijava.KorisnikId && k.Email == edit.Email).AnyAsync())
            {
                return new KorisnikEditResponse
                {
                    Editovan = false,
                    Greska = "The email already exists in the system!"
                };
            }
            if (await db.Korisnici.Where(k => k.Id != prijavainfo.Prijava.KorisnikId && k.BrojTelefona == edit.Telefon).AnyAsync())
            {
                return new KorisnikEditResponse
                {
                    Editovan = false,
                    Greska = "The phone number already exists in the system!"
                };
            }
            var korisnik = await db.Korisnici.FindAsync(prijavainfo.Prijava.KorisnikId);
            korisnik.Ime = edit.Ime;
            korisnik.Prezime = edit.Prezime;
            korisnik.Email = edit.Email;
            korisnik.BrojTelefona = edit.Telefon;
            korisnik.SmsSlanje = edit.SmsSlanje;
            korisnik.EmailSlanje = edit.EmailSlanje;
            db.SaveChanges();
            return new KorisnikEditResponse
            {
                Editovan = true,
                Greska = ""
            };
        }
        [HttpPost]
        public async Task<KorisnikEditResponse> generisiNovuLozinku(PasswordChangeReq req)
        {
            var korisnik = await db.Korisnici.Where(k => k.Email == req.Email).FirstOrDefaultAsync();
            if(korisnik == null)
            {
                return new KorisnikEditResponse { Editovan = false, Greska = "The email doesn't exist in the system!" };
            }
            var lozinka = PasswordGenerator.GenerisiLozinku();
            if (await emailSender.sendPassword(korisnik.Email, lozinka, korisnik.Username))
            {
                korisnik.Password = lozinka;
                db.SaveChanges();
                return new KorisnikEditResponse { Editovan = true, Greska = "" };
            }
            return new KorisnikEditResponse { Editovan = false, Greska = "Password change failed!" };
        }
        [HttpGet]
        public async Task<List<PrijavaList>> getPrijave ()
        {
            var prijavainfo = await auth.getInfo();
            if (!prijavainfo.jeLogiran) throw new Exception("Nemate pravo pristupa!");
            return await db.Prijave.Where(p => p.KorisnikId == prijavainfo.Prijava.KorisnikId &&
            !p.JeUredjaj).Select(
                p => new PrijavaList
                {
                    PrijavaId = p.Id,
                    Datum = p.DatumVrijeme.Date.ToString(),
                    Vrijeme = p.DatumVrijeme.TimeOfDay.Hours.ToString() 
                    +":"+p.DatumVrijeme.TimeOfDay.Minutes.ToString(),
                    IpAdresa = p.IpAdresa
                })
            .ToListAsync();
        }
        [HttpGet]
        public async Task<bool> odjaviUredjaj([FromQuery] int id)
        {
            var prijavainfo = await auth.getInfo();
            if (!prijavainfo.jeLogiran) throw new Exception("Nemate pravo pristupa!");
            if (id == prijavainfo.Prijava.Id) return false;
            var prijava = await db.Prijave.FindAsync(id);
            if (prijava == null) return false;
            if (prijava.JeUredjaj) return false;
            if(prijava.KorisnikId != prijavainfo.Prijava.KorisnikId) throw new Exception("Nemate pravo pristupa!");
            db.Prijave.Remove(prijava);
            db.SaveChanges();
            return true;
        }
        [HttpGet]
        public async Task<KorisnikEdit?> getInfo ()
        {
            var prijavainfo = await auth.getInfo();
            if (!prijavainfo.jeLogiran) throw new Exception("Nemate pravo pristupa!");
            return await db.Korisnici.Where(k => k.Id == prijavainfo.Prijava.KorisnikId).Select(
                k => new KorisnikEdit
                {
                    Ime = k.Ime,
                    Prezime = k.Prezime,
                    Email = k.Email,
                    Telefon = k.BrojTelefona,
                    EmailSlanje = k.EmailSlanje,
                    SmsSlanje = k.SmsSlanje
                }).FirstOrDefaultAsync();
        }
        [HttpPost]
        public async Task<PrijavaResponse> Prijava (PrijavaRequest req)
        {
            var korisnik = await db.Korisnici.Where(k => k.Username == req.Username && k.Password == req.Password)
                .FirstOrDefaultAsync();
            if (korisnik == null) return new PrijavaResponse { jeLogiran = false, Prijava = null };
            var token = TokenGenerator.generisiToken();
            while(await db.Prijave.Where(p=> p.Token == token).AnyAsync())
            {
                token = TokenGenerator.generisiToken();
            }
            var prijava = new Prijava
            {
                DatumVrijeme = DateTime.Now,
                IpAdresa = context.HttpContext.Connection.RemoteIpAddress.ToString(),
                KorisnikId = korisnik.Id,
                Token = token,
                JeUredjaj=false
            };
            db.Prijave.Add(prijava);
            db.SaveChanges();
            return new PrijavaResponse { jeLogiran = true, Prijava = prijava };
        }
        [HttpPost] 
        public async Task<EditLozinkaResponse> promjenaLozinke([FromBody] EditLozinkaReq edit)
        {
            var prijavainfo = await auth.getInfo();
            if (!prijavainfo.jeLogiran) return new EditLozinkaResponse
            {
                Editovan = false,
                Greska = "You are not logged!"
            };

            var korisnik = await db.Korisnici.FindAsync(prijavainfo.Prijava.KorisnikId);
            if(korisnik.Password != edit.StaraLozinka)
            {
                return new EditLozinkaResponse { Editovan = false, Greska = "Wrong entered current password!" };
            }
            if(edit.NovaLozinka == edit.StaraLozinka)
            {
                return new EditLozinkaResponse { Editovan = false, Greska = "The entered password is the same" };
            }
            if(edit.NovaLozinka.Length<8)
            {
                return new EditLozinkaResponse { Editovan = false, Greska = "The password must contain at least 8 characters!" };
            }
            korisnik.Password = edit.NovaLozinka;
            db.SaveChanges();
            if (edit.Odjava)
            {
                var prijave = await db.Prijave.Where(p=> p.KorisnikId == korisnik.Id).ToListAsync();
                foreach (var prijava in prijave)
                {
                    if (!prijava.JeUredjaj && prijava.Id != prijavainfo.Prijava.Id)
                    {
                        db.Prijave.Remove(prijava);
                        db.SaveChanges();
                    }
                }
            }
            return new EditLozinkaResponse { Editovan = true, Greska = "" };
        }

        [HttpGet]
        public async Task<bool> jePrijavljen()
        {
            var prijavainfo = await auth.getInfo();
            return prijavainfo.jeLogiran;
        }
        [HttpGet]
        public async Task<bool> Odjava ()
        {
            var prijavainfo = await auth.getInfo();
            if (!prijavainfo.jeLogiran) return false;
            var prijava = await db.Prijave.FindAsync(prijavainfo.Prijava.Id);
            if (prijava.JeUredjaj) return false;
            db.Prijave.Remove(prijava);
            db.SaveChanges();
            return true;
        }
    }
    public class PrijavaList
    {
        public int PrijavaId { get; set; }
        public string Datum { get; set; }
        public string Vrijeme { get; set; }
        public string IpAdresa { get; set; }
    }
    public class EditLozinkaReq
    {
        public string StaraLozinka { get; set; }
        public string NovaLozinka { get; set; }
        public bool Odjava { get; set; }
    }
    public class EditLozinkaResponse
    {
        public bool Editovan { get; set; }
        public string Greska { get; set; }
    }
    public class PrijavaResponse
    {
        public bool jeLogiran { get; set; }
        public Prijava? Prijava { get; set; }
    }
    public class PrijavaRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public class KorisnikEdit
    {
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Email { get; set; }
        public string Telefon { get; set; }
        public bool SmsSlanje { get; set; }
        public bool EmailSlanje { get; set; }
    }
    public class KorisnikEditResponse
    {
        public bool Editovan { get; set; }
        public string Greska { get; set; }
    }
    public class PasswordChangeReq
    {
        public string Email { get; set; }
    }
}

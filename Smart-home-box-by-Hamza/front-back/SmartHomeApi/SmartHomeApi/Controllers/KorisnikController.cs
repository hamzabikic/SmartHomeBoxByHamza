using Duende.IdentityServer.ResponseHandling;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartHomeApi.Data;
using SmartHomeApi.Data.Tablice;
using SmartHomeApi.Helpers;
using System.ComponentModel;

namespace SmartHomeApi.Controllers
{
    [ApiController]
    [Route("[action]")]
    public class KorisnikController
    {
        private readonly MyDBContext db;
        private readonly IHttpContextAccessor context;
        private readonly AuthService auth;
        public KorisnikController (MyDBContext _db, IHttpContextAccessor _context, AuthService _auth)
        {
            db = _db;
            context = _context;
            auth = _auth;
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
        public async Task<bool> editujKorisnika(KorisnikEdit edit)
        {
            var prijavainfo = await auth.getInfo();
            if (!prijavainfo.jeLogiran) return false;
            var korisnik = await db.Korisnici.FindAsync(prijavainfo.Prijava.KorisnikId);
            korisnik.Ime = edit.Ime;
            korisnik.Prezime = edit.Prezime;
            korisnik.Email = edit.Email;
            korisnik.BrojTelefona = edit.Telefon;
            korisnik.SmsSlanje = edit.SmsSlanje;
            korisnik.EmailSlanje = edit.EmailSlanje;
            db.SaveChanges();
            return true;
        }
        [HttpGet]
        public async Task<KorisnikEdit?> getInfo ()
        {
            var prijavainfo = await auth.getInfo();
            if (!prijavainfo.jeLogiran) return null;
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
}

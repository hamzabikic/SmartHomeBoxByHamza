using Microsoft.AspNetCore.Mvc;
using SmartHomeApi.Data;
using SmartHomeApi.Data.Tablice;

namespace SmartHomeApi.Controllers
{
    [ApiController]
    [Route("[action]")]
    public class KorisnikController
    {
        private readonly MyDBContext db;
        public KorisnikController (MyDBContext _db)
        {
            db = _db;
        }
        [HttpPost]
        public void dodajKorisnika ()
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
        public bool editujKorisnika(KorisnikEdit edit)
        {
            var korisnik = db.Korisnici.First();
            korisnik.Ime = edit.Ime;
            korisnik.Prezime = edit.Prezime;
            korisnik.Email = edit.Email;
            korisnik.BrojTelefona = edit.Telefon;
            korisnik.SmsSlanje = edit.SmsSlanje;
            korisnik.EmailSlanje = edit.EmailSlanje;
            korisnik.WhatsAppSlanje = edit.WhatsAppSlanje;
            db.SaveChanges();
            return true;
        }
        [HttpGet]
        public KorisnikEdit getInfo ()
        {
            return db.Korisnici.Select(
                k => new KorisnikEdit
                {
                    Ime = k.Ime,
                    Prezime = k.Prezime,
                    Email = k.Email,
                    Telefon = k.BrojTelefona,
                    EmailSlanje = k.EmailSlanje,
                    WhatsAppSlanje = k.WhatsAppSlanje,
                    SmsSlanje = k.SmsSlanje
                }).First();
        }
    }
    public class KorisnikEdit
    {
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Email { get; set; }
        public string Telefon { get; set; }
        public bool SmsSlanje { get; set; }
        public bool WhatsAppSlanje { get; set; }
        public bool EmailSlanje { get; set; }
    }
}

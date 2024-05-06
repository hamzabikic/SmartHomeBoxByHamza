using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartHomeApi.Data;
using SmartHomeApi.Data.Tablice;
using SmartHomeApi.Helpers;

namespace SmartHomeApi.Controllers
{
    [ApiController]
    [Auth]
    [Route("[controller]/[action]")]
    public class SkeniraniPokretiController
    {
        private readonly MyDBContext db;
        private readonly WhatsAppSender sender;
        private readonly SmsSender smsSender;
        private readonly EmailSender emailSender;
        private readonly AuthService auth;
        public SkeniraniPokretiController(MyDBContext _db, WhatsAppSender _sender, SmsSender _smsSender,
            EmailSender _emailSender, AuthService _auth)
        {
            db = _db;
            sender = _sender;
            smsSender = _smsSender;
            emailSender = _emailSender;
            auth = _auth;
        }
        [HttpPost]
        public async Task<bool> addPokret()
        {
            var prijavaInfo = await auth.getInfo();
            var pokret = new SkeniraniPokret { DatumVrijeme = DateTime.Now
                , KorisnikId = prijavaInfo.Prijava.KorisnikId};
            db.SkeniraniPokreti.Add(pokret);
            db.SaveChanges();
            var poruka = "Smart home box by Hamza: Skeniran pokret u prostoru. Molimo posjetite aplikaciju za vise detalja.";
            await emailSender.sendEmail(poruka);
            await sender.sendMessage(poruka);
            await smsSender.sendMessage(poruka);
            return true;
        }
        [HttpGet]
        public async Task<PokretiResponse> getPokretiAll ()
        {
            var prijavaInfo = await auth.getInfo();
            List<PokretResponse> list = await db.SkeniraniPokreti.Where(sp=> sp.KorisnikId == 
            prijavaInfo.Prijava.KorisnikId).OrderByDescending(sp=> sp.DatumVrijeme).
                Select(sp => new PokretResponse
                { Id = sp.Id, Datum = sp.DatumVrijeme.Date.ToString(),
                    Vrijeme = $"{sp.DatumVrijeme.TimeOfDay.Hours}:{sp.DatumVrijeme.TimeOfDay.Minutes}",
                }
                ).ToListAsync();
            return new PokretiResponse { Pokreti = list };
        }
        [HttpGet]
        public async Task<PokretiResponse> getPokretiByDate ([FromQuery] DateTime datum )
        {
            var prijavaInfo = await auth.getInfo();
            List<PokretResponse> list = await db.SkeniraniPokreti.Where(
                sp => sp.KorisnikId == prijavaInfo.Prijava.KorisnikId && sp.DatumVrijeme.Date == datum.Date)
                .OrderByDescending(sp => sp.DatumVrijeme).
                Select(sp => new PokretResponse
                { Id = sp.Id,  Datum = sp.DatumVrijeme.Date.ToString(),
                    Vrijeme = $"{sp.DatumVrijeme.TimeOfDay.Hours}:{sp.DatumVrijeme.TimeOfDay.Minutes}"
                }
                ).ToListAsync();
            return new PokretiResponse { Pokreti = list };

        }
        [HttpGet]
        public async Task<PokretiResponse> getPokretiLast7Days ()
        {
            var prijavaInfo = await auth.getInfo();
            DateTime datum = DateTime.Now.AddDays(-7).Date;
            var lista = await db.SkeniraniPokreti.Where(
                sp => sp.KorisnikId == prijavaInfo.Prijava.KorisnikId && sp.DatumVrijeme.Date >= datum)
                .OrderByDescending(sp => sp.DatumVrijeme).Select(
                sp => new PokretResponse
                {
                    Id = sp.Id,
                    Datum = sp.DatumVrijeme.Date.ToString(),
                    Vrijeme = $"{sp.DatumVrijeme.TimeOfDay.Hours}:{sp.DatumVrijeme.TimeOfDay.Minutes}"
                }).ToListAsync();
            return new PokretiResponse { Pokreti = lista };
        }
        [HttpGet]
        public async Task<PokretiResponse> getPokretiLastMonth()
        {
            var prijavaInfo = await auth.getInfo();
            DateTime datum = DateTime.Now.AddDays(-30).Date;
            var lista = await db.SkeniraniPokreti.Where(
                sp => sp.KorisnikId == prijavaInfo.Prijava.KorisnikId && sp.DatumVrijeme.Date >= datum)
                .OrderByDescending(sp => sp.DatumVrijeme).Select(
                sp => new PokretResponse
                {
                    Id= sp.Id,
                    Datum = sp.DatumVrijeme.Date.ToString(),
                    Vrijeme = $"{sp.DatumVrijeme.TimeOfDay.Hours}:{sp.DatumVrijeme.TimeOfDay.Minutes}"
                }).ToListAsync();
            return new PokretiResponse { Pokreti = lista };
        }
        [HttpPost]
        public async Task<bool> deletePokret([FromQuery] int id)
        {
            SkeniraniPokret pokret = await db.SkeniraniPokreti.FindAsync(id);
            if (pokret == null) return false;
            var prijavaInfo = await auth.getInfo();
            if (pokret.KorisnikId != prijavaInfo.Prijava.KorisnikId) return false;
            db.SkeniraniPokreti.Remove(pokret);
            db.SaveChanges();
            return true;
        }
        
    }
    public class PokretiResponse
    {
        public List<PokretResponse> Pokreti { get; set; }
    }
    public class PokretResponse
    {
        public int Id { get; set; }
        public string Datum { get; set; }
        public string Vrijeme { get; set; }

    }
}

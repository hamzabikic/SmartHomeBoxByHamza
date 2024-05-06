using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using SmartHomeApi.Data;
using SmartHomeApi.Data.Tablice;
using SmartHomeApi.Helpers;
using System.Net.NetworkInformation;
using System.Security.Cryptography.X509Certificates;

namespace SmartHomeApi.Controllers
{
    [ApiController]
    [Auth]
    [Route("[controller]/[action]")]
    public class GasFireController
    {
        private readonly MyDBContext db;
        private readonly WhatsAppSender sender;
        private readonly SmsSender smsSender;
        private readonly EmailSender emailSender;
        private readonly AuthService auth;
        public GasFireController(MyDBContext _db, WhatsAppSender _sender, SmsSender _smsSender,
            EmailSender _emailsender, AuthService _auth)
        {
            db = _db;
            sender = _sender;
            smsSender = _smsSender;
            emailSender = _emailsender;
            auth = _auth;
           
        }
        [HttpPost]
        public async Task<bool> AddInfo(AddInfoRequest req)
        {
            var prijavaInfo = await auth.getInfo();
            var novi = new CO2FireAlarm
            { DatumVrijeme = DateTime.Now, CO2Value = req.GasValue, FireDetected = req.FireDetected,
            KorisnikId = prijavaInfo.Prijava.KorisnikId};
            db.CO2FireAlarmi.Add(novi);
            db.SaveChanges();
            var poruka = "";
            if(req.FireDetected)
            {
                poruka = "Smart home box by Hamza: Detektovan pozar u prostoru. Molimo posjetite aplikaciju za vise detalja.";
            }
            else
            {
                poruka = "Smart home box by Hamza: Detektovano pustanje gasa u prostoru. Molimo posjetite aplikaciju za vise detalja.";
            }
            await emailSender.sendEmail(poruka);
            await sender.sendMessage(poruka);
            await smsSender.sendMessage(poruka);
            return true;
        }
        [HttpGet]
        public async Task<GasFireListResponse> getInfoAll ()
        {
            var prijavaInfo = await auth.getInfo();
            var lista = await db.CO2FireAlarmi.Where(a=> a.KorisnikId == prijavaInfo.Prijava.KorisnikId).
                OrderByDescending(a=> a.DatumVrijeme).Select(
                a => new GasFireResponse
                {
                    Id = a.Id,
                    Datum = a.DatumVrijeme.Date.ToString(),
                    Vrijeme = $"{a.DatumVrijeme.TimeOfDay.Hours}:{a.DatumVrijeme.TimeOfDay.Minutes}",
                    GasValue = a.CO2Value,
                    FireDetected = a.FireDetected
                }).ToListAsync();
            return new GasFireListResponse { Lista = lista };
        }
        [HttpGet]
        public async Task<GasFireListResponse> getInfoByDate([FromQuery] DateTime datum)
        {
            var prijavaInfo = await auth.getInfo();
            var lista = await db.CO2FireAlarmi.Where(a=> a.KorisnikId == prijavaInfo.Prijava.KorisnikId &&
            a.DatumVrijeme.Date == datum.Date)
                .OrderByDescending(a => a.DatumVrijeme).Select(
                a => new GasFireResponse
                {
                    Id = a.Id,
                    Datum = a.DatumVrijeme.Date.ToString(),
                    Vrijeme = $"{a.DatumVrijeme.TimeOfDay.Hours}:{a.DatumVrijeme.TimeOfDay.Minutes}",
                    GasValue = a.CO2Value,
                    FireDetected = a.FireDetected
                }).ToListAsync();
            return new GasFireListResponse { Lista = lista };
        }
        [HttpGet]
        public async Task<GasFireListResponse> getInfoLast7Days()
        {
            var prijavaInfo = await auth.getInfo();
            var datum = DateTime.Now.AddDays(-7);
            var lista = await db.CO2FireAlarmi.Where(a => a.KorisnikId == prijavaInfo.Prijava.KorisnikId &&
            a.DatumVrijeme.Date >= datum.Date)
                .OrderByDescending(a => a.DatumVrijeme).Select(
                a => new GasFireResponse
                {
                    Id = a.Id,
                    Datum = a.DatumVrijeme.Date.ToString(),
                    Vrijeme = $"{a.DatumVrijeme.TimeOfDay.Hours}:{a.DatumVrijeme.TimeOfDay.Minutes}",
                    GasValue = a.CO2Value,
                    FireDetected = a.FireDetected
                }).ToListAsync();
            return new GasFireListResponse { Lista = lista };
        }
        [HttpGet]
        public async Task<GasFireListResponse> getInfoLastMonth()
        {
            var prijavaInfo = await auth.getInfo();
            var datum = DateTime.Now.AddDays(-30);
            var lista = await db.CO2FireAlarmi.Where(a => a.KorisnikId == prijavaInfo.Prijava.KorisnikId &&
            a.DatumVrijeme.Date >= datum.Date)
                .OrderByDescending(a => a.DatumVrijeme).Select(
                a => new GasFireResponse
                {
                    Id = a.Id,
                    Datum = a.DatumVrijeme.Date.ToString(),
                    Vrijeme = $"{a.DatumVrijeme.TimeOfDay.Hours}:{a.DatumVrijeme.TimeOfDay.Minutes}",
                    GasValue = a.CO2Value,
                    FireDetected = a.FireDetected
                }).ToListAsync();
            return new GasFireListResponse { Lista = lista };
        }
        [HttpPost]
        public async Task<bool> deleteInfo([FromQuery]int id)
        {
            CO2FireAlarm value = await db.CO2FireAlarmi.FindAsync(id);
            if (value == null) return false;
            var prijavaInfo = await auth.getInfo();
            if (value.KorisnikId != prijavaInfo.Prijava.KorisnikId) return false;
            db.CO2FireAlarmi.Remove(value);
            db.SaveChanges();
            return true;
        }


    }
    public class AddInfoRequest
    {
        public int GasValue { get; set; }
        public bool FireDetected { get; set; }
    }
    public class GasFireListResponse
    {
        public List<GasFireResponse> Lista { get; set; }
    }
    public class GasFireResponse
    {
        public int Id { get; set; }
        public string Datum { get; set; }
        public string Vrijeme { get; set; }
        public int GasValue { get; set; }
        public bool FireDetected { get; set; }
    }
}

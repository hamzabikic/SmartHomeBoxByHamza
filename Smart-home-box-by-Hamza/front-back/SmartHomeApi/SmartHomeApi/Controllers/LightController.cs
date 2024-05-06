using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartHomeApi.Data;
using SmartHomeApi.Data.Tablice;
using SmartHomeApi.Helpers;
using System.Reflection;

namespace SmartHomeApi.Controllers
{
    [ApiController]
    [Auth]
    [Route("[controller]/[action]")]
    public class LightController
    {
        private readonly MyDBContext db;
        private readonly AuthService auth;
        public LightController(MyDBContext _db, AuthService _auth)
        {
            db = _db;
            auth = _auth;
        }
        [HttpPost]
        public async void startnoVrijeme()
        {
            var prijavaInfo = await auth.getInfo();
            TimeSpan _pocetak = new TimeSpan(0, 0, 0);
            TimeSpan _kraj = new TimeSpan(23, 0, 0);
            Light vrijeme = new Light 
            { Pocetak = _pocetak, Kraj = _kraj, KorisnikId = prijavaInfo.Prijava.KorisnikId  };
            db.Lights.Add(vrijeme);
            db.SaveChanges();
        }
        [HttpPost]
        public async Task<bool> setTime(int pocetak, int kraj)
        {
            var prijavaInfo = await auth.getInfo();
            Light vrijeme = await db.Lights.
                Where(l => l.KorisnikId == prijavaInfo.Prijava.KorisnikId).FirstOrDefaultAsync();
            TimeSpan _pocetak = new TimeSpan(pocetak, 0, 0);
            TimeSpan _kraj = new TimeSpan(kraj, 0, 0);
            vrijeme.Pocetak = _pocetak;
            vrijeme.Kraj = _kraj;
            db.SaveChanges();
            return true;
        }
        [HttpGet]
        public async Task<bool> jeUpaljeno ()
        {
            var prijavaInfo = await auth.getInfo();
            Light vrijeme = await db.Lights.
                Where(l => l.KorisnikId == prijavaInfo.Prijava.KorisnikId).FirstOrDefaultAsync();
            if (vrijeme.Pocetak == vrijeme.Kraj) return true;
            TimeSpan trenutno = DateTime.Now.TimeOfDay;
            if (vrijeme.Pocetak < vrijeme.Kraj)
            {
                if (trenutno >= vrijeme.Pocetak && trenutno <= vrijeme.Kraj) return true;
            }
            if(vrijeme.Pocetak >vrijeme.Kraj)
            {
                if(trenutno < vrijeme.Kraj)
                {
                    return true;
                }
                if(trenutno>=vrijeme.Kraj)
                {
                    if(trenutno >= vrijeme.Pocetak)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        [HttpGet]
        public async Task<TimeResponse> getTime ()
        {
            var prijavaInfo = await auth.getInfo();
            Light vrijeme = await db.Lights.
                Where(l => l.KorisnikId == prijavaInfo.Prijava.KorisnikId).FirstOrDefaultAsync();
            return new TimeResponse { Pocetak = vrijeme.Pocetak.Hours, Kraj = vrijeme.Kraj.Hours };
        }

    }
    public class TimeResponse
    {
        public int Pocetak { get; set; }
        public int Kraj { get; set; }
    }
}

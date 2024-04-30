using Microsoft.AspNetCore.Mvc;
using SmartHomeApi.Data;
using SmartHomeApi.Data.Tablice;
using System.Reflection;

namespace SmartHomeApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class LightController
    {
        private readonly MyDBContext db;
        public LightController(MyDBContext _db)
        {
            db = _db;
        }
        [HttpPost]
        public void startnoVrijeme()
        {
            TimeSpan _pocetak = new TimeSpan(0, 0, 0);
            TimeSpan _kraj = new TimeSpan(23, 0, 0);
            Light vrijeme = new Light { Pocetak = _pocetak, Kraj = _kraj };
            db.Lights.Add(vrijeme);
            db.SaveChanges();
        }
        [HttpPost]
        public bool setTime(int pocetak, int kraj)
        {
            Light vrijeme = db.Lights.First();
            TimeSpan _pocetak = new TimeSpan(pocetak, 0, 0);
            TimeSpan _kraj = new TimeSpan(kraj, 0, 0);
            vrijeme.Pocetak = _pocetak;
            vrijeme.Kraj = _kraj;
            db.SaveChanges();
            return true;
        }
        [HttpGet]
        public bool jeUpaljeno ()
        {
            Light vrijeme = db.Lights.First();
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
        public TimeResponse getTime ()
        {
            Light vrijeme = db.Lights.First();
            return new TimeResponse { Pocetak = vrijeme.Pocetak.Hours, Kraj = vrijeme.Kraj.Hours };
        }

    }
    public class TimeResponse
    {
        public int Pocetak { get; set; }
        public int Kraj { get; set; }
    }
}

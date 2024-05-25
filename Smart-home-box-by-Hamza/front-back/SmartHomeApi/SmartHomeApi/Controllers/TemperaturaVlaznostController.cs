using Duende.IdentityServer.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SmartHomeApi.Data;
using SmartHomeApi.Data.Tablice;
using SmartHomeApi.Helpers;

namespace SmartHomeApi.Controllers
{
    [ApiController]
    [Auth]
    [Route("[controller]/[action]")]
    public class TemperaturaVlaznostController
    {
        private readonly MyDBContext db;
        private readonly AuthService auth;
        public TemperaturaVlaznostController(MyDBContext _db, AuthService _auth)
        {
            db = _db;
            auth = _auth;
        }
        [HttpPost]
        public async Task<bool> addInfo([FromBody] InfoRequest info)
        {
            var prijavaInfo = await auth.getInfo();
            if (!prijavaInfo.Prijava.JeUredjaj) return false;
            var novi = new TemperaturaVlaznost
            {
                DatumVrijeme = DateTime.Now,
                Temperatura = info.Temperatura,
                Vlaznost = info.Vlaznost, 
                KorisnikId = prijavaInfo.Prijava.KorisnikId
            };
            db.TemperatureVlaznosti.Add(novi);
            db.SaveChanges();
            return true;
        }
        [HttpGet]
        public async Task<InfoListResponse> getInfoByDate (DateTime datum)
        {
            var prijavaInfo = await auth.getInfo();
            var lista = await db.TemperatureVlaznosti.Where(tv =>
            tv.KorisnikId == prijavaInfo.Prijava.KorisnikId && tv.DatumVrijeme.Date == datum.Date)
                .OrderByDescending(tv=> tv.DatumVrijeme).Select(
                tv => new InfoResponse
                {
                    Datum = tv.DatumVrijeme.Date.ToString(),
                    Vrijeme = $"{tv.DatumVrijeme.TimeOfDay.Hours}:{tv.DatumVrijeme.TimeOfDay.Minutes}",
                    Temperatura = tv.Temperatura,
                    Vlaznost = tv.Vlaznost,
                    Id = tv.Id
                }
                ).ToListAsync();
            return new InfoListResponse { InfoLista = lista };

        }
        [HttpGet]
        public async Task<InfoListResponse> getInfoLast7Days()
        {
            var prijavaInfo = await auth.getInfo();
            var datum = DateTime.Now.AddDays(-7);
            var lista = await db.TemperatureVlaznosti.Where(tv =>
            tv.KorisnikId == prijavaInfo.Prijava.KorisnikId && tv.DatumVrijeme.Date >= datum.Date)
                .OrderByDescending(tv => tv.DatumVrijeme).Select(
                tv => new InfoResponse
                {
                    Datum = tv.DatumVrijeme.Date.ToString(),
                    Vrijeme = $"{tv.DatumVrijeme.TimeOfDay.Hours}:{tv.DatumVrijeme.TimeOfDay.Minutes}",
                    Temperatura = tv.Temperatura,
                    Vlaznost = tv.Vlaznost,
                    Id = tv.Id
                }
                ).ToListAsync();
            return new InfoListResponse { InfoLista = lista };
        }
        [HttpGet]
        public async Task<ChartResponse> getTemperatureChartToday (bool isTemperature)
        {
            var prijavaInfo = await auth.getInfo();
            var datum = DateTime.Now;
            var labels = new List<string>();
            var data = new List<double>();
            var lista = await db.TemperatureVlaznosti.Where(tv =>
            tv.KorisnikId == prijavaInfo.Prijava.KorisnikId && tv.DatumVrijeme.Date == datum.Date)
                .OrderBy(tv => tv.DatumVrijeme)
                .ToListAsync();
            await Task.Run(() =>
            {
                foreach (var temphum in lista)
                {
                    if (temphum.DatumVrijeme.Minute != 0) continue;
                    labels.Add(temphum.DatumVrijeme.Hour + ":" + temphum.DatumVrijeme.Minute);
                    if (isTemperature)
                    {
                        data.Add(temphum.Temperatura);
                    }
                    else
                    {
                        data.Add(temphum.Vlaznost);
                    }
                }
            });
            return new ChartResponse
            {
                Data = data,
                Labels = labels
            };
        }
        [HttpGet]
        public async Task<ChartResponse> getTemperatureChartLast7Days(bool isTemperature)
        {
            var prijavaInfo = await auth.getInfo();
            var datum = DateTime.Now.AddDays(-7);
            var labels = new List<string>();
            var data = new List<double>();
            var lista = await db.TemperatureVlaznosti.Where(tv =>
            tv.KorisnikId == prijavaInfo.Prijava.KorisnikId && tv.DatumVrijeme.Date >= datum.Date)
                .OrderBy(tv => tv.DatumVrijeme)
                .ToListAsync();
            await Task.Run(() =>
            {
                DateTime trenutni = DateTime.Now.Date.AddDays(1);
                foreach (var temphum in lista)
                {
                    if(temphum.DatumVrijeme.Date == trenutni)
                    {
                        continue;
                    }
                    else
                    {
                        trenutni = temphum.DatumVrijeme.Date;
                    }
                    labels.Add(temphum.DatumVrijeme.Date.ToShortDateString()) ;
                    int brojac = 0;
                    int sum = 0;
                    foreach (var temphum2 in lista)
                    {
                        if (temphum2.DatumVrijeme.Date == trenutni)
                        {
                            if (isTemperature == true)
                            {
                                sum += temphum2.Temperatura;
                                brojac++;
                            }
                            else
                            {
                                sum += temphum2.Vlaznost;
                                brojac++;
                            }
                        }
                    }
                    data.Add(Math.Round((float)sum / brojac,1));
                }
            });
            return new ChartResponse
            {
                Data = data,
                Labels = labels
            };
        }
        [HttpGet]
        public async Task<ChartResponse> getTemperatureChartLastMonth(bool isTemperature)
        {
            var prijavaInfo = await auth.getInfo();
            var datum = DateTime.Now.AddDays(-30);
            var labels = new List<string>();
            var data = new List<double>();
            var lista = await db.TemperatureVlaznosti.Where(tv =>
            tv.KorisnikId == prijavaInfo.Prijava.KorisnikId && tv.DatumVrijeme.Date >= datum.Date)
                .OrderBy(tv => tv.DatumVrijeme)
                .ToListAsync();
            await Task.Run(() =>
            {
                DateTime trenutni = DateTime.Now.Date.AddDays(1);
                foreach (var temphum in lista)
                {
                    if (temphum.DatumVrijeme.Date == trenutni)
                    {
                        continue;
                    }
                    else
                    {
                        trenutni = temphum.DatumVrijeme.Date;
                    }
                    labels.Add(temphum.DatumVrijeme.Date.ToShortDateString());
                    int brojac = 0;
                    int sum = 0;
                    foreach (var temphum2 in lista)
                    {
                        if (temphum2.DatumVrijeme.Date == trenutni)
                        {
                            if (isTemperature == true)
                            {
                                sum += temphum2.Temperatura;
                                brojac++;
                            }
                            else
                            {
                                sum += temphum2.Vlaznost;
                                brojac++;
                            }
                        }
                    }
                    data.Add(Math.Round((float)sum / brojac, 1));
                }
            });
            return new ChartResponse
            {
                Data = data,
                Labels = labels
            };
        }
        [HttpGet]
        public async Task<InfoListResponse> getInfoLastMonth()
        {
            var prijavaInfo = await auth.getInfo();
            var datum = DateTime.Now.AddDays(-30);
            var lista = await db.TemperatureVlaznosti.Where(tv => 
            tv.KorisnikId == prijavaInfo.Prijava.KorisnikId && tv.DatumVrijeme.Date >= datum.Date)
                .OrderByDescending(tv => tv.DatumVrijeme).Select(
                tv => new InfoResponse
                {
                    Datum = tv.DatumVrijeme.Date.ToString(),
                    Vrijeme = $"{tv.DatumVrijeme.TimeOfDay.Hours}:{tv.DatumVrijeme.TimeOfDay.Minutes}",
                    Temperatura = tv.Temperatura,
                    Vlaznost = tv.Vlaznost,
                    Id = tv.Id
                }
                ).ToListAsync();
            return new InfoListResponse { InfoLista = lista };
        }



    }
    public class ChartResponse
    {
        public List<string> Labels { get; set; }
        public List<double> Data { get; set; }
    }
    public class InfoRequest
    {
        public int Temperatura { get; set; }
        public int Vlaznost { get; set; }
    }
    public class InfoListResponse
    {
        public List<InfoResponse> InfoLista { get; set; }
    }
    public class InfoResponse
    {
        public int Id { get; set; }
        public string Datum { get; set; }
        public string Vrijeme { get; set; }
        public int Temperatura { get; set; }
        public int Vlaznost { get; set; }
    }
}

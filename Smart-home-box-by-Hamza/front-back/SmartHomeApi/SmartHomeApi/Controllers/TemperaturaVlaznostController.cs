﻿using Duende.IdentityServer.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartHomeApi.Data;
using SmartHomeApi.Data.Tablice;

namespace SmartHomeApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class TemperaturaVlaznostController
    {
        private readonly MyDBContext db;
        public TemperaturaVlaznostController(MyDBContext _db)
        {
            db = _db;
        }
        [HttpPost]
        public bool addInfo([FromBody] InfoRequest info)
        {
            var novi = new TemperaturaVlaznost
            {
                DatumVrijeme = DateTime.Now,
                Temperatura = info.Temperatura,
                Vlaznost = info.Vlaznost
            };
            db.TemperatureVlaznosti.Add(novi);
            db.SaveChanges();
            return true;
        }
        [HttpGet]
        public async Task<InfoListResponse> getInfoByDate (DateTime datum)
        {
            var lista = await db.TemperatureVlaznosti.Where(tv => tv.DatumVrijeme.Date == datum.Date)
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
            var datum = DateTime.Now.AddDays(-7);
            var lista = await db.TemperatureVlaznosti.Where(tv => tv.DatumVrijeme.Date >= datum.Date)
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
        public async Task<InfoListResponse> getInfoLastMonth()
        {
            var datum = DateTime.Now.AddDays(-30);
            var lista = await db.TemperatureVlaznosti.Where(tv => tv.DatumVrijeme.Date >= datum.Date)
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

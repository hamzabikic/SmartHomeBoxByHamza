using Microsoft.EntityFrameworkCore;
using SmartHomeApi.Data.Tablice;

namespace SmartHomeApi.Data
{
    public class MyDBContext : DbContext
    {
        public MyDBContext(DbContextOptions contextOptions) :base(contextOptions)
        {

        }
        public DbSet<TemperaturaVlaznost> TemperatureVlaznosti { get; set; }
        public DbSet<CO2FireAlarm> CO2FireAlarmi { get; set; }
        public DbSet<SkeniraniPokret> SkeniraniPokreti { get; set; }
        public DbSet<Light> Lights { get; set; }
        public DbSet<Korisnik> Korisnici { get; set; }
    }
}

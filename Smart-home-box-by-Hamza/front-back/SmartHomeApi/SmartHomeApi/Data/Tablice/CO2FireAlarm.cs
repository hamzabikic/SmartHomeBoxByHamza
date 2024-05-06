using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHomeApi.Data.Tablice
{
    [Table("CO2FireAlarmi")]
    public class CO2FireAlarm
    {
        public int Id { get; set; }
        [ForeignKey(nameof(Korisnik))]
        public int KorisnikId { get; set; }
        public Korisnik Korisnik { get; set; }
        public DateTime DatumVrijeme { get; set; }
        public int CO2Value { get; set; }
        public bool FireDetected { get; set; }
    }
}

using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHomeApi.Data.Tablice
{
    [Table("Lights")]
    public class Light
    {
        public int Id { get; set; }
        [ForeignKey(nameof(Korisnik))]
        public int KorisnikId { get; set; }
        public Korisnik Korisnik { get; set; }
        public TimeSpan Pocetak { get; set; }
        public TimeSpan Kraj { get; set; }
    }
}

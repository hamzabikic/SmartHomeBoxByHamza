using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHomeApi.Data.Tablice
{
    [Table("SkeniraniPokreti")]
    public class SkeniraniPokret
    {
        public int Id { get; set; }
        [ForeignKey(nameof(Korisnik))]
        public int KorisnikId { get; set; }
        public Korisnik Korisnik { get; set; }
        public DateTime DatumVrijeme { get; set; }
    }
}

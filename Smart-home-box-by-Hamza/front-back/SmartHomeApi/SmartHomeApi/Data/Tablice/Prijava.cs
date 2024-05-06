using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHomeApi.Data.Tablice
{
    public class Prijava
    {
        public int Id { get; set; }
        [ForeignKey(nameof(Korisnik))]
        public int KorisnikId { get; set; }
        public Korisnik Korisnik { get; set; }
        public bool JeUredjaj { get; set; }
        public string Token { get; set; }
        public DateTime DatumVrijeme { get; set; }
        public string IpAdresa { get; set; }
    }
}

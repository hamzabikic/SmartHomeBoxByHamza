using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHomeApi.Data.Tablice
{
    [Table("Korisnici")]
    public class Korisnik
    {
        public int Id { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public bool SmsSlanje { get; set; }
        public bool WhatsAppSlanje { get; set; }
        public bool EmailSlanje { get; set; }
        public string BrojTelefona { get; set; }
        public string Email { get; set; }
    }
}

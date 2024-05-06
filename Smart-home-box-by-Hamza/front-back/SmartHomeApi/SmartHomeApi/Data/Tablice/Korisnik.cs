using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHomeApi.Data.Tablice
{
    [Table("Korisnici")]
    public class Korisnik
    {
        [JsonIgnore]
        public int Id { get; set; }
        [JsonIgnore]
        public string Ime { get; set; }
        [JsonIgnore]
        public string Prezime { get; set; }
        public string Username { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
        [JsonIgnore]
        public bool SmsSlanje { get; set; }
        [JsonIgnore]
        public bool EmailSlanje { get; set; }
        [JsonIgnore]
        public string BrojTelefona { get; set; }
        [JsonIgnore]
        public string Email { get; set; }
    }
}

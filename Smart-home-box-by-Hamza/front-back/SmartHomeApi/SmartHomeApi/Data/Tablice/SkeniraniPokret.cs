using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHomeApi.Data.Tablice
{
    [Table("SkeniraniPokreti")]
    public class SkeniraniPokret
    {
        public int Id { get; set; }
        public DateTime DatumVrijeme { get; set; }
    }
}

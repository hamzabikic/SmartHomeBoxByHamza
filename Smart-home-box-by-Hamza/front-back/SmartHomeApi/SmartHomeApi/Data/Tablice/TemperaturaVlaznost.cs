using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHomeApi.Data.Tablice
{
    [Table("TemperatureVlaznosti")]
    public class TemperaturaVlaznost
    {
        public int Id { get; set; }
        public DateTime DatumVrijeme { get; set; }
        public int Temperatura { get; set; }
        public int Vlaznost { get; set; }
    }
}

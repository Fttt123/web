using System.ComponentModel.DataAnnotations;

namespace BajajWeb.Domain.Entity
{
    public class FinalCharacteristics
    {
        public string characteristic_name { get; set; }
        public string category_name { get; set; }
        public int id_motorcycle_model { get; set; }
        public string value { get; set; }
        public string unit { get; set; }
    }
}

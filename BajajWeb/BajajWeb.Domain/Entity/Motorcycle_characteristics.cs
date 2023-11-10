namespace BajajWeb.Domain.Entity
{
    public class Motorcycle_characteristics
    {
        public int id_characteristic { get; set; }
        public int id_motorcycle_model { get; set; }
        public string value { get; set; }
        public int? id_unit { get; set; }
    }
}

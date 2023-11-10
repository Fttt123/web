using System.Collections.Generic;

namespace BajajWeb.Domain.Entity
{
    public class FinalModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string mark { get; set; }
        public int years_of_release { get; set; }
        public decimal retail_price { get; set; }
        public string description { get; set; }
        public int priority { get; set; }
        public List<Motorcycles> motorcycles { get; set; }
        public List<Photos> photos { get; set; }
        public List<FinalCharacteristics> characteristics { get; set; }
    }
}

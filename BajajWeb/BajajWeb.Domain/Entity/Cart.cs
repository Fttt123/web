using System.ComponentModel.DataAnnotations.Schema;

namespace BajajWeb.Domain.Entity
{
    public class Cart
    {
        public int id { get; set; }
        public int user_id { get; set; }
        public int motorcycle_id { get; set; }
        public int status_id { get; set; }
        public int count { get; set; }
        [NotMapped]
        public FinalModel model { get; set; }
    }
}

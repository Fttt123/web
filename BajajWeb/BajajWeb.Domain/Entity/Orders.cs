using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BajajWeb.Domain.Entity
{
    public class Orders
    {
        public int id { get; set; }
        public DateTime date { get; set; }
        public int id_motorcycle { get; set; }
        public int id_user_buyer { get; set; }
        public int id_order_status { get; set; }
        public int id_payment_method { get; set; }
        public decimal final_price { get; set; }
    }
}

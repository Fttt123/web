using System.ComponentModel.DataAnnotations;

namespace BajajWeb.Domain.ViewModels.Order
{
    public class OrderViewModel
    {
        public int id_motorcycle { get; set; }
        [Required(ErrorMessage = "Укажите способ оплаты")]
        public int id_payment_method { get; set; }
    }
}

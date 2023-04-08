using WebApplication4.Areas.Identity.Data;

namespace WebApplication4.Models.ViewModels
{
    public class CartViewModel
    {
        public List<CartItems> CartItems { get; set; }
        public decimal GrandTotal { get; set; }
    }
}

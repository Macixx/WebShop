using Microsoft.AspNetCore.Mvc;
using WebApplication4.Models;
using WebApplication4.Areas.Identity.Data;
using WebApplication4.Models.ViewModels;
using WebApplication4.Infrastructure;

namespace WebApplication4.Views.Components
{
    public class SmallCartViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            List<CartItems> cart = HttpContext.Session.GetJson<List<CartItems>>("Cart");
            SmallCartViewModel smallCartVM;

            if (cart == null || cart.Count == 0)
            {
                smallCartVM = null;
            }
            else
            {
                smallCartVM = new()
                {
                    NumberOfItems = cart.Sum(x => x.Quantity),
                    TotalAmount = cart.Sum(x => x.Quantity * x.Price)
                };
            }

            return View(smallCartVM);
        }
    }
}

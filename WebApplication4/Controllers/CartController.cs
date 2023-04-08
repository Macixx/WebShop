using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebApplication4.Areas.Identity.Data;
using WebApplication4.Infrastructure;
using WebApplication4.Models.ViewModels;

namespace WebApplication4.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext _context;

        public CartController(AppDbContext context)
        {
            _context = context;
        }
        [Route("/koszyk")]
        public IActionResult Index()
        {
            List<CartItems> cart = HttpContext.Session.GetJson<List<CartItems>>("Cart") ?? new List<CartItems>();

            CartViewModel cartVM = new()
            {
                CartItems = cart,
                GrandTotal = cart.Sum(x => x.Quantity * x.Price)
            };

            return View(cartVM);
        }

        public async Task<IActionResult> Add(int id)
        {
            ShopItem product = await _context.ShopItems.FindAsync(id);
            List<CartItems> cart = HttpContext.Session.GetJson<List<CartItems>>("Cart") ?? new List<CartItems>();
            CartItems cartItem = cart.Where(c => c.Id == id).FirstOrDefault();

            if (cartItem == null)
            {
                cart.Add(new CartItems(product));
            }
            else
            {
                cartItem.Quantity += 1;
            }

            HttpContext.Session.SetJson("Cart", cart);

            TempData["Success"] = "The product has been added!";

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Decrease(int id)
        {
            List<CartItems> cart = HttpContext.Session.GetJson<List<CartItems>>("Cart");

            CartItems cartItem = cart.Where(c => c.Id == id).FirstOrDefault();

            if (cartItem.Quantity > 1)
            {
                --cartItem.Quantity;
            }
            else
            {
                cart.RemoveAll(p => p.Id == id);
            }

            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }

            TempData["Success"] = "The product has been removed!";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Remove(int id)
        {
            List<CartItems> cart = HttpContext.Session.GetJson<List<CartItems>>("Cart");

            cart.RemoveAll(p => p.Id == id);

            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
                

            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }

            TempData["Success"] = "The product has been removed!";

            return RedirectToAction("Index");
        }

        public IActionResult Clear()
        {
            HttpContext.Session.Remove("Cart");

            return RedirectToAction("Index");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication4.Areas.Identity.Data;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class ShopItemsController : Controller
    {
        private readonly AppDbContext _context;

        public ShopItemsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: ShopItems
        [Route("/")]
        public async Task<IActionResult> Index(string sortOrder)
        {
            ViewData["PopularitySortParm"] = String.IsNullOrEmpty(sortOrder) ? "" : "";

            var shopItems = from s in _context.ShopItems
                            select s;
            switch (sortOrder)
            {
                default:
                    shopItems = shopItems.OrderByDescending(s => s.Sales);
                    break;
            }
            return View(await shopItems.AsNoTracking().ToListAsync());
        }

        [Route("/szukaj")]
        public async Task<IActionResult> Produkty(string c, string sortOrder, string searchString, int? pageNumber, string producerFilter)
        {




            ViewData["DefaultSortParm"] = String.IsNullOrEmpty(sortOrder) ? "" : "";
            ViewData["PriceSortParm"] = sortOrder == "price" ? "price_desc" : "price";
            ViewData["NameSortParm"] = sortOrder == "name" ? "name_desc" : "name";

           
            var shopItems = from s in _context.ShopItems
                            select s;

            if (!String.IsNullOrEmpty(c))
            {
                if (!String.IsNullOrEmpty(searchString))
                {

                    if (!String.IsNullOrEmpty(producerFilter))
                    {

                        shopItems = shopItems.Where(s => s.ItemName.Contains(searchString) && s.Producer.Contains(producerFilter) && s.Category.Contains(c) || s.Category.Contains(c) && s.Producer.Contains(producerFilter));


                    }
                    else
                    {
                        shopItems = shopItems.Where(s => s.ItemName.Contains(searchString) && s.Category.Contains(c) || s.Category.Contains(c) || s.Producer.Contains(searchString) && s.Category.Contains(c));


                    }




                }
                else
                {
                    if (!String.IsNullOrEmpty(producerFilter))
                    {

                        shopItems = shopItems.Where(s => s.Producer.Contains(producerFilter) && s.Category.Contains(c) || s.Category.Contains(c) && s.Producer.Contains(producerFilter));


                    }
                    else
                    {
                        shopItems = shopItems.Where(s => s.Category.Contains(c));


                    }


                }
            }
            else
            {
                if (!String.IsNullOrEmpty(searchString))
                {

                    if (!String.IsNullOrEmpty(producerFilter))
                    {

                        shopItems = shopItems.Where(s => s.ItemName.Contains(searchString) && s.Producer.Contains(producerFilter) || s.Category.Contains(searchString) && s.Producer.Contains(producerFilter));


                    }
                    else
                    {
                        shopItems = shopItems.Where(s => s.ItemName.Contains(searchString) || s.Category.Contains(searchString) || s.Producer.Contains(searchString));


                    }




                }
                else
                {

                    Response.Redirect("/");

                }


            }







            switch (sortOrder)
            {
                case "price_desc":
                    shopItems = shopItems.OrderByDescending(s => s.ItemPrice);
                    break;
                case "price":
                    shopItems = shopItems.OrderBy(s => s.ItemPrice);
                    break;
                case "name_desc":
                    shopItems = shopItems.OrderByDescending(s => s.ItemName);
                    break;
                case "name":
                    shopItems = shopItems.OrderBy(s => s.ItemName);
                    break;
                default:
                    shopItems = shopItems.OrderByDescending(s => s.Sales);
                    break;
            }
            ViewData["searchString"] = searchString;
            ViewData["Query"] = HttpContext.Request.QueryString;
            ViewData["SortOrder"] = sortOrder;
            ViewData["pageNumber"] = pageNumber;
            ViewData["producerFilter"] = producerFilter;
            ViewData["category"] = c;



            int pageSize = 32;
            return View(await PaginatedList<ShopItem>.CreateAsync(shopItems.AsNoTracking(), pageNumber ?? 1, pageSize));

        }




        // GET: ShopItems/Details/5
        [Route("/produkt")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ShopItems == null)
            {
                Response.Redirect("/");
            }

            var shopItem = await _context.ShopItems
                .FirstOrDefaultAsync(m => m.ID == id);
            if (shopItem == null)
            {
                Response.Redirect("/");
            }

            return View(shopItem);
        }

        // GET: ShopItems/Create
        public IActionResult Create()
        {
            return View();
            //return Redirect("/");
        }

        // POST: ShopItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,ItemName,Count,ItemImage,ItemPrice,Category,Producer,whenAdded")] ShopItem shopItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shopItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
             return View(shopItem);
            //return Redirect("/");
        }

        // GET: ShopItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ShopItems == null)
            {
                return NotFound();
            }

            var shopItem = await _context.ShopItems.FindAsync(id);
            if (shopItem == null)
            {
                return NotFound();
            }
            //return View(shopItem);
            return Redirect("/");
        }

        // POST: ShopItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,ItemName,Count,ItemImage,ItemPrice,Category,Producer")] ShopItem shopItem)
        {
            if (id != shopItem.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shopItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShopItemExists(shopItem.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            //return View(shopItem);
            return Redirect("/");
        }

        // GET: ShopItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ShopItems == null)
            {
                return NotFound();
            }

            var shopItem = await _context.ShopItems
                .FirstOrDefaultAsync(m => m.ID == id);
            if (shopItem == null)
            {
                return NotFound();
            }

            //return View(shopItem);
            return Redirect("/");
        }

        // POST: ShopItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ShopItems == null)
            {
                return Problem("Entity set 'AppDbContext.ShopItems'  is null.");
            }
            var shopItem = await _context.ShopItems.FindAsync(id);
            if (shopItem != null)
            {
                _context.ShopItems.Remove(shopItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShopItemExists(int id)
        {
            return _context.ShopItems.Any(e => e.ID == id);
        }
    }
}

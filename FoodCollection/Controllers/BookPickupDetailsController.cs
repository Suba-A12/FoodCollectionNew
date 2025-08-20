using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FoodCollection.Data;
using FoodCollection.Models;

namespace FoodCollection.Controllers
{
    public class BookPickupDetailsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookPickupDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BookPickupDetails
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.BookPickupDetail.Include(b => b.BookPickup).Include(b => b.FoodItem);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: BookPickupDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookPickupDetail = await _context.BookPickupDetail
                .Include(b => b.BookPickup)
                .Include(b => b.FoodItem)
                .FirstOrDefaultAsync(m => m.BookPickupDetailId == id);
            if (bookPickupDetail == null)
            {
                return NotFound();
            }

            return View(bookPickupDetail);
        }
        //public IActionResult Create()
        //{
        //    ViewData["BookPickupId"] = new SelectList(_context.BookPickup, "BookPickupId", "BookPickupId");
        //    ViewData["FoodItemId"] = new SelectList(_context.FoodItem, "FoodItemId", "FoodItemId");
        //    return View();
        //}

        // GET: BookPickupDetails/Create
        public IActionResult Create(int bookPickupId)
        {
            var model = new BookPickupDetail
            {
                BookPickupId = bookPickupId
            };
            ViewBag.FoodItemList = new SelectList(_context.FoodItem, "FoodItemId", "FoodType");

            return View(model);
        }

        // POST: BookPickupDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookPickupDetail bookPickupDetail, string ItemList)
        {
            if (string.IsNullOrEmpty(ItemList))
            {
                ModelState.AddModelError("", "Please add at least one food item.");
                ViewBag.FoodItemList = new SelectList(_context.FoodItem, "FoodItemId", "FoodType", bookPickupDetail.FoodItemId);
                return View(bookPickupDetail);
            }
            // Split items (each line = one item)
            var items = ItemList.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            foreach (var item in items)
            {
                // item looks like: "2 - Rice (Expiry: 2025-09-01)"
                try
                {
                    // Extract parts (very basic parsing)
                    var qtyPart = item.Split('-')[0].Trim();
                    var rest = item.Split('-')[1].Trim();

                    var foodName = rest.Substring(0, rest.IndexOf("(Expiry")).Trim();
                    var expiryText = rest.Substring(rest.IndexOf(":") + 1).Replace(")", "").Trim();

                    var newDetail = new BookPickupDetail
                    {
                        BookPickupId = bookPickupDetail.BookPickupId,
                        QuantityLeft = int.Parse(qtyPart),
                        ExpiryDate = DateOnly.Parse(expiryText),
                        FoodItemId = _context.FoodItem
                                            .Where(f => f.FoodType == foodName)
                                            .Select(f => f.FoodItemId)
                                            .FirstOrDefault()
                    };
                    _context.Add(newDetail);
                }
                catch
                {
                    // If parsing fails, skip or log
                }
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Confirmation", "Payments", new { bookPickupId = bookPickupDetail.BookPickupId });
        }
        //public async Task<IActionResult> Create([Bind("BookPickupDetailId,QuantityLeft,ExpiryDate,BookPickupId,FoodItemId")] BookPickupDetail bookPickupDetail)
        //{
        //    //if (ModelState.IsValid)
        //    //{
        //        _context.Add(bookPickupDetail);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction("Create", "Payments", new { bookPickupId = bookPickupDetail.BookPickupId });
        //    //return RedirectToAction(nameof(Index));
        //    //}
        //    //ViewData["BookPickupId"] = new SelectList(_context.BookPickup, "BookPickupId", "BookPickupId", bookPickupDetail.BookPickupId);
        //    //ViewData["FoodItemId"] = new SelectList(_context.FoodItem, "FoodItemId", "FoodItemId", bookPickupDetail.FoodItemId);
        //    ViewBag.FoodItemList = new SelectList(_context.FoodItem, "FoodItemId", "FoodType", bookPickupDetail.FoodItemId);
        //    return View(bookPickupDetail);
        //}

        // GET: BookPickupDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookPickupDetail = await _context.BookPickupDetail.FindAsync(id);
            if (bookPickupDetail == null)
            {
                return NotFound();
            }
            ViewData["BookPickupId"] = new SelectList(_context.BookPickup, "BookPickupId", "BookPickupId", bookPickupDetail.BookPickupId);
            ViewData["FoodItemId"] = new SelectList(_context.FoodItem, "FoodItemId", "FoodItemId", bookPickupDetail.FoodItemId);
            return View(bookPickupDetail);
        }

        // POST: BookPickupDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookPickupDetailId,QuantityLeft,ExpiryDate,BookPickupId,FoodItemId")] BookPickupDetail bookPickupDetail)
        {
            if (id != bookPickupDetail.BookPickupDetailId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bookPickupDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookPickupDetailExists(bookPickupDetail.BookPickupDetailId))
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
            ViewData["BookPickupId"] = new SelectList(_context.BookPickup, "BookPickupId", "BookPickupId", bookPickupDetail.BookPickupId);
            ViewData["FoodItemId"] = new SelectList(_context.FoodItem, "FoodItemId", "FoodItemId", bookPickupDetail.FoodItemId);
            return View(bookPickupDetail);
        }

        // GET: BookPickupDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookPickupDetail = await _context.BookPickupDetail
                .Include(b => b.BookPickup)
                .Include(b => b.FoodItem)
                .FirstOrDefaultAsync(m => m.BookPickupDetailId == id);
            if (bookPickupDetail == null)
            {
                return NotFound();
            }

            return View(bookPickupDetail);
        }

        // POST: BookPickupDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bookPickupDetail = await _context.BookPickupDetail.FindAsync(id);
            if (bookPickupDetail != null)
            {
                _context.BookPickupDetail.Remove(bookPickupDetail);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookPickupDetailExists(int id)
        {
            return _context.BookPickupDetail.Any(e => e.BookPickupDetailId == id);
        }
    }
}

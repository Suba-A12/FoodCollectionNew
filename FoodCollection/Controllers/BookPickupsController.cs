using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FoodCollection.Data;
using FoodCollection.Models;
using Microsoft.AspNetCore.Authorization;

namespace FoodCollection.Controllers
{
    public class BookPickupsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookPickupsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> MyBookings()
        {
            var username = User.Identity.Name;
            var getBookings = await _context.BookPickup.Include(b => b.Customer).Where(b => b.Customer.Email == username).ToListAsync();
            return View(getBookings);
        }

        // GET: BookPickups
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.BookPickup.Include(b => b.Customer);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: BookPickups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookPickup = await _context.BookPickup
                .Include(b => b.Customer)
                .FirstOrDefaultAsync(m => m.BookPickupId == id);
            if (bookPickup == null)
            {
                return NotFound();
            }

            return View(bookPickup);
        }

        // GET: BookPickups/Create
        public IActionResult Create()
        {
            //ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "CustomerId");
            return View();
        }

        // POST: BookPickups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookPickupId,EventName,EventAddress,BookDate,BookTime,BookStatus,TotalAmount,PaymentStatus,CustomerId")] BookPickup bookPickup)
        {
            //if (ModelState.IsValid)
            //{
            //    _context.Add(bookPickup);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //    return RedirectToAction("Create", "BookPickupDetails", new { bookPickupId = bookPickup.BookPickupId });
            //}
            //ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "CustomerId", bookPickup.CustomerId);
            //return View(bookPickup);

            var username = User.Identity.Name;
            var customer = await _context.Customer.FirstOrDefaultAsync(c => c.Email == username);
            if (customer == null)
            {
                return Unauthorized(); 
            }
            bookPickup.CustomerId = customer.CustomerId; 

            _context.Add(bookPickup);
            await _context.SaveChangesAsync();
            return RedirectToAction("Create", "BookPickupDetails", new { bookPickupId = bookPickup.BookPickupId });
        }

        // GET: BookPickups/Edit/5
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookPickup = await _context.BookPickup.FindAsync(id);
            if (bookPickup == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "CustomerId", bookPickup.CustomerId);
            return View(bookPickup);
        }

        // POST: BookPickups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookPickupId,EventName,EventAddress,BookDate,BookTime,BookStatus,TotalAmount,PaymentStatus,CustomerId")] BookPickup bookPickup)
        {
            if (id != bookPickup.BookPickupId)
            {
                return NotFound();
            }

            //if (ModelState.IsValid)
            //{
                try
                {
                    _context.Update(bookPickup);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookPickupExists(bookPickup.BookPickupId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                //}
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "CustomerId", bookPickup.CustomerId);
            return View(bookPickup);
        }

        // GET: BookPickups/Delete/5
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookPickup = await _context.BookPickup
                .Include(b => b.Customer)
                .FirstOrDefaultAsync(m => m.BookPickupId == id);
            if (bookPickup == null)
            {
                return NotFound();
            }

            return View(bookPickup);
        }

        // POST: BookPickups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bookPickup = await _context.BookPickup.FindAsync(id);
            if (bookPickup != null)
            {
                _context.BookPickup.Remove(bookPickup);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookPickupExists(int id)
        {
            return _context.BookPickup.Any(e => e.BookPickupId == id);
        }
    }
}

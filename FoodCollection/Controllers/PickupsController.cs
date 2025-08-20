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
    public class PickupsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PickupsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Pickups
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Pickup.Include(p => p.BookPickup).Include(p => p.Staff);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Pickups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pickup = await _context.Pickup
                .Include(p => p.BookPickup)
                .Include(p => p.Staff)
                .FirstOrDefaultAsync(m => m.PickupId == id);
            if (pickup == null)
            {
                return NotFound();
            }

            return View(pickup);
        }

        // GET: Pickups/Create
        public IActionResult Create()
        {
            ViewData["BookPickupId"] = new SelectList(_context.BookPickup, "BookPickupId", "BookPickupId");
            ViewData["StaffId"] = new SelectList(_context.Staff, "StaffId", "StaffId");
            return View();
        }

        // POST: Pickups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PickupId,Date,PickupStatus,BookPickupId,StaffId")] Pickup pickup)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pickup);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookPickupId"] = new SelectList(_context.BookPickup, "BookPickupId", "BookPickupId", pickup.BookPickupId);
            ViewData["StaffId"] = new SelectList(_context.Staff, "StaffId", "StaffId", pickup.StaffId);
            return View(pickup);
        }

        // GET: Pickups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pickup = await _context.Pickup.FindAsync(id);
            if (pickup == null)
            {
                return NotFound();
            }
            ViewData["BookPickupId"] = new SelectList(_context.BookPickup, "BookPickupId", "BookPickupId", pickup.BookPickupId);
            ViewData["StaffId"] = new SelectList(_context.Staff, "StaffId", "StaffId", pickup.StaffId);
            return View(pickup);
        }

        // POST: Pickups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PickupId,Date,PickupStatus,BookPickupId,StaffId")] Pickup pickup)
        {
            if (id != pickup.PickupId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pickup);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PickupExists(pickup.PickupId))
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
            ViewData["BookPickupId"] = new SelectList(_context.BookPickup, "BookPickupId", "BookPickupId", pickup.BookPickupId);
            ViewData["StaffId"] = new SelectList(_context.Staff, "StaffId", "StaffId", pickup.StaffId);
            return View(pickup);
        }

        // GET: Pickups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pickup = await _context.Pickup
                .Include(p => p.BookPickup)
                .Include(p => p.Staff)
                .FirstOrDefaultAsync(m => m.PickupId == id);
            if (pickup == null)
            {
                return NotFound();
            }

            return View(pickup);
        }

        // POST: Pickups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pickup = await _context.Pickup.FindAsync(id);
            if (pickup != null)
            {
                _context.Pickup.Remove(pickup);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PickupExists(int id)
        {
            return _context.Pickup.Any(e => e.PickupId == id);
        }
    }
}

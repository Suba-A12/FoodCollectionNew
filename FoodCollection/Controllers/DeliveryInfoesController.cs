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
    public class DeliveryInfoesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DeliveryInfoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DeliveryInfoes
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.DeliveryInfo.Include(d => d.Organization).Include(d => d.Pickup);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: DeliveryInfoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deliveryInfo = await _context.DeliveryInfo
                .Include(d => d.Organization)
                .Include(d => d.Pickup)
                .FirstOrDefaultAsync(m => m.DelilveryInfoId == id);
            if (deliveryInfo == null)
            {
                return NotFound();
            }

            return View(deliveryInfo);
        }

        // GET: DeliveryInfoes/Create
        public IActionResult Create()
        {
            ViewData["OrganizationId"] = new SelectList(_context.Organization, "OrganizationId", "OrganizationId");
            ViewData["PickupId"] = new SelectList(_context.Pickup, "PickupId", "PickupId");
            return View();
        }

        // POST: DeliveryInfoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DelilveryInfoId,Date,DeliveryStatus,PickupId,OrganizationId")] DeliveryInfo deliveryInfo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(deliveryInfo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrganizationId"] = new SelectList(_context.Organization, "OrganizationId", "OrganizationId", deliveryInfo.OrganizationId);
            ViewData["PickupId"] = new SelectList(_context.Pickup, "PickupId", "PickupId", deliveryInfo.PickupId);
            return View(deliveryInfo);
        }

        // GET: DeliveryInfoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deliveryInfo = await _context.DeliveryInfo.FindAsync(id);
            if (deliveryInfo == null)
            {
                return NotFound();
            }
            ViewData["OrganizationId"] = new SelectList(_context.Organization, "OrganizationId", "OrganizationId", deliveryInfo.OrganizationId);
            ViewData["PickupId"] = new SelectList(_context.Pickup, "PickupId", "PickupId", deliveryInfo.PickupId);
            return View(deliveryInfo);
        }

        // POST: DeliveryInfoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DelilveryInfoId,Date,DeliveryStatus,PickupId,OrganizationId")] DeliveryInfo deliveryInfo)
        {
            if (id != deliveryInfo.DelilveryInfoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(deliveryInfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeliveryInfoExists(deliveryInfo.DelilveryInfoId))
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
            ViewData["OrganizationId"] = new SelectList(_context.Organization, "OrganizationId", "OrganizationId", deliveryInfo.OrganizationId);
            ViewData["PickupId"] = new SelectList(_context.Pickup, "PickupId", "PickupId", deliveryInfo.PickupId);
            return View(deliveryInfo);
        }

        // GET: DeliveryInfoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deliveryInfo = await _context.DeliveryInfo
                .Include(d => d.Organization)
                .Include(d => d.Pickup)
                .FirstOrDefaultAsync(m => m.DelilveryInfoId == id);
            if (deliveryInfo == null)
            {
                return NotFound();
            }

            return View(deliveryInfo);
        }

        // POST: DeliveryInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var deliveryInfo = await _context.DeliveryInfo.FindAsync(id);
            if (deliveryInfo != null)
            {
                _context.DeliveryInfo.Remove(deliveryInfo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DeliveryInfoExists(int id)
        {
            return _context.DeliveryInfo.Any(e => e.DelilveryInfoId == id);
        }
    }
}

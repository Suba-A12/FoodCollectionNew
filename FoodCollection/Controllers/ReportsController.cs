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
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Reports
        public async Task<IActionResult> Index()
        {
            //var applicationDbContext = _context.Report.Include(r => r.Staff);
            //return View(await applicationDbContext.ToListAsync());
            var reportVM = new ReportVM
            {
                MonthlyRevenue = GetMonthlyRevenue(),
                PopularFood = GetPopularFood(),
                OrganizationStatus = GetOrganizationStatus()
            };
            reportVM.TotalRevenue = reportVM.MonthlyRevenue.Sum(m => m.Amount);
            return View(reportVM);
        }
        private List<MonthlyRevenueVM> GetMonthlyRevenue()
        {
            // Fetch data from DB first
            var payments = _context.Payment
                .AsEnumerable() 
                .GroupBy(p => new { p.Date.Year, p.Date.Month })
                .Select(g => new MonthlyRevenueVM
                {
                    Month = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MMM yyyy"),
                    Amount = (decimal)g.Sum(p => p.Amount) 
                })
                .OrderBy(r => r.Month)
                .ToList();

            return payments;
        }
        private List<PopularFoodVM> GetPopularFood()
        {
            return _context.BookPickupDetail
                .Join(
                    _context.FoodItem,
                    detail => detail.FoodItemId,
                    food => food.FoodItemId,
                    (detail, food) => new { detail, food }
                )
                .GroupBy(x => new { x.food.FoodItemId, x.food.FoodType })
                .Select(g => new PopularFoodVM
                {
                    FoodItemId = g.Key.FoodItemId,
                    FoodType = g.Key.FoodType,
                    CountFoodItem = g.Count()
                })
                .OrderByDescending(f => f.CountFoodItem)
                .ToList();
        }

        private List<OrganizationPickupVM> GetOrganizationStatus()
        {
            return _context.DeliveryInfo   
                .Include(d => d.Organization) 
                .AsEnumerable()               
                .GroupBy(d => d.Organization?.OrganizationName ?? "Unknown")
                .Select(g => new OrganizationPickupVM
                {
                    OrganizationName = g.Key,
                    PickupCount = g.Count()
                })
                .OrderByDescending(x => x.PickupCount)
                .ToList();
        }
        // GET: Reports/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var report = await _context.Report
                .Include(r => r.Staff)
                .FirstOrDefaultAsync(m => m.ReportId == id);
            if (report == null)
            {
                return NotFound();
            }

            return View(report);
        }

        // GET: Reports/Create
        public IActionResult Create()
        {
            ViewData["StaffId"] = new SelectList(_context.Staff, "StaffId", "StaffId");
            return View();
        }

        // POST: Reports/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReportId,ReportTitle,Date,StaffId")] Report report)
        {
            if (ModelState.IsValid)
            {
                _context.Add(report);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StaffId"] = new SelectList(_context.Staff, "StaffId", "StaffId", report.StaffId);
            return View(report);
        }

        // GET: Reports/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var report = await _context.Report.FindAsync(id);
            if (report == null)
            {
                return NotFound();
            }
            ViewData["StaffId"] = new SelectList(_context.Staff, "StaffId", "StaffId", report.StaffId);
            return View(report);
        }

        // POST: Reports/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReportId,ReportTitle,Date,StaffId")] Report report)
        {
            if (id != report.ReportId)
            {
                return NotFound();
            }

            //if (ModelState.IsValid)
            //{
                try
                {
                    _context.Update(report);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReportExists(report.ReportId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            //}
            ViewData["StaffId"] = new SelectList(_context.Staff, "StaffId", "StaffId", report.StaffId);
            return View(report);
        }

        // GET: Reports/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var report = await _context.Report
                .Include(r => r.Staff)
                .FirstOrDefaultAsync(m => m.ReportId == id);
            if (report == null)
            {
                return NotFound();
            }

            return View(report);
        }

        // POST: Reports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var report = await _context.Report.FindAsync(id);
            if (report != null)
            {
                _context.Report.Remove(report);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReportExists(int id)
        {
            return _context.Report.Any(e => e.ReportId == id);
        }
    }
}

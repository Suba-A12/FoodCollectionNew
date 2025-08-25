using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FoodCollection.Data;
using FoodCollection.Models;
using Stripe;
using Microsoft.Extensions.Options;
using Stripe.Checkout;

namespace FoodCollection.Controllers
{
    public class PaymentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly StripeSettings _stripeSettings;
        private readonly IConfiguration _configuration;

        public PaymentsController(ApplicationDbContext context, IOptions<StripeSettings> stripeSettings, IConfiguration configuration)
        {
            _context = context;
            _stripeSettings = stripeSettings.Value;
            _configuration = configuration;
        }

        // GET: Payments
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Payment.Include(p => p.BookPickup);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Payments/Confirmation/{id}
        public IActionResult Confirmation(int bookPickupId)
        {
            ViewBag.Amount = 35.00m;   // fixed price
            ViewBag.BookPickupId = bookPickupId;
            return View();
        }
        
        // POST: Payments/CreateCheckoutSession
        [HttpPost]
        public IActionResult CreateCheckoutSession(int bookPickupId)
        {
            decimal amount = 35.00m; // fixed $35
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = 3500, 
                    Currency = "sgd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = "Food Donation Booking"
                    }
                },
                Quantity = 1
            }
            },
                Mode = "payment",
                SuccessUrl = Url.Action("Success", "Payments", new { bookPickupId }, Request.Scheme),
                CancelUrl = Url.Action("Cancel", "Payments", new { bookPickupId }, Request.Scheme),
        };
            var service = new SessionService();
            var session = service.Create(options);

            return Redirect(session.Url); 
        }

        public async Task<IActionResult> Success(int bookPickupId)
        {
            var booking = await _context.BookPickup
            .FirstOrDefaultAsync(b => b.BookPickupId == bookPickupId);

            if (booking != null)
            {
                booking.PaymentStatus = "Paid";
                booking.BookStatus = "Confirmed";

                var payment = new Payment
                {
                    PaymentMethod = "Stripe",
                    Amount = 35.00, 
                    Date = DateOnly.FromDateTime(DateTime.Now),
                    BookPickupId = bookPickupId
                };

                _context.Payment.Add(payment);
                await _context.SaveChangesAsync();
            }
            ViewBag.BookPickupId = bookPickupId;
            return View(); 
        }
        public async Task<IActionResult> Cancel(int bookPickupId)
        {
            var booking = await _context.BookPickup
            .FirstOrDefaultAsync(b => b.BookPickupId == bookPickupId);

            if (booking != null)
            {
                booking.PaymentStatus = "Unpaid";
                booking.BookStatus = "Pending";
                await _context.SaveChangesAsync();
            }
            ViewBag.BookPickupId = bookPickupId;
            return View(); 
        }

        // GET: Payments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payment
                .Include(p => p.BookPickup)
                .FirstOrDefaultAsync(m => m.PaymentId == id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        // GET: Payments/Create
        public IActionResult Create()
        {
            ViewData["BookPickupId"] = new SelectList(_context.BookPickup, "BookPickupId", "BookPickupId");
            return View();
        }

        // POST: Payments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PaymentId,PaymentMethod,Amount,Date,BookPickupId")] Payment payment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(payment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookPickupId"] = new SelectList(_context.BookPickup, "BookPickupId", "BookPickupId", payment.BookPickupId);
            return View(payment);
        }

        // GET: Payments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payment.FindAsync(id);
            if (payment == null)
            {
                return NotFound();
            }
            ViewData["BookPickupId"] = new SelectList(_context.BookPickup, "BookPickupId", "BookPickupId", payment.BookPickupId);
            return View(payment);
        }

        // POST: Payments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PaymentId,PaymentMethod,Amount,Date,BookPickupId")] Payment payment)
        {
            if (id != payment.PaymentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(payment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaymentExists(payment.PaymentId))
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
            ViewData["BookPickupId"] = new SelectList(_context.BookPickup, "BookPickupId", "BookPickupId", payment.BookPickupId);
            return View(payment);
        }

        // GET: Payments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payment
                .Include(p => p.BookPickup)
                .FirstOrDefaultAsync(m => m.PaymentId == id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        // POST: Payments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var payment = await _context.Payment.FindAsync(id);
            if (payment != null)
            {
                _context.Payment.Remove(payment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PaymentExists(int id)
        {
            return _context.Payment.Any(e => e.PaymentId == id);
        }
    }
}

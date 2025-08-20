using FoodCollection.Models;
using Microsoft.AspNetCore.Mvc;

namespace FoodCollection.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            //var report = GenerateMockMonthlyPayments();
            //return View(report);
            var dvm = new DashboardVM();
            dvm.foodItems = PopularFood();
            dvm.payments = GenerateMockMonthlyPayments();
            ViewBag.TotalSales = dvm.payments.Sum(p => p.Amount).ToString("C2");
            return View(dvm);
        }
        public List<Payment> GenerateMockMonthlyPayments()
        {
            var monthlyPayments = new List<Payment>();
            monthlyPayments.Add(new Payment
            {
                PaymentId = 1,
                PaymentMethod = "Cash",
                Amount = 1000,
                Date = DateOnly.Parse("2025-01-01"),
                BookPickupId = 1
            });

            monthlyPayments.Add(new Payment
            {
                PaymentId = 2,
                PaymentMethod = "Cash",
                Amount = 3500,
                Date = DateOnly.Parse("2025-02-02"),
                BookPickupId = 2
            });

            monthlyPayments.Add(new Payment
            {
                PaymentId = 3,
                PaymentMethod = "Cash",
                Amount = 1500,
                Date = DateOnly.Parse("2025-03-03"),
                BookPickupId = 3
            });

            monthlyPayments.Add(new Payment
            {
                PaymentId = 4,
                PaymentMethod = "Cash",
                Amount = 2100,
                Date = DateOnly.Parse("2025-04-04"),
                BookPickupId = 4
            });

            monthlyPayments.Add(new Payment
            {
                PaymentId = 5,
                PaymentMethod = "Cash",
                Amount = 2900,
                Date = DateOnly.Parse("2025-05-05"),
                BookPickupId = 5
            });

            monthlyPayments.Add(new Payment
            {
                PaymentId = 6,
                PaymentMethod = "Cash",
                Amount = 3000,
                Date = DateOnly.Parse("2025-06-06"),
                BookPickupId = 6
            });

            monthlyPayments.Add(new Payment
            {
                PaymentId = 7,
                PaymentMethod = "Cash",
                Amount = 4300,
                Date = DateOnly.Parse("2025-07-07"),
                BookPickupId = 7
            });

            return monthlyPayments.OrderBy(mp => mp.Date).ToList();
        }

        public IActionResult FoodReport()
        {
            var Food = PopularFood();
            return View(Food);
        }
        public List<FoodItemVM>PopularFood()
        {
            var food = new List<BookPickupDetail>();

            food.Add(new BookPickupDetail
            {
                BookPickupDetailId = 1,
                BookPickupId = 1,
                FoodItemId = 3,
                QuantityLeft = 20,
                ExpiryDate = DateOnly.Parse("2025-07-22")
            });

            food.Add(new BookPickupDetail
            {
                BookPickupDetailId = 2,
                BookPickupId = 2,
                FoodItemId = 3,
                QuantityLeft = 10,
                ExpiryDate = DateOnly.Parse("2025-06-02")
            });

            food.Add(new BookPickupDetail
            {
                BookPickupDetailId = 3,
                BookPickupId =3,
                FoodItemId = 3,
                QuantityLeft = 15,
                ExpiryDate = DateOnly.Parse("2025-07-12")
            });

            food.Add(new BookPickupDetail
            {
                BookPickupDetailId = 4,
                BookPickupId = 4,
                FoodItemId = 3,
                QuantityLeft = 6,
                ExpiryDate = DateOnly.Parse("2025-05-18")
            });

            food.Add(new BookPickupDetail
            {
                BookPickupDetailId = 5,
                BookPickupId = 5,
                FoodItemId = 2,
                QuantityLeft =  18,
                ExpiryDate = DateOnly.Parse("2025-04-22")
            });

            food.Add(new BookPickupDetail
            {
                BookPickupDetailId = 6,
                BookPickupId = 6,
                FoodItemId = 2,
                QuantityLeft = 8,
                ExpiryDate = DateOnly.Parse("2025-07-23")
            });

            food.Add(new BookPickupDetail
            {
                BookPickupDetailId = 7,
                BookPickupId = 7,
                FoodItemId = 1,
                QuantityLeft = 3,
                ExpiryDate = DateOnly.Parse("2025-07-28")
            });

            var counts = food.GroupBy(bp => bp.FoodItemId)
                .Select(g => new
                {
                    FoodItemId = g.Key,
                    Count = g.Count()
                });

            var lstFoodItemVM = new List<FoodItemVM>();
            foreach (var item in counts)
            {
                lstFoodItemVM.Add(new FoodItemVM
                {
                    FoodItemId = item.FoodItemId,
                    CountFoodItem = item.Count
                });
            }

            return lstFoodItemVM.OrderByDescending(p => p.CountFoodItem).ToList();

        }
    }
}

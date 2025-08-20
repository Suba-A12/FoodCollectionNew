using Microsoft.AspNetCore.Mvc;
using FoodCollection.Models;
using FoodCollection.Services;

namespace FoodCollection.Controllers
{
    public class EmailController : Controller
    {
        //object instantiation of the emailservice class from service folder
        private readonly EmailService _emailService;

        //Constructor to initialize the _emailservice onject
        //if no initialization it will yieldan error when running
        // the error onject instance is set to null
        public EmailController(EmailService emailService)
        {
            _emailService = emailService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(EmailModel model)
        {
            if (ModelState.IsValid)
            {
                await _emailService.SendEmailAsync(model);
                ViewBag.Message = "Email sent successfully";
            }
            return View();
        }
    }
}

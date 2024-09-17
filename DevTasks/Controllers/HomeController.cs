using DevTasks.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DevTasks.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Error()
        {
            if (HttpContext.Request.Headers.Accept.ToString().Contains("application/json"))
            {
                // Return JSON response for API requests
                return Json(new { error = "An unexpected error occurred." });
            }

            // In development environment, show detailed errors
            if (HttpContext.RequestServices.GetService<IWebHostEnvironment>().IsDevelopment())
            {
                var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
                return View("DevelopmentError", new ErrorViewModel
                {
                    ErrorMessage = exceptionHandlerPathFeature?.Error.Message,
                    StackTrace = exceptionHandlerPathFeature?.Error.StackTrace
                });
            }

            // In production environment, show user-friendly errors
            return View("Error");
        }
    }
}

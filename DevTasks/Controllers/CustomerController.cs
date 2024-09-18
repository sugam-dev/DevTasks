using Microsoft.AspNetCore.Mvc;

namespace DevTasks.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

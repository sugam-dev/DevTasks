using Microsoft.AspNetCore.Mvc;

namespace DevTasks.Controllers
{
    public class DistrictController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

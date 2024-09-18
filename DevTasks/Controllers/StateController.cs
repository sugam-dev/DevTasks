using Microsoft.AspNetCore.Mvc;

namespace DevTasks.Controllers
{
    public class StateController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

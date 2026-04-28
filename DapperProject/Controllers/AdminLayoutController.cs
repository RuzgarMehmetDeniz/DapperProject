using Microsoft.AspNetCore.Mvc;

namespace DapperProject.Controllers
{
    public class AdminLayoutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

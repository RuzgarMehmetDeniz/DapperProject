using Microsoft.AspNetCore.Mvc;

namespace DapperProject.ViewComponents.DasboardViewComponentPartial
{
    public class _DashboarHeaderComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}

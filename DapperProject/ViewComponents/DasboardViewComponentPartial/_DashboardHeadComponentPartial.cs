using Microsoft.AspNetCore.Mvc;

namespace DapperProject.ViewComponents.DasboardViewComponentPartial
{
    public class _DashboardHeadComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace DapperProject.ViewComponents.AdminLayoutComponentPartial
{
    public class _AdminLayoutSidebarComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}

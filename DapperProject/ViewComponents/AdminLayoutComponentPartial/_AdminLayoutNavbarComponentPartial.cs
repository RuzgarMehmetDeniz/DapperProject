using Microsoft.AspNetCore.Mvc;

namespace DapperProject.ViewComponents.AdminLayoutComponentPartial
{
    public class _AdminLayoutNavbarComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}

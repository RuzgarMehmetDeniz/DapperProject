using Microsoft.AspNetCore.Mvc;

namespace DapperProject.ViewComponents.AdminLayoutComponentPartial
{
    public class _AdminLayoutHeaderComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}

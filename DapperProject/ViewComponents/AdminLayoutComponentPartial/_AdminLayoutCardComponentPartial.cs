using Microsoft.AspNetCore.Mvc;

namespace DapperProject.ViewComponents.AdminLayoutComponentPartial
{
    public class _AdminLayoutCardComponentPartial:ViewComponent
    {
                public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}

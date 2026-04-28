using Microsoft.AspNetCore.Mvc;

namespace DapperProject.ViewComponents.AdminLayoutComponentPartial
{
    public class _AdminLayoutScriptComponentPartial: ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using DapperProject.Repositories.DashboardRepository;

namespace DapperProject.ViewComponents.DasboardViewComponentPartial
{
    public class _DashboardChart1ComponentPartial : ViewComponent
    {
        private readonly IDashboardService _dashboardService;

        public _DashboardChart1ComponentPartial(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public IViewComponentResult Invoke()
        {
            var model = _dashboardService.GetKpiData();
            return View(model);
        }
    }
}
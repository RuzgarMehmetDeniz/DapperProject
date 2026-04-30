using DapperProject.Models;
using DapperProject.Models.Dashboard;

namespace DapperProject.Repositories.DashboardRepository
{
    public interface IDashboardService
    {
        DashboardKpiViewModel GetKpiData();
    }
}
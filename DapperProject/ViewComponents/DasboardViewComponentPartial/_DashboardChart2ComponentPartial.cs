using Dapper;
using DapperProject.Models;
using DapperProject.Models._DashboardChart2;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace DapperProject.ViewComponents.DasboardViewComponentPartial
{
    public class _DashboardChart2ComponentPartial : ViewComponent
    {
        private readonly string _connectionString;

        public _DashboardChart2ComponentPartial(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("connectionkey");
        }

        public IViewComponentResult Invoke()
        {
            var monthlyRevenues = new List<MonthlyRevenue>
    {
        new MonthlyRevenue { Ay = 1,  ToplamCiro = 42000 },
        new MonthlyRevenue { Ay = 2,  ToplamCiro = 58000 },
        new MonthlyRevenue { Ay = 3,  ToplamCiro = 35000 },
        new MonthlyRevenue { Ay = 4,  ToplamCiro = 71000 },
        new MonthlyRevenue { Ay = 5,  ToplamCiro = 89000 },
        new MonthlyRevenue { Ay = 6,  ToplamCiro = 64000 },
        new MonthlyRevenue { Ay = 7,  ToplamCiro = 95000 },
        new MonthlyRevenue { Ay = 8,  ToplamCiro = 110000 },
        new MonthlyRevenue { Ay = 9,  ToplamCiro = 78000 },
        new MonthlyRevenue { Ay = 10, ToplamCiro = 130000 },
        new MonthlyRevenue { Ay = 11, ToplamCiro = 115000 },
        new MonthlyRevenue { Ay = 12, ToplamCiro = 145000 },
    };

            var statusCounts = new List<OrderStatusCount>
    {
        new OrderStatusCount { Status = "Shipped",   Adet = 225000 },
        new OrderStatusCount { Status = "Pending",   Adet = 200500 },
        new OrderStatusCount { Status = "Cancelled", Adet = 74500  },
    };

            var model = new DashboardChartViewModel
            {
                MonthlyRevenues = monthlyRevenues,
                StatusCounts = statusCounts,
                TotalOrders = statusCounts.Sum(s => s.Adet)
            };

            return View(model);
        }
    }
}
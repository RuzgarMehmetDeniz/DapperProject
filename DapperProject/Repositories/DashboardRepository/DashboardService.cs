using Dapper;
using DapperProject.Models;
using DapperProject.Models.Dashboard;
using Microsoft.Data.SqlClient;

namespace DapperProject.Repositories.DashboardRepository
{
    public class DashboardService : IDashboardService
    {
        private readonly string _connectionString;

        public DashboardService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("connectionkey");
        }

        public DashboardKpiViewModel GetKpiData()
        {
            using var connection = new SqlConnection(_connectionString);

            var sql = @"
    SELECT 
        (SELECT COUNT(*) FROM Orders) AS TotalOrders,
        (SELECT COUNT(*) FROM Categories WHERE Status = 1) AS ActiveCategories,
        -- Bu Ayki Ciro → Aralık 2024
        (SELECT ISNULL(SUM(Price * Quantity), 0) 
         FROM Orders 
         WHERE MONTH(OrderDate) = 12
         AND YEAR(OrderDate) = 2024) AS RevenueThisMonth,
        -- Geçen Ayki Ciro → Kasım 2024
        (SELECT ISNULL(SUM(Price * Quantity), 0) 
         FROM Orders 
         WHERE MONTH(OrderDate) = 11
         AND YEAR(OrderDate) = 2024) AS RevenueLastMonth,
        -- Toplam Müşteri Sayısı
        (SELECT COUNT(*) FROM Customers) AS TotalCustomers,
        -- Bugünkü Siparişler → 2 Aralık 2024
        (SELECT COUNT(*) FROM Orders 
         WHERE CAST(OrderDate AS DATE) = '2024-12-02') AS NewOrdersToday
";

            var result = connection.QuerySingle<DashboardKpiViewModel>(sql);

            if (result.RevenueLastMonth > 0)
                result.MonthlyGrowthPercent =
                    ((result.RevenueThisMonth - result.RevenueLastMonth) / result.RevenueLastMonth) * 100;
            else
                result.MonthlyGrowthPercent = 0;

            return result;
        }
    }
}
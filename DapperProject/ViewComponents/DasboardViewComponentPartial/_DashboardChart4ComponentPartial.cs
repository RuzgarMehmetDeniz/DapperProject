using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace DapperProject.ViewComponents.DasboardViewComponentPartial
{
    public class _DashboardChart4ComponentPartial : ViewComponent
    {
        private readonly IConfiguration _configuration;

        public _DashboardChart4ComponentPartial(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IViewComponentResult Invoke()
        {
            var connectionString = _configuration.GetConnectionString("connectionkey");

            using var connection = new SqlConnection(connectionString);
            connection.Open();

            var ordersQuery = @"
                SELECT TOP 10
                    OrderId, CustomerId, ProductId,
                    Quantity, Price, Status, OrderDate
                FROM Orders
                ORDER BY OrderDate DESC";

            var topStockQuery = @"
                SELECT TOP 5 Name, Stock 
                FROM Product 
                ORDER BY Stock DESC";

            var lowStockQuery = @"
                SELECT TOP 5 Name, Stock 
                FROM Product 
                ORDER BY Stock ASC";

            ViewBag.Orders = connection.Query(ordersQuery).ToList();
            ViewBag.TopStock = connection.Query(topStockQuery).ToList();
            ViewBag.LowStock = connection.Query(lowStockQuery).ToList();

            return View();
        }
    }
}
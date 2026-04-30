using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace DapperProject.ViewComponents.DasboardViewComponentPartial
{
    public class _DashboardChart3ComponentPartial : ViewComponent
    {
        private readonly IConfiguration _configuration;

        public _DashboardChart3ComponentPartial(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IViewComponentResult Invoke()
        {
            var connectionString = _configuration.GetConnectionString("connectionkey");

            using var connection = new SqlConnection(connectionString);
            connection.Open();

            var query = @"
                SELECT TOP 8 
                    c.CategoryName,
                    COUNT(p.ProductID) AS ProductCount
                FROM Product p
                INNER JOIN Categories c ON p.CategoryID = c.CategoryID
                GROUP BY c.CategoryName
                ORDER BY ProductCount DESC";

            var data = connection.Query(query).ToList();

            var categories = data.Select(x => (string)x.CategoryName).ToList();
            var counts = data.Select(x => (int)x.ProductCount).ToList();

            ViewBag.Categories = categories;
            ViewBag.Counts = counts;
            ViewBag.MaxCount = counts.Any() ? counts.Max() : 1;

            return View();
        }
    }
}
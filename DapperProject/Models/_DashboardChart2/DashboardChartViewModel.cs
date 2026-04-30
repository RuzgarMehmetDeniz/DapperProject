namespace DapperProject.Models._DashboardChart2
{
    public class DashboardChartViewModel
    {
        public List<MonthlyRevenue> MonthlyRevenues { get; set; } = new();
        public List<OrderStatusCount> StatusCounts { get; set; } = new();
        public int TotalOrders { get; set; }
    }

    public class MonthlyRevenue
    {
        public int Ay { get; set; }
        public decimal ToplamCiro { get; set; }
    }

    public class OrderStatusCount
    {
        public string Status { get; set; }
        public int Adet { get; set; }
    }
}

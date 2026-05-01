namespace DapperProject.Models.Dashboard
{
    public class DashboardKpiViewModel
    {
        public int TotalOrders { get; set; }
        public int ActiveCategories { get; set; }
        public decimal RevenueThisMonth { get; set; }
        public decimal RevenueLastMonth { get; set; }
        public decimal MonthlyGrowthPercent { get; set; }
        public int TotalCustomers { get; set; }
        public int NewOrdersToday { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc;
using DapperProject.Repositories.OrderRepository;

namespace DapperProject.ViewComponents.OrderViewComponentPartial
{
    public class _AdminLayoutOrderComponentPartial : ViewComponent
    {
        private readonly IOrderService _orderService;

        public _AdminLayoutOrderComponentPartial(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.TotalOrders = await _orderService.GetTotalOrderCountAsync();
            ViewBag.TotalRevenue = await _orderService.GetTotalRevenueAsync();
            ViewBag.TopCustomer = await _orderService.GetTopCustomerAsync();
            ViewBag.LastOrder = await _orderService.GetLastOrderAsync();

            return View();
        }
    }
}
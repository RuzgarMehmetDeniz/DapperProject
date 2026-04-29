using DapperProject.Repositories.CustomerRepository;
using Microsoft.AspNetCore.Mvc;

namespace DapperProject.ViewComponents.CustomerViewComponentPartial
{
    public class _AdminLayoutCustomerComponentPartial : ViewComponent
    {
        private readonly ICustomerService _customerService;

        public _AdminLayoutCustomerComponentPartial(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.TotalCustomerCount = await _customerService.GetTotalCustomerCountAsync();

            ViewBag.CityCount = await _customerService.GetCityCountAsync();

            ViewBag.TopCityName = await _customerService.GetTopCityNameAsync();

            ViewBag.LastCustomer = await _customerService.GetLastCustomerAsync();

            return View();
        }
    }
}
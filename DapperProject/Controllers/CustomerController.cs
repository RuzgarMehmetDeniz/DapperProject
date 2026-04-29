using DapperProject.Dtos.CustomerDtos;
using DapperProject.Repositories.CustomerRepository;
using Microsoft.AspNetCore.Mvc;

namespace DapperProject.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<IActionResult> Index(int page = 1, string search = "")
        {
            ViewBag.Title = "Müşteri";
            ViewBag.Title2 = "Müşteriler";

            var allCustomers = await _customerService.GetAllCustomerAsync();

            // Arama Filtresi
            if (!string.IsNullOrEmpty(search))
            {
                allCustomers = allCustomers.Where(x =>
                    x.Name.ToLower().Contains(search.ToLower()) ||
                    x.Surname.ToLower().Contains(search.ToLower()) ||
                    x.City.ToLower().Contains(search.ToLower())
                ).ToList();
            }

            // Sayfalama Ayarları
            int pageSize = 12;
            int totalCount = allCustomers.Count;
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var pagedCustomers = allCustomers
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // ViewBag verileri
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalCount = totalCount;
            ViewBag.Search = search;

            return View(pagedCustomers);
        }

        [HttpGet]
        public IActionResult CreateCustomer()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer(CreateCustomerDto createCustomerDto)
        {
            await _customerService.CreateCustomerAsync(createCustomerDto);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteCustomer(int id)
        {
            await _customerService.DeleteCustomerAsync(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateCustomer(int id)
        {
            var value = await _customerService.GetByIdCustomerAsync(id);
            var updateDto = new UpdateCustomerDto
            {
                CustomerId = value.CustomerId,
                Name = value.Name,
                Surname = value.Surname,
                City = value.City,
                Country = value.Country
            };
            return View(updateDto);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCustomer(UpdateCustomerDto updateCustomerDto)
        {
            await _customerService.UpdateCustomerAsync(updateCustomerDto);
            return RedirectToAction("Index");
        }
    }
}
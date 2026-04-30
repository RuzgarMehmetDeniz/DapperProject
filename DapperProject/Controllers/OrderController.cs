using DapperProject.Dtos.OrderDtos;
using DapperProject.Repositories.CategoryRepository;
using DapperProject.Repositories.CustomerRepository;
using DapperProject.Repositories.OrderRepository;
using DapperProject.Repositories.ProductRepository;
using DapperProject.Repositories.ProductRepository.ProductService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DapperProject.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        private readonly ICustomerService _customerService;
        private readonly ICategoryService _categoryService;

        public OrderController(
            IOrderService orderService,
            IProductService productService,
            ICustomerService customerService,
            ICategoryService categoryService)
        {
            _orderService = orderService;
            _productService = productService;
            _customerService = customerService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index(int page = 1, string search = "")
        {
            ViewBag.Title = "Sipariş";
            ViewBag.Title2 = "Siparişler";

            var allOrders = await _orderService.GetAllOrderAsync();

            if (!string.IsNullOrEmpty(search))
            {
                allOrders = allOrders.Where(x =>
                    x.CustomerId.ToString().Contains(search) ||
                    x.ProductId.ToString().Contains(search) ||
                    x.Status.ToLower().Contains(search.ToLower())
                ).ToList();
            }

            int pageSize = 12;
            int totalCount = allOrders.Count;
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var pagedOrders = allOrders
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalCount = totalCount;
            ViewBag.Search = search;

            return View(pagedOrders);
        }

        public async Task<IActionResult> CreateOrder()
        {
            var products = await _productService.GetAllProductAsync();
            var customers = await _customerService.GetAllCustomerAsync();
            var categories = await _categoryService.GetAllCategoryAsync();

            ViewBag.Products = new SelectList(products, "ProductID", "Name");
            ViewBag.Customers = new SelectList(customers, "CustomerId", "Name");
            ViewBag.Categories = new SelectList(categories, "CategoryId", "CategoryName");

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderDto createOrderDto)
        {
            await _orderService.CreateOrderAsync(createOrderDto);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> UpdateOrder(int id)
        {
            var value = await _orderService.GetByIdOrderAsync(id);
            var products = await _productService.GetAllProductAsync();
            var customers = await _customerService.GetAllCustomerAsync();
            var categories = await _categoryService.GetAllCategoryAsync();

            ViewBag.Products = new SelectList(products, "ProductID", "Name");
            ViewBag.Customers = new SelectList(customers, "CustomerId", "Name");
            ViewBag.Categories = new SelectList(categories, "CategoryId", "CategoryName");

            return View(value);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateOrder(UpdateOrderDto updateOrderDto)
        {
            await _orderService.UpdateOrderAsync(updateOrderDto);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteOrder(int id)
        {
            await _orderService.DeleteOrderAsync(id);
            return RedirectToAction("Index");
        }
    }
}
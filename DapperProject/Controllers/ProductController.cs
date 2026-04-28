using DapperProject.Dtos.ProductDtos;
using DapperProject.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DapperProject.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        // Listeleme
        // ProductController.cs
        public async Task<IActionResult> Index(int page = 1, string search = "")
        {
            var allProducts = await _productService.GetAllProductAsync();

            if (!string.IsNullOrEmpty(search))
                allProducts = allProducts.Where(x => x.Name.ToLower()
                              .Contains(search.ToLower())).ToList();

            int pageSize = 12;
            int totalCount = allProducts.Count;
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var pagedProducts = allProducts
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalCount = totalCount;
            ViewBag.Search = search;

            return View(pagedProducts);
        }
        // Oluşturma GET
        [HttpGet]
        public IActionResult CreateProduct()
        {
            return View();
        }

        // Oluşturma POST
        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductDto createProductDto)
        {
            await _productService.CreateProductAsync(createProductDto);
            return RedirectToAction("Index");
        }

        // Silme
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _productService.DeleteProductAsync(id);
            return RedirectToAction("Index");
        }

        // Güncelleme GET
        [HttpGet]
        public async Task<IActionResult> UpdateProduct(int id)
        {
            var value = await _productService.GetByIdProductAsync(id);
            return View(value);
        }

        // Güncelleme POST
        [HttpPost]
        public async Task<IActionResult> UpdateProduct(UpdateProductDto updateProductDto)
        {
            await _productService.UpdateProductAsync(updateProductDto);
            return RedirectToAction("Index");
        }
    }
}
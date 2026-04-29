using DapperProject.Dtos.ProductDtos;
using DapperProject.Repositories.CategoryRepository;
using DapperProject.Repositories.ProductRepository.ProductService;
using Microsoft.AspNetCore.Mvc;

namespace DapperProject.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ProductController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        // Listeleme
        // ProductController.cs
        public async Task<IActionResult> Index(int page = 1, string search = "")
        {
            ViewBag.Title = "Ürün";
            ViewBag.Title2 = "Ürünleri";
            var categoryCount = await _productService.GetAllProductAsync();
            ViewBag.CategoryCount = categoryCount.Count;

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
        public async Task<IActionResult> CreateProduct()
        {
            var categories = await _categoryService.GetAllCategoryAsync();
            ViewBag.Categories = categories;
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
            var categories = await _categoryService.GetAllCategoryAsync();
            ViewBag.Categories = categories;
            var value = await _productService.GetByIdProductAsync(id);
            var updateDto = new UpdateProductDto
            {
                ProductID = value.ProductID,
                Name = value.Name,
                Stock = value.Stock,
                CategoryID = value.CategoryID,
                Price = value.Price,
                Brand = value.Brand
            };
            return View(updateDto);
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
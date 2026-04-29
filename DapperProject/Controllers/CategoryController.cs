using DapperProject.Dtos.CategoryDtos;
using DapperProject.Repositories.CategoryRepository;
using Microsoft.AspNetCore.Mvc;

namespace DapperProject.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;


        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // Listeleme
        public async Task<IActionResult> Index(int page = 1, string search = "")
        {
            ViewBag.Title = "Kategori";
            ViewBag.Title2 = "Kategoriler";

            var allCategories = await _categoryService.GetAllCategoryAsync();

            if (!string.IsNullOrEmpty(search))
                allCategories = allCategories
                    .Where(x => x.CategoryName.ToLower().Contains(search.ToLower()))
                    .ToList();

            int pageSize = 12;
            int totalCount = allCategories.Count();
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var pagedCategories = allCategories
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalCount = totalCount;
            ViewBag.Search = search;
            ViewBag.CategoryCount = totalCount;

            return View(pagedCategories);
        }

        // Oluşturma GET
        [HttpGet]
        public IActionResult CreateCategory()
        {
            return View();
        }

        // Oluşturma POST
        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryDto createCategoryDto)
        {
            await _categoryService.CreateCategoryAsync(createCategoryDto);
            return RedirectToAction("Index");
        }

        // Silme
        public async Task<IActionResult> DeleteCategory(int id)
        {
            await _categoryService.DeleteCategoryAsync(id);
            return RedirectToAction("Index");
        }

        // Güncelleme GET
        [HttpGet]
        public async Task<IActionResult> UpdateCategory(int id)
        {
            var value = await _categoryService.GetByIdCategoryAsync(id);
            var updateDto = new UpdateCategoryDto
            {
                CategoryId = value.CategoryId,
                CategoryName = value.CategoryName,
                Status = value.Status
            };
            return View(updateDto);
        }

        // Güncelleme POST
        [HttpPost]
        public async Task<IActionResult> UpdateCategory(UpdateCategoryDto updateCategoryDto)
        {
            await _categoryService.UpdateCategoryAsync(updateCategoryDto);
            return RedirectToAction("Index");
        }
    }
}
using DapperProject.Repositories.CategoryRepository;
using Microsoft.AspNetCore.Mvc;

namespace DapperProject.ViewComponents.CategoryViewComponentPartial
{
    public class _AdminLayoutCategoryCardComponentPartial:ViewComponent
    {
        private readonly ICategoryService _categoryService;

        public _AdminLayoutCategoryCardComponentPartial(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                ViewBag.TotalCategory = await _categoryService.GetTotalCategoryAsync();
                ViewBag.ActiveCategory = await _categoryService.GetActiveCategoryAsync();
                ViewBag.PassiveCategory = await _categoryService.GetPassiveCategoryAsync();
                ViewBag.LastCategory = await _categoryService.GetLastCategoryAsync();
            }
            catch (Exception)
            {
                ViewBag.TotalCategory = 0;
                ViewBag.ActiveCategory = 0;
                ViewBag.PassiveCategory = 0;
                ViewBag.LastCategory = null;
            }

            return View();
        }
    }
}

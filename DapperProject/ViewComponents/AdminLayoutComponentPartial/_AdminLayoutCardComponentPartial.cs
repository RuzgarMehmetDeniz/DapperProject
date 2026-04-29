using DapperProject.Repositories;
using DapperProject.Repositories.ProductRepository.ProductService;
using Microsoft.AspNetCore.Mvc;

namespace DapperProject.ViewComponents.AdminLayoutComponentPartial
{
    public class _AdminLayoutCardComponentPartial : ViewComponent
    {
        private readonly IProductService _productService;

        public _AdminLayoutCardComponentPartial(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.TotalStock = await _productService.GetTotalStockAsync();
            ViewBag.MaxStock = await _productService.GetMaxStockProductAsync();
            ViewBag.MinStock = await _productService.GetMinStockProductAsync();
            ViewBag.LastProduct = await _productService.GetLastAddedProductAsync();

            return View();
        }
    }
}
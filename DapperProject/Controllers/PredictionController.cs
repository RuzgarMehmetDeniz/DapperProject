using DapperProject.Repositories.OrderRepository;
using DapperProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace DapperProject.Controllers
{
    public class PredictionController : Controller
    {
        private readonly OrderPredictionService _predictionService;
        private readonly IOrderService _orderService;

        public PredictionController(OrderPredictionService predictionService, IOrderService orderService)
        {
            _predictionService = predictionService;
            _orderService = orderService;
        }

        public async Task<IActionResult> Index()
        {
            var yearlyData = await _orderService.GetYearlyOrderDataAsync();
            var predictions = _predictionService.PredictNextYears(yearlyData, howManyYears: 2);

            ViewBag.YearlyData = yearlyData;   // 2022, 2023, 2024 gerçek veri
            ViewBag.Predictions = predictions;  // 2025, 2026 tahmin

            return View();
        }
    }
}
using Microsoft.ML;
using Microsoft.ML.Data;
using DapperProject.Models.ML;

namespace DapperProject.Services
{
    public class OrderPredictionService
    {
        private readonly MLContext _mlContext;

        public OrderPredictionService()
        {
            _mlContext = new MLContext(seed: 0);
        }

        public List<OrderPrediction> PredictNextYears(List<OrderYearlyData> historicalData, int howManyYears = 2)
        {
            var results = new List<OrderPrediction>();

            float baseYear = (float)historicalData.Min(d => d.Year);
            double baseRevenue = (double)historicalData.Average(d => d.TotalRevenue);
            double baseOrders = (double)historicalData.Average(d => d.TotalOrders);

            // --- Ciro pipeline ---
            var revenueTrainData = _mlContext.Data.LoadFromEnumerable(
                historicalData.Select(d => new OrderYearlyInput
                {
                    YearOffset = (float)(d.Year - baseYear),
                    YearOffsetSquared = MathF.Pow((float)(d.Year - baseYear), 2),
                    Label = (float)((double)d.TotalRevenue / baseRevenue)
                })
            );

            var revenuePipeline = _mlContext.Transforms
                .Concatenate("Features",
                    nameof(OrderYearlyInput.YearOffset),
                    nameof(OrderYearlyInput.YearOffsetSquared))
                .Append(_mlContext.Regression.Trainers.LbfgsPoissonRegression(
                    labelColumnName: "Label"));

            var revenueModel = revenuePipeline.Fit(revenueTrainData);
            var revenuePredEngine = _mlContext.Model
                .CreatePredictionEngine<OrderYearlyInput, RegressionPrediction>(revenueModel);

            // --- Sipariş adedi pipeline ---
            var ordersTrainData = _mlContext.Data.LoadFromEnumerable(
                historicalData.Select(d => new OrderYearlyInput
                {
                    YearOffset = (float)(d.Year - baseYear),
                    YearOffsetSquared = MathF.Pow((float)(d.Year - baseYear), 2),
                    Label = (float)((double)d.TotalOrders / baseOrders)
                })
            );

            var ordersPipeline = _mlContext.Transforms
                .Concatenate("Features",
                    nameof(OrderYearlyInput.YearOffset),
                    nameof(OrderYearlyInput.YearOffsetSquared))
                .Append(_mlContext.Regression.Trainers.LbfgsPoissonRegression(
                    labelColumnName: "Label"));

            var ordersModel = ordersPipeline.Fit(ordersTrainData);
            var ordersPredEngine = _mlContext.Model
                .CreatePredictionEngine<OrderYearlyInput, RegressionPrediction>(ordersModel);

            float lastYear = (float)historicalData.Max(d => d.Year);

            for (int i = 1; i <= howManyYears; i++)
            {
                float offset = (lastYear + i) - baseYear;

                var revPred = revenuePredEngine.Predict(new OrderYearlyInput
                {
                    YearOffset = offset,
                    YearOffsetSquared = MathF.Pow(offset, 2)
                });

                var ordersPred = ordersPredEngine.Predict(new OrderYearlyInput
                {
                    YearOffset = offset,
                    YearOffsetSquared = MathF.Pow(offset, 2)
                });

                results.Add(new OrderPrediction
                {
                    Year = (int)(lastYear + i),
                    PredictedRevenue = MathF.Max(0, revPred.Score * (float)baseRevenue),
                    PredictedOrders = MathF.Max(0, ordersPred.Score * (float)baseOrders)
                });
            }

            return results;
        }
    }

    public class OrderYearlyInput
    {
        public float YearOffset { get; set; }
        public float YearOffsetSquared { get; set; }
        public float Label { get; set; }
    }

    public class RegressionPrediction
    {
        [ColumnName("Score")]
        public float Score { get; set; }
    }
}
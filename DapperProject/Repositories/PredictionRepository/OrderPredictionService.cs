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

            // Ciro pipeline
            var revenueTrainData = _mlContext.Data.LoadFromEnumerable(
                historicalData.Select(d => new OrderYearlyInput
                {
                    Year = (float)d.Year,
                    Label = (float)d.TotalRevenue
                })
            );

            var revenuePipeline = _mlContext.Transforms
                .Concatenate("Features", nameof(OrderYearlyInput.Year))
                .Append(_mlContext.Regression.Trainers.Sdca(
                    labelColumnName: "Label",
                    maximumNumberOfIterations: 100));

            var revenueModel = revenuePipeline.Fit(revenueTrainData);
            var revenuePredEngine = _mlContext.Model
                .CreatePredictionEngine<OrderYearlyInput, RegressionPrediction>(revenueModel);

            // Sipariş adedi pipeline
            var ordersTrainData = _mlContext.Data.LoadFromEnumerable(
                historicalData.Select(d => new OrderYearlyInput
                {
                    Year = (float)d.Year,
                    Label = (float)d.TotalOrders
                })
            );

            var ordersPipeline = _mlContext.Transforms
                .Concatenate("Features", nameof(OrderYearlyInput.Year))
                .Append(_mlContext.Regression.Trainers.Sdca(
                    labelColumnName: "Label",
                    maximumNumberOfIterations: 100));

            var ordersModel = ordersPipeline.Fit(ordersTrainData);
            var ordersPredEngine = _mlContext.Model
                .CreatePredictionEngine<OrderYearlyInput, RegressionPrediction>(ordersModel);

            float lastYear = (float)historicalData.Max(d => d.Year);

            for (int i = 1; i <= howManyYears; i++)
            {
                float targetYear = lastYear + i;

                var revPred = revenuePredEngine.Predict(new OrderYearlyInput { Year = targetYear });
                var ordersPred = ordersPredEngine.Predict(new OrderYearlyInput { Year = targetYear });

                results.Add(new OrderPrediction
                {
                    Year = (int)targetYear,
                    PredictedRevenue = MathF.Max(0, revPred.Score),
                    PredictedOrders = MathF.Max(0, ordersPred.Score)
                });
            }

            return results;
        }
    }

    public class OrderYearlyInput
    {
        public float Year { get; set; }
        public float Label { get; set; }
    }

    public class RegressionPrediction
    {
        [ColumnName("Score")]
        public float Score { get; set; }
    }
}
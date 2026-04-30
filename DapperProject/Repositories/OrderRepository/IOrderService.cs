using DapperProject.Dtos.OrderDtos;
using DapperProject.Models.ML;

namespace DapperProject.Repositories.OrderRepository
{
    public interface IOrderService
    {
        Task<List<ResultOrderDto>> GetAllOrderAsync();
        Task<GetByIdOrderDto> GetByIdOrderAsync(int id);
        Task CreateOrderAsync(CreateOrderDto createOrderDto);
        Task UpdateOrderAsync(UpdateOrderDto updateOrderDto);
        Task DeleteOrderAsync(int id);
        Task<int> GetTotalOrderCountAsync();
        Task<decimal> GetTotalRevenueAsync();
        Task<TopCustomerDto> GetTopCustomerAsync();
        Task<LastOrderDto> GetLastOrderAsync();
        Task<List<OrderYearlyData>> GetYearlyOrderDataAsync(); // sadece bir kez!
    }
}
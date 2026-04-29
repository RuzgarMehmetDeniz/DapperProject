using DapperProject.Dtos.CustomerDtos;

namespace DapperProject.Repositories.CustomerRepository
{
    public interface ICustomerService
    {
        Task<List<ResultCustomerDto>> GetAllCustomerAsync();
        Task CreateCustomerAsync(CreateCustomerDto createCustomerDto);
        Task UpdateCustomerAsync(UpdateCustomerDto updateCustomerDto);
        Task DeleteCustomerAsync(int id);
        Task<GetByIdCustomerDto> GetByIdCustomerAsync(int id);

        Task<int> GetTotalCustomerCountAsync();          
        Task<int> GetCityCountAsync();             
        Task<ResultCustomerDto> GetLastCustomerAsync(); 
        Task<string> GetTopCityNameAsync();
    }
}

using DapperProject.Dtos.CustomerDto;

namespace DapperProject.Repositories
{
    public interface ICustomerService
    {
        Task<List<ResultCustomerDto>> GetAllCustomersAsync();
        Task CreateCustomerAsync(CreateCustomerDto createCustomerDto);
        Task UpdateCustomerAsync(UpdateCustomerDto updateCustomerDto);
        Task DeleteCustomerAsync(int id);
        Task<GetCustomerByIdDto> GetCustomerByIdAsync(int id);
    }
}

using Dapper;
using DapperProject.Context;
using DapperProject.Dtos.CustomerDto;

namespace DapperProject.Repositories
{
    public class CustomerService : ICustomerService
    {
        private readonly DapperContext _context;

        public CustomerService(DapperContext context)
        {
            _context = context;
        }

        public async Task CreateCustomerAsync(CreateCustomerDto createCustomerDto)
        {
            string query = "insert into Customers (CustomerName,CustomerSurname,CustomerCity) values (@customerName,@customerSurname,@customerCity)";
            var parameters = new DynamicParameters();
            parameters.Add("@customerName", createCustomerDto.CustomerName);
            parameters.Add("@customerSurname", createCustomerDto.CustomerSurname);
            parameters.Add("@customerCity", createCustomerDto.CustomerCity);
            var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
        }

        public async Task DeleteCustomerAsync(int id)
        {
            string query = "Delete From Customers Where Customerıd=@p";
            var parameters = new DynamicParameters();
            parameters.Add("@p", id);
            var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
        }

        public async Task<List<ResultCustomerDto>> GetAllCustomersAsync()
        {
            string query = "Select * From Customers";
            var connection = _context.CreateConnection();
            var values = await connection.QueryAsync<ResultCustomerDto>(query);
            return values.ToList();
        }

        public async Task<GetCustomerByIdDto> GetCustomerByIdAsync(int id)
        {
            string query = "Select * From Customers Where CustomerId=@id";
            var parameters = new DynamicParameters();
            parameters.Add("@id", id);
            var connection = _context.CreateConnection();
            var value = await connection.QueryFirstAsync<GetCustomerByIdDto>(query,parameters);
            return value;
        }

        public async Task UpdateCustomerAsync(UpdateCustomerDto updateCustomerDto)
        {
           string query = "Update Customers Set CustomerName=@customerName,CustomerSurname=@customerSurname,CustomerCity=@customerCity Where CustomerId=@id";
            var parameters = new DynamicParameters();
            parameters.Add("@customerName", updateCustomerDto.CustomerName);
            parameters.Add("@customerSurname", updateCustomerDto.CustomerSurname);
            parameters.Add("@customerCity", updateCustomerDto.CustomerCity);
            parameters.Add("@id", updateCustomerDto.CustomerId);
            var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
        }
    }
}

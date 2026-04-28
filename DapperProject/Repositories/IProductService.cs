using DapperProject.Dtos.ProductDtos;

namespace DapperProject.Repositories
{
    public interface IProductService
    {
        Task<List<ResultProductDto>> GetAllProductAsync();
        Task<GetByIdProductDto> GetByIdProductAsync(int id);
        Task CreateProductAsync(CreateProductDto productDto);
        Task UpdateProductAsync(UpdateProductDto productDto);
        Task DeleteProductAsync(int id);
    }
}

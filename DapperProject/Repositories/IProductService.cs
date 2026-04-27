using DapperProject.Dtos.ProductDtos;

namespace DapperProject.Repositories
{
    public interface IProductService
    {
        Task<List<ResultProductDto>> GetAllProductAsync();
        Task<GetProrudctDto> GetProductByIdAsync(int productId);
        Task CreateProductAsync(CreateProductDto createProductDto);
        Task UpdateProductAsync(UpdateProductDto updateProductDto);
        Task DeleteProductAsync(int productId);
    }
}

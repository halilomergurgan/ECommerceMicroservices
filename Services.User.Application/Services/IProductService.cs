using Services.User.Application.DTOs;

namespace Services.User.Application.Services;

public interface IProductService
{
    Task<ProductDto?> GetByIdAsync(Guid id);
    Task<List<ProductDto>> GetAllAsync();
    Task<List<ProductListDto>> GetAllListAsync();
    Task<ProductDto> CreateAsync(CreateProductDto createProductDto);
    Task<ProductDto> UpdateAsync(Guid id, UpdateProductDto updateProductDto);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
    Task<List<ProductDto>> GetByCategoryAsync(string categoryName);
}
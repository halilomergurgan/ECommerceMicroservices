using Services.User.Domain.Entities;

namespace Services.User.Domain.Repositories;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(Guid id);
    Task<List<Product>> GetAllAsync();
    Task<Product> CreateAsync(Product product);
    Task<Product> UpdateAsync(Product product);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
}
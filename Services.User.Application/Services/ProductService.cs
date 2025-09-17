using Services.User.Application.DTOs;
using Services.User.Domain.Entities;
using Services.User.Domain.Repositories;

namespace Services.User.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductDto?> GetByIdAsync(Guid id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        return product == null ? null : MapToDto(product);
    }

    public async Task<List<ProductDto>> GetAllAsync()
    {
        var products = await _productRepository.GetAllAsync();
        return products.Select(MapToDto).ToList();
    }

    public async Task<List<ProductListDto>> GetAllListAsync()
    {
        var products = await _productRepository.GetAllAsync();
        return products.Select(MapToListDto).ToList();
    }

    public async Task<ProductDto> CreateAsync(CreateProductDto createProductDto)
    {
        var product = new Product
        {
            Name = createProductDto.Name,
            Description = createProductDto.Description,
            Price = createProductDto.Price,
            CategoryName = createProductDto.CategoryName,
            Quantity = createProductDto.Quantity,
            CreatedAt = DateTime.UtcNow
        };

        var createdProduct = await _productRepository.CreateAsync(product);
        return MapToDto(createdProduct);
    }

    public async Task<ProductDto> UpdateAsync(Guid id, UpdateProductDto updateProductDto)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
        {
            throw new InvalidOperationException("Ürün bulunamadı.");
        }

        if (updateProductDto.Name != null)
            product.Name = updateProductDto.Name;
            
        if (updateProductDto.Description != null)
            product.Description = updateProductDto.Description;
            
        if (updateProductDto.Price.HasValue)
            product.Price = updateProductDto.Price.Value;
            
        if (updateProductDto.CategoryName != null)
            product.CategoryName = updateProductDto.CategoryName;
            
        if (updateProductDto.Quantity.HasValue)
            product.Quantity = updateProductDto.Quantity.Value;
            
        product.UpdatedAt = DateTime.UtcNow;

        var updatedProduct = await _productRepository.UpdateAsync(product);
        return MapToDto(updatedProduct);
    }

    public async Task DeleteAsync(Guid id)
    {
        if (!await _productRepository.ExistsAsync(id))
        {
            throw new InvalidOperationException("Ürün bulunamadı.");
        }

        await _productRepository.DeleteAsync(id);
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _productRepository.ExistsAsync(id);
    }

    public async Task<List<ProductDto>> GetByCategoryAsync(string categoryName)
    {
        // Tüm ürünleri al ve kategori adına göre filtrele
        var allProducts = await _productRepository.GetAllAsync();
        var filteredProducts = allProducts.Where(p => p.CategoryName.Equals(categoryName, StringComparison.OrdinalIgnoreCase)).ToList();
        return filteredProducts.Select(MapToDto).ToList();
    }

    private static ProductDto MapToDto(Product product)
    {
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            CategoryName = product.CategoryName,
            Quantity = product.Quantity,
            CreatedAt = product.CreatedAt,
            UpdatedAt = product.UpdatedAt
        };
    }

    private static ProductListDto MapToListDto(Product product)
    {
        return new ProductListDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            CategoryName = product.CategoryName,
            Quantity = product.Quantity
        };
    }
}
using System;
using System.ComponentModel.DataAnnotations;

namespace Services.User.Application.DTOs;

public class ProductDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateProductDto
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;
    
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
    public decimal Price { get; set; }
    
    [Required]
    public string CategoryName { get; set; } = string.Empty;
    
    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Quantity cannot be negative")]
    public int Quantity { get; set; }
}

public class UpdateProductDto
{
    [Required]
    public Guid Id { get; set; }
    
    [MaxLength(100)]
    public string? Name { get; set; }
    
    [MaxLength(500)]
    public string? Description { get; set; }
    
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
    public decimal? Price { get; set; }
    
    public string? CategoryName { get; set; }
    
    [Range(0, int.MaxValue, ErrorMessage = "Quantity cannot be negative")]
    public int? Quantity { get; set; }
}

public class ProductListDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public int Quantity { get; set; }
}
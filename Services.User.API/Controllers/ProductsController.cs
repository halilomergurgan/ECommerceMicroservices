using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Services.User.Application.DTOs;
using Services.User.Application.Services;

namespace Services.User.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductDto>>> GetAll()
    {
        var products = await _productService.GetAllAsync();
        return Ok(products);
    }

    [HttpGet("list")]
    public async Task<ActionResult<List<ProductListDto>>> GetAllList()
    {
        var products = await _productService.GetAllListAsync();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetById(Guid id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product == null)
            return NotFound();

        return Ok(product);
    }

    [HttpGet("category/{categoryName}")]
    public async Task<ActionResult<List<ProductDto>>> GetByCategory(string categoryName)
    {
        var products = await _productService.GetByCategoryAsync(categoryName);
        return Ok(products);
    }

    [HttpPost]
    public async Task<ActionResult<ProductDto>> Create(CreateProductDto createProductDto)
    {
        try
        {
            var product = await _productService.CreateAsync(createProductDto);
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ProductDto>> Update(Guid id, UpdateProductDto updateProductDto)
    {
        try
        {
            var product = await _productService.UpdateAsync(id, updateProductDto);
            return Ok(product);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _productService.DeleteAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("{id}/exists")]
    public async Task<ActionResult<bool>> Exists(Guid id)
    {
        var exists = await _productService.ExistsAsync(id);
        return Ok(exists);
    }
}
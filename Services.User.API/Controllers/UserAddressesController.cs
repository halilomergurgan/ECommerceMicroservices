using Microsoft.AspNetCore.Mvc;
using Services.User.Application.DTOs;
using Services.User.Application.Services;
using Microsoft.AspNetCore.Authorization;

namespace Services.User.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserAddressesController : ControllerBase
{
    private readonly IUserAddressService _userAddressService;

    public UserAddressesController(IUserAddressService userAddressService)
    {
        _userAddressService = userAddressService;
    }

    [HttpGet]
    public async Task<ActionResult<List<UserAddressDto>>> GetAll()
    {
        var addresses = await _userAddressService.GetAllAsync();
        return Ok(addresses);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserAddressDto>> GetById(Guid id)
    {
        var address = await _userAddressService.GetByIdAsync(id);
        if (address == null)
            return NotFound();
        return Ok(address);
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<List<UserAddressDto>>> GetByUserId(Guid userId)
    {
        var addresses = await _userAddressService.GetByUserIdAsync(userId);
        return Ok(addresses);
    }

    [HttpPost]
    public async Task<ActionResult<UserAddressDto>> Create(CreateUserAddressDto createUserAddressDto)
    {
        var address = await _userAddressService.CreateAsync(createUserAddressDto);
        return CreatedAtAction(nameof(GetById), new { id = address.Id }, address);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<UserAddressDto>> Update(Guid id, UpdateUserAddressDto updateUserAddressDto)
    {
        var address = await _userAddressService.UpdateAsync(id, updateUserAddressDto);
        return Ok(address);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _userAddressService.DeleteAsync(id);
        return NoContent();
    }
}

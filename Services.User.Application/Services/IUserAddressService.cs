using Services.User.Application.DTOs;

namespace Services.User.Application.Services;

public interface IUserAddressService
{
    Task<UserAddressDto?> GetByIdAsync(Guid id);
    Task<List<UserAddressDto>> GetAllAsync();
    Task<List<UserAddressDto>> GetByUserIdAsync(Guid userId);
    Task<UserAddressDto> CreateAsync(CreateUserAddressDto createUserAddressDto);
    Task<UserAddressDto> UpdateAsync(Guid id, UpdateUserAddressDto updateUserAddressDto);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
}

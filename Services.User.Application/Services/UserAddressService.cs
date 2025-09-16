using Services.User.Application.DTOs;
using Services.User.Application.Services;
using Services.User.Domain.Entities;
using Services.User.Domain.Repositories;

namespace Services.User.Application.Services;

public class UserAddressService : IUserAddressService
{
    private readonly IUserAddressRepository _userAddressRepository;

    public UserAddressService(IUserAddressRepository userAddressRepository)
    {
        _userAddressRepository = userAddressRepository;
    }

    public async Task<UserAddressDto?> GetByIdAsync(Guid id)
    {
        var address = await _userAddressRepository.GetByIdAsync(id);
        return address == null ? null : MapToDto(address);
    }

    public async Task<List<UserAddressDto>> GetAllAsync()
    {
        var addresses = await _userAddressRepository.GetAllAsync();
        return addresses.Select(MapToDto).ToList();
    }

    public async Task<List<UserAddressDto>> GetByUserIdAsync(Guid userId)
    {
        var addresses = await _userAddressRepository.GetByUserIdAsync(userId);
        return addresses.Select(MapToDto).ToList();
    }

    public async Task<UserAddressDto> CreateAsync(CreateUserAddressDto createUserAddressDto)
    {
        var address = new UserAddress
        {
            UserId = createUserAddressDto.UserId,
            Address = createUserAddressDto.Address,
            Street = createUserAddressDto.Street,
            City = createUserAddressDto.City,
            State = createUserAddressDto.State,
            PostalCode = createUserAddressDto.PostalCode,
            Country = createUserAddressDto.Country,
            IsDefault = createUserAddressDto.IsDefault,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = null
        };

        var createdAddress = await _userAddressRepository.CreateAsync(address);
        return MapToDto(createdAddress);
    }

    public async Task<UserAddressDto> UpdateAsync(Guid id, UpdateUserAddressDto updateUserAddressDto)
    {
        var address = await _userAddressRepository.GetByIdAsync(id);
        if (address == null)
            throw new InvalidOperationException("Adres bulunamadı.");

        address.Address = updateUserAddressDto.Address;
        address.Street = updateUserAddressDto.Street;
        address.City = updateUserAddressDto.City;
        address.State = updateUserAddressDto.State;
        address.PostalCode = updateUserAddressDto.PostalCode;
        address.Country = updateUserAddressDto.Country;
        address.IsDefault = updateUserAddressDto.IsDefault;
        address.UpdatedAt = DateTime.UtcNow;

        var updatedAddress = await _userAddressRepository.UpdateAsync(address);
        return MapToDto(updatedAddress);
    }

    public async Task DeleteAsync(Guid id)
    {
        if (!await _userAddressRepository.ExistsAsync(id))
            throw new InvalidOperationException("Adres bulunamadı.");

        await _userAddressRepository.DeleteAsync(id);
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _userAddressRepository.ExistsAsync(id);
    }

    private static UserAddressDto MapToDto(UserAddress address)
    {
        return new UserAddressDto
        {
            Id = address.Id,
            UserId = address.UserId,
            Address = address.Address,
            Street = address.Street,
            City = address.City,
            State = address.State,
            PostalCode = address.PostalCode,
            Country = address.Country,
            IsDefault = address.IsDefault,
            CreatedAt = address.CreatedAt,
            UpdatedAt = address.UpdatedAt
        };
    }
}

using Services.User.Domain.Entities;

namespace Services.User.Domain.Repositories;

public interface IUserAddressRepository
{
    Task<UserAddress?> GetByIdAsync(Guid id);
    Task<List<UserAddress>> GetByUserIdAsync(Guid userId);
    Task<UserAddress> CreateAsync(UserAddress address);
    Task<UserAddress> UpdateAsync(UserAddress address);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);

    Task<UserAddress?> GetDefaultAddressAsync(Guid userId);
    Task SetDefaultAddressAsync(Guid userId, Guid addressId);
    Task<UserAddress?> GetUserAddressByIdAsync(Guid userId, Guid addressId);
    Task<List<UserAddress>> GetAllAsync();
}
using UserEntity = Services.User.Domain.Entities.User;

namespace Services.User.Domain.Repositories;

public interface IUserRepository
{
Task<UserEntity?> GetByIdAsync(Guid id);
    Task<UserEntity?> GetByEmailAsync(string email);
    Task<List<UserEntity>> GetAllAsync();
    Task<UserEntity> CreateAsync(UserEntity user);
    Task<UserEntity> UpdateAsync(UserEntity user);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
    Task<bool> EmailExistsAsync(string email);
Task<UserEntity?> GetByRefreshTokenAsync(string refreshToken);
}

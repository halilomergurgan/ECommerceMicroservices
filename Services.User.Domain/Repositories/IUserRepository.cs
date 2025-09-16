using Services.User.Domain.Entities;

namespace Services.User.Domain.Repositories;

public interface IUserRepository
{
    Task<Services.User.Domain.Entities.User?> GetByIdAsync(Guid id);
    Task<Services.User.Domain.Entities.User?> GetByEmailAsync(string email);
    Task<List<Services.User.Domain.Entities.User>> GetAllAsync();
    Task<Services.User.Domain.Entities.User> CreateAsync(Services.User.Domain.Entities.User user);
    Task<Services.User.Domain.Entities.User> UpdateAsync(Services.User.Domain.Entities.User user);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
    Task<bool> EmailExistsAsync(string email);
}

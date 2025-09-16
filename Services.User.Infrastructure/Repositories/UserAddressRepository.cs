using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Services.User.Infrastructure.Data;
using Services.User.Domain.Entities;
using Services.User.Domain.Repositories;

namespace Services.User.Infrastructure.Repositories
{
    public class UserAddressRepository : IUserAddressRepository
    {
        private readonly UserDbContext _context;

        public UserAddressRepository(UserDbContext context)
        {
            _context = context;
        }

        public async Task<UserAddress?> GetByIdAsync(Guid id)
        {
            return await _context.UserAddresses
                .FirstOrDefaultAsync(ua => ua.Id == id);
        }

        public async Task<List<UserAddress>> GetByUserIdAsync(Guid userId)
        {
            return await _context.UserAddresses
                .Where(ua => ua.UserId == userId)
                .ToListAsync();
        }

        public async Task<UserAddress> CreateAsync(UserAddress address)
        {
            await _context.UserAddresses.AddAsync(address);
            await _context.SaveChangesAsync();
            return address;
        }

        public async Task<UserAddress> UpdateAsync(UserAddress address)
        {
            _context.UserAddresses.Update(address);
            await _context.SaveChangesAsync();
            return address;
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _context.UserAddresses.FindAsync(id);
            if (entity != null)
            {
                _context.UserAddresses.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.UserAddresses.AnyAsync(ua => ua.Id == id);
        }

        public async Task<UserAddress?> GetDefaultAddressAsync(Guid userId)
        {
            return await _context.UserAddresses
                .Where(ua => ua.UserId == userId && ua.IsDefault)
                .FirstOrDefaultAsync();
        }

        public async Task SetDefaultAddressAsync(Guid userId, Guid addressId)
        {
            var addresses = await _context.UserAddresses
                .Where(ua => ua.UserId == userId)
                .ToListAsync();

            foreach (var address in addresses)
            {
                address.IsDefault = address.Id == addressId;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<UserAddress?> GetUserAddressByIdAsync(Guid userId, Guid addressId)
        {
            return await _context.UserAddresses
                .Where(ua => ua.UserId == userId && ua.Id == addressId)
                .FirstOrDefaultAsync();
        }

        public async Task<List<UserAddress>> GetAllAsync()
        {
            // Burada kendi ORM/DbContext'inize göre implementasyon yapmalısınız.
            // Örnek:
            // return await _dbContext.UserAddresses.ToListAsync();
            throw new NotImplementedException();
        }
    }
}
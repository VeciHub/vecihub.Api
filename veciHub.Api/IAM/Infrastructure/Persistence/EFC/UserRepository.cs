using Microsoft.EntityFrameworkCore;
using VeciHub.IAM.Domain.Model;
using VeciHub.IAM.Domain.Repositories;
using VeciHub.Shared.Persistence;

namespace VeciHub.IAM.Infrastructure.Persistence.EFC
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> FindByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
    }
}

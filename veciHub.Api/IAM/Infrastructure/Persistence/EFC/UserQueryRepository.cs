using Microsoft.EntityFrameworkCore;
using VeciHub.IAM.Domain.Model;
using VeciHub.IAM.Domain.Repositories;
using VeciHub.Shared.Persistence;

namespace VeciHub.IAM.Infrastructure.Persistence.EFC
{
    public class UserQueryRepository : IUserQueryRepository
    {
        private readonly ApplicationDbContext _context;

        public UserQueryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> FindByIdAsync(Guid id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<IEnumerable<User>> ListAsync()
        {
            return await _context.Users.ToListAsync();
        }
    }
}

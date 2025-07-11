using VeciHub.IAM.Domain.Model;

namespace VeciHub.IAM.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<User?> FindByEmailAsync(string email);
        Task AddAsync(User user);
    }
}

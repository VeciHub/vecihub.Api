using VeciHub.IAM.Domain.Model;

namespace VeciHub.IAM.Domain.Repositories
{
    public interface IUserQueryRepository
    {
        Task<User?> FindByIdAsync(Guid id);
        Task<IEnumerable<User>> ListAsync();
    }
}

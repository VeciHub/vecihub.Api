using VeciHub.IAM.Domain.Model;
using VeciHub.IAM.Domain.Repositories;

namespace VeciHub.IAM.Application.Internal.QueryServices
{
    public class UserQueryService
    {
        private readonly IUserQueryRepository _userRepository;

        public UserQueryService(IUserQueryRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<User>> ListAsync()
        {
            return await _userRepository.ListAsync();
        }

        public async Task<User?> FindByIdAsync(Guid id)
        {
            return await _userRepository.FindByIdAsync(id);
        }
    }
}

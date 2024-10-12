using Domain.Models.Entities;
using Domain.Models.ViewModels;

namespace Domain.Stores
{
    public interface IUserStore
    {
        public Task<User?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken, bool tracking = true);
        public Task<User?> GetUserByCredentialsAsync(Credentials credentials, CancellationToken cancellationToken);
        public Task<int> AddUserAsync(User user, CancellationToken cancellationToken);
        public Task UpdateUserAsync(CancellationToken cancellationToken);
        public Task DeleteUserAsync(User user, CancellationToken cancellationToken);
    }
}

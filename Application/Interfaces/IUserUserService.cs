using Application.Models.ViewModels;
using Domain.Models.ViewModels;

namespace Application.Interfaces
{
    public interface IUserUserService
    {
        public Task<UserOutputModel> GetUserAsync(Guid userId, CancellationToken cancellationToken);
        public Task EditCredentialsAsync(Guid userId, Credentials credentials, CancellationToken cancellationToken);
        public Task EditNameAsync(Guid userId, string name, CancellationToken cancellationToken);
        public Task DeleteUserAsync(Guid userId, CancellationToken cancellationToken);
    }
}

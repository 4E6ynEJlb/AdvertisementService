using Application.Models.ViewModels;
using Domain.Models.ViewModels;

namespace Application.Interfaces
{
    public interface IAuthService
    {
        public void AuthHost(Credentials credentials);
        public Task<UserOutputModel> AuthUserAsync(Credentials credentials, CancellationToken cancellationToken);
        public Task<Guid> RegisterUserAsync(RegisterUserModel registerUserModel, CancellationToken cancellationToken);
    }
}

using Application.Interfaces;
using Application.Models.ApplicationModels;
using Application.Models.ViewModels;
using Domain.Models.ViewModels;
using Microsoft.Extensions.Options;
using Application.Models.Exceptions;
using Domain.Models.Entities;
using Domain.Stores;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserStore _userStore;
        private readonly HostCredentials _hostCredentials;
        private readonly IMemoryCache _cache;
        private readonly int _cacheSlidingExpirationMinutes;
        public AuthService(IUserStore userRepository, IOptions<HostCredentials> hostCredentialsOptions, 
            IMemoryCache cache, IOptions<ApplicationConfiguration> configuration)
        {
            _userStore = userRepository;
            _hostCredentials = hostCredentialsOptions.Value;
            _cache = cache;
            _cacheSlidingExpirationMinutes = 
                configuration.Value.CacheSlidingExpirationMinutes;
        }

        public void AuthHost(Credentials credentials)
        {
            if (credentials.Login != _hostCredentials.HostLogin ||
                credentials.Password != _hostCredentials.HostPassword)
                throw new IncorrectCredentialsException();
        }

        public async Task<UserOutputModel> AuthUserAsync(Credentials credentials, CancellationToken cancellationToken)
        {
            User? user = await _userStore.GetUserByCredentialsAsync(credentials, cancellationToken);
            if (user == null)
                throw new IncorrectCredentialsException();
            return new UserOutputModel()
            {
                Id = user.Id,
                IsAdmin = user.IsAdmin,
                Name = user.Name
            };
        }

        public async Task<Guid> RegisterUserAsync(RegisterUserModel registerUserModel, CancellationToken cancellationToken)
        {
            User user = new User()
            {
                Id = Guid.NewGuid(),
                IsAdmin = false,
                Login = registerUserModel.Login,
                Name = registerUserModel.Name,
                Password = registerUserModel.Password
            };
            if (await _userStore.AddUserAsync(user, cancellationToken) > 0)
                _cache.Set(user.Id, user, new MemoryCacheEntryOptions()
                {
                    SlidingExpiration = TimeSpan.FromMinutes(_cacheSlidingExpirationMinutes)
                });
            return user.Id;
        }
    }
}

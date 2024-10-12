using Application.Interfaces;
using Application.Models.Exceptions;
using Application.Models.ViewModels;
using Domain.Models.Entities;
using Domain.Models.ViewModels;
using Domain.Stores;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Services
{
    public class UsersService : IUserHostService, IUserUserService
    {
        private readonly IUserStore _userStore;
        private readonly IMemoryCache _cache;
        public UsersService(IUserStore userStore, IMemoryCache cache)
        {
            _userStore = userStore;
            _cache = cache;
        }
        public async Task<UserOutputModel> GetUserAsync(Guid userId, CancellationToken cancellationToken)
        {
            if (!_cache.TryGetValue(userId, out User? user))
                user = await _userStore.GetUserByIdAsync(userId, cancellationToken, false);
            if (user == null)
                throw new IncorrectIdException();
            return new UserOutputModel()
            {
                Id = user.Id,
                IsAdmin = user.IsAdmin,
                Name = user.Name
            };
        }

        public async Task AssignAsAdminAsync(Guid userId, CancellationToken cancellationToken)
        {
            User? user = await _userStore.GetUserByIdAsync(userId, cancellationToken);
            if (user == null)
                throw new IncorrectIdException();
            user.IsAdmin = true;
            await _userStore.UpdateUserAsync(cancellationToken);
            _cache.Remove(userId);
        }

        public async Task DeleteUserAsync(Guid userId, CancellationToken cancellationToken)
        {
            User? user = await _userStore.GetUserByIdAsync(userId, cancellationToken);
            if (user == null)
                throw new IncorrectIdException();
            await _userStore.DeleteUserAsync(user, cancellationToken);
            _cache.Remove(userId);
        }

        public async Task EditCredentialsAsync(Guid userId, Credentials credentials, CancellationToken cancellationToken)
        {
            User? user = await _userStore.GetUserByIdAsync(userId, cancellationToken);
            if (user == null)
                throw new IncorrectIdException();
            user.Login = credentials.Login;
            user.Password = credentials.Password;
            await _userStore.UpdateUserAsync(cancellationToken);
            _cache.Remove(userId);
        }

        public async Task EditNameAsync(Guid userId, string name, CancellationToken cancellationToken)
        {
            User? user = await _userStore.GetUserByIdAsync(userId, cancellationToken);
            if (user == null)
                throw new IncorrectIdException();
            user.Name = name;
            await _userStore.UpdateUserAsync(cancellationToken);
            _cache.Remove(userId);
        }

        public async Task UnassignAsAdminAsync(Guid userId, CancellationToken cancellationToken)
        {
            User? user = await _userStore.GetUserByIdAsync(userId, cancellationToken);
            if (user == null)
                throw new IncorrectIdException();
            user.IsAdmin = false;
            await _userStore.UpdateUserAsync(cancellationToken);
            _cache.Remove(userId);
        }
    }
}

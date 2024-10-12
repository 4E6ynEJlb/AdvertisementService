using Domain.Models.Entities;
using Domain.Models.ViewModels;
using Domain.Stores;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Persistence.Exceptions;

namespace Persistence.Repositories
{
    public class UserRepository : IUserStore
    {
        private readonly AdvertisementContext _context;
        public UserRepository(AdvertisementContext context)
        {
            _context = context;
        }
        public async Task<User?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken, bool tracking = true)
        {
            IQueryable<User> query = _context.Users;
            if (!tracking)
                query = query.AsNoTracking();
            return await query.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
        }

        public async Task<User?> GetUserByCredentialsAsync(Credentials credentials, CancellationToken cancellationToken)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Login == credentials.Login && u.Password == credentials.Password, cancellationToken);
        }

        public async Task<int> AddUserAsync(User user, CancellationToken cancellationToken)
        {
            try
            {
                _context.Users.Add(user);
                return await _context.SaveChangesAsync(cancellationToken);
            }
            catch(DbUpdateException e)
            {
                throw new RegistrationException(e);
            }
        }

        public async Task UpdateUserAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteUserAsync(User user, CancellationToken cancellationToken)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}

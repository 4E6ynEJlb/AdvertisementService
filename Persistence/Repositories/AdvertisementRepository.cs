using Domain.Models.ApplicationModels;
using Domain.Models.Entities;
using Domain.Models.ViewModels;
using Domain.Stores;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Persistence.Exceptions;

namespace Persistence.Repositories
{
    public class AdvertisementRepository : IAdvertisementStore
    {
        private readonly AdvertisementContext _context;
        public AdvertisementRepository(AdvertisementContext context)
        {
            _context = context;
        }
        public async Task<Advertisement?> GetAdvertisementByIdAsync(Guid advertisementId, CancellationToken cancellationToken, bool tracking = true)
        {
            IQueryable<Advertisement> query = _context.Advertisements;
            if (!tracking)
                query = query.AsNoTracking();
            return await query.FirstOrDefaultAsync(a => a.Id == advertisementId, cancellationToken);
        }

        public async Task<Advertisement[]> GetAdvertisementsByUserAsync(Guid userId, CancellationToken cancellationToken)
        {
            Advertisement[] result;
            using (IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.RepeatableRead))
            {
                result = await _context.Advertisements.Where(a => a.UserId == userId).AsNoTracking().ToArrayAsync(cancellationToken);
            }
            return result;
        }

        public async Task<AdvertisementsPages> GetAdvertisementsAsync(GetAdvertisementsOptions options, CancellationToken cancellationToken)
        {
            double? ratingHigh = options.RatingHigh;
            double? ratingLow = options.RatingLow;
            string? keyWord = options.KeyWord;
            bool isASC = options.IsASC;
            SortCriteria criterion = options.Criterion;
            int pageNumber = options.Page;
            int pageSize = options.PageSize;
            int pagesCount = 0;
            Advertisement[] advertisements;

            if (ratingHigh.HasValue && ratingLow.HasValue && ratingLow > ratingHigh)
                (ratingLow, ratingHigh) = (ratingHigh, ratingLow);
            IQueryable<Advertisement> query;
            switch (criterion)
            {
                case SortCriteria.Rating:
                    query = (isASC ? _context.Advertisements.OrderBy(a => a.Rating) : _context.Advertisements.OrderByDescending(a => a.Rating));
                    break;
                case SortCriteria.Created:
                    query = (isASC ? _context.Advertisements.OrderBy(a => a.Created) : _context.Advertisements.OrderByDescending(a => a.Created));
                    break;
                default:
                    throw new CriterionNotImplementedException();
            }
            if (keyWord != null)
                query = query.Where(a => EF.Functions.Like(a.Text, $"%{keyWord}%"));
            if (ratingLow.HasValue)
            {
                query = query.Where(a => a.Rating >= ratingLow);
            }
            if (ratingHigh.HasValue)
            {
                query = query.Where(a => a.Rating <= ratingHigh);
            }
            using (IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.RepeatableRead))
            {
                pagesCount = await query.CountAsync(cancellationToken);
                if (pagesCount > 0)
                    pagesCount = pagesCount / pageSize + ((pagesCount % pageSize) == 0 ? 0 : 1);
                else
                    pagesCount = 1;
                if (pageNumber > pagesCount || pageNumber < 1)
                    throw new InvalidPageException();
                query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
                advertisements = await query.AsNoTracking().ToArrayAsync(cancellationToken);
            }
            return new AdvertisementsPages { Advertisements = advertisements, PagesCount = pagesCount };
        }

        public async Task<Advertisement[]> GetOldAdvertisementsAsync(CancellationToken cancellationToken)
        {
            Advertisement[] result;
            using (IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.RepeatableRead))
            {
                result = await _context.Advertisements.Where(a => a.WillBeDeleted <= DateTime.UtcNow).ToArrayAsync(cancellationToken);
            }
            return result;
        }

        public async Task<int> GetUserAdvertisementsCountAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _context.Advertisements.Where(a => a.UserId == userId).CountAsync(cancellationToken);
        }

        public async Task<string?> GetImageContentTypeAsync(string imageName, CancellationToken cancellationToken)
        {
            return await _context.Advertisements.Where(a => a.ImageName == imageName).Select(a => a.ImageContentType).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<int> AddAdvertisementAsync(Advertisement advertisement, CancellationToken cancellationToken)
        {
            _context.Advertisements.Add(advertisement);
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAdvertisementAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAdvertisementAsync(Advertisement advertisement, CancellationToken cancellationToken)
        {
            _context.Advertisements.Remove(advertisement);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAdvertisementsAsync(Advertisement[] advertisements, CancellationToken cancellationToken)
        {
            _context.Advertisements.RemoveRange(advertisements);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}

using Domain.Models.ApplicationModels;
using Domain.Models.Entities;
using Domain.Models.ViewModels;

namespace Domain.Stores
{
    public interface IAdvertisementStore
    {
        public Task<Advertisement?> GetAdvertisementByIdAsync(Guid advertisementId, CancellationToken cancellationToken, bool tracking = true);
        public Task<AdvertisementsPages> GetAdvertisementsAsync(GetAdvertisementsOptions args, CancellationToken cancellationToken);
        public Task<Advertisement[]> GetAdvertisementsByUserAsync(Guid userId, CancellationToken cancellationToken);
        public Task<Advertisement[]> GetOldAdvertisementsAsync(CancellationToken cancellationToken);
        public Task<string?> GetImageContentTypeAsync(string imageName, CancellationToken cancellationToken);
        public Task<int> GetUserAdvertisementsCountAsync(Guid userId, CancellationToken cancellationToken);
        public Task<int> AddAdvertisementAsync(Advertisement advertisement, CancellationToken cancellationToken);
        public Task UpdateAdvertisementAsync(CancellationToken cancellationToken);
        public Task DeleteAdvertisementAsync(Advertisement advertisement, CancellationToken cancellationToken);
        public Task DeleteAdvertisementsAsync(Advertisement[] advertisements, CancellationToken cancellationToken);
    }
}

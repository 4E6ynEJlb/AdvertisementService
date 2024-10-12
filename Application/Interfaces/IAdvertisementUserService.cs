using Application.Models.ViewModels;
using Domain.Models.ViewModels;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces
{
    public interface IAdvertisementUserService
    {
        public Task<AdvertisementOutputModel> GetAdvertisementAsync(Guid advertisementId, CancellationToken cancellationToken);
        public Task<AdvertisementsPagesOutput> GetAdvertisementsAsync(GetAdvertisementsOptions options, CancellationToken cancellationToken);
        public Task<AdvertisementOutputModel[]> GetUserAdvertisementsAsync(Guid userId, CancellationToken cancellationToken);
        public Task<MemoryStream> GetImageAsync(string name, CancellationToken cancellationToken);
        public Task<string?> GetImageContentTypeAsync(string name, CancellationToken cancellationToken);
        public Task<Guid> CreateAdvertisementAsync(Guid userId, AdvertisementInputModel advertisementModel, CancellationToken cancellationToken);
        public Task EditAdvertisementAsync(Guid advertisementId, Guid userId, AdvertisementInputModel advertisementModel, CancellationToken cancellationToken);
        public Task<double> RateAdvertisementAsync(Guid advertisementId, int mark, CancellationToken cancellationToken);
        public Task<string> AttachImageAsync(Guid advertisementId, Guid userId, IFormFile file, CancellationToken cancellationToken);
        public Task DeleteImageAsync(Guid advertisementId, Guid userId, CancellationToken cancellationToken);
        public Task DeleteAdvertisementAsync(Guid advertisementId, Guid userId, CancellationToken cancellationToken);
    }
}

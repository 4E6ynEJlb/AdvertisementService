using Application.Interfaces;
using Application.Models.ApplicationModels;
using Application.Models.Exceptions;
using Application.Models.ViewModels;
using Domain.Models.ApplicationModels;
using Domain.Models.Entities;
using Domain.Models.ViewModels;
using Domain.Stores;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Application.Services
{
    public class AdvertisementsService : IAdvertisementAdminService, IAdvertisementUserService
    {
        private readonly IAdvertisementStore _advertisementStore;
        private readonly IUserStore _userStore;
        private readonly IMemoryCache _cache;
        private readonly ApplicationConfiguration _configuration;
        private readonly IFileClient _fileClient;
        public AdvertisementsService(IAdvertisementStore advertisementStore, IUserStore userStore, IMemoryCache cache,
            IOptions<ApplicationConfiguration> configuration, IFileClient fileClient)
        {
            _advertisementStore = advertisementStore;
            _userStore = userStore;
            _cache = cache;
            _configuration = configuration.Value;
            _fileClient = fileClient;
        }
        public async Task<AdvertisementOutputModel> GetAdvertisementAsync(Guid advertisementId, CancellationToken cancellationToken)
        {
            if (!_cache.TryGetValue(advertisementId, out Advertisement? advertisement))
                advertisement = await _advertisementStore.GetAdvertisementByIdAsync(advertisementId, cancellationToken, false);
            if (advertisement == null)
                throw new IncorrectIdException();
            if (_cache.TryGetValue(advertisementId, out _))
                _cache.Set(advertisementId, advertisement, new MemoryCacheEntryOptions()
                {
                    SlidingExpiration = TimeSpan.FromMinutes(_configuration.CacheSlidingExpirationMinutes)
                });
            return advertisement.ToAdvertisementOutputModel(_configuration.ImagesLinkTemplate);
        }

        public async Task<AdvertisementsPagesOutput> GetAdvertisementsAsync(GetAdvertisementsOptions options, CancellationToken cancellationToken)
        {
            AdvertisementsPages advertisementsPages = await _advertisementStore.GetAdvertisementsAsync(options, cancellationToken);
            return advertisementsPages.ToAdvertisementsPagesOutput(_configuration.ImagesLinkTemplate);
        }

        public async Task<AdvertisementOutputModel[]> GetUserAdvertisementsAsync(Guid userId, CancellationToken cancellationToken)
        {
            Advertisement[] advertisements = await _advertisementStore.GetAdvertisementsByUserAsync(userId, cancellationToken);
            AdvertisementOutputModel[] resultArray = new AdvertisementOutputModel[advertisements.Length];
            for (int index = 0; index < advertisements.Length; index++)
            {
                resultArray[index] = advertisements[index].ToAdvertisementOutputModel(_configuration.ImagesLinkTemplate);
            }
            return resultArray;
        }

        public async Task<MemoryStream> GetImageAsync(string name, CancellationToken cancellationToken)
        {
            return await _fileClient.GetAsync(name, cancellationToken);
        }

        public async Task<string?> GetImageContentTypeAsync(string name, CancellationToken cancellationToken)
        {
            return await _advertisementStore.GetImageContentTypeAsync(name, cancellationToken);
        }

        public async Task<string> AttachImageAsync(Guid advertisementId, Guid userId, IFormFile file, CancellationToken cancellationToken)
        {
            Advertisement? advertisement = await _advertisementStore.GetAdvertisementByIdAsync(advertisementId, cancellationToken);
            if (advertisement == null)
                throw new IncorrectIdException();
            if (advertisement.UserId != userId)
                throw new ForbiddenActionException();
            if (!file.ContentType.Contains("image"))
                throw new IncorrectFileFormatException();
            string filename = advertisementId.ToString();
            await _fileClient.SaveAsync(file.OpenReadStream(), filename, cancellationToken);
            if (advertisement.ImageName != null)
                await _fileClient.DeleteAsync(advertisement.ImageName, cancellationToken);
            advertisement.ImageName = filename;
            advertisement.ImageContentType = file.ContentType;
            await _advertisementStore.UpdateAdvertisementAsync(cancellationToken);
            _cache.Remove(advertisementId);
            return $"{_configuration.ImagesLinkTemplate}{filename}";
        }

        public async Task<Guid> CreateAdvertisementAsync(Guid userId, AdvertisementInputModel advertisementModel, CancellationToken cancellationToken)
        {
            if (!_cache.TryGetValue(userId, out User? user))
                user = await _userStore.GetUserByIdAsync(userId, cancellationToken, false);
            if (user == null)
                throw new IncorrectIdException();
            int userAdvertisementsCount = await _advertisementStore.GetUserAdvertisementsCountAsync(userId, cancellationToken);
            if (userAdvertisementsCount >= _configuration.UserAdvertisementsMaxCount && !user.IsAdmin)
                throw new ForbiddenActionException();
            DateTime created = DateTime.UtcNow;
            Advertisement advertisement = new Advertisement()
            {
                Text = advertisementModel.Text,
                AlreadyRated = 0,
                Rating = null,
                Id = Guid.NewGuid(),
                ImageName = null,
                ImageContentType = null,
                Number = advertisementModel.Number,
                UserId = userId,
                Created = created,
                WillBeDeleted = created.AddDays(_configuration.AdvertisementLifetimeDays)
            };
            if (await _advertisementStore.AddAdvertisementAsync(advertisement, cancellationToken) > 0)
                _cache.Set(advertisement.Id, advertisement, new MemoryCacheEntryOptions()
                {
                    SlidingExpiration = TimeSpan.FromMinutes(_configuration.CacheSlidingExpirationMinutes)
                });
            return advertisement.Id;
        }

        public async Task DeleteAdvertisementAsync(Guid advertisementId, Guid userId, CancellationToken cancellationToken)
        {
            Advertisement? advertisement = await _advertisementStore.GetAdvertisementByIdAsync(advertisementId, cancellationToken);
            if (advertisement == null)
                throw new IncorrectIdException();
            if (advertisement.UserId != userId)
                throw new ForbiddenActionException();
            if (advertisement.ImageName != null)
                await _fileClient.DeleteAsync(advertisement.ImageName, cancellationToken);
            await _advertisementStore.DeleteAdvertisementAsync(advertisement, cancellationToken);
            _cache.Remove(advertisementId);
        }

        public async Task DeleteAdvertisementAdminAsync(Guid advertisementId, Guid adminId, CancellationToken cancellationToken)
        {
            Advertisement? advertisement = await _advertisementStore.GetAdvertisementByIdAsync(advertisementId, cancellationToken);
            User? user = await _userStore.GetUserByIdAsync(adminId, cancellationToken);
            if (advertisement == null || user == null)
                throw new IncorrectIdException();
            if (!user.IsAdmin)
                throw new ForbiddenActionException();
            if (advertisement.ImageName != null)
                await _fileClient.DeleteAsync(advertisement.ImageName, cancellationToken);
            await _advertisementStore.DeleteAdvertisementAsync(advertisement, cancellationToken);
            _cache.Remove(advertisementId);
        }

        public async Task DeleteImageAsync(Guid advertisementId, Guid userId, CancellationToken cancellationToken)
        {
            Advertisement? advertisement = await _advertisementStore.GetAdvertisementByIdAsync(advertisementId, cancellationToken);
            if (advertisement == null)
                throw new IncorrectIdException();
            if (advertisement.UserId != userId)
                throw new ForbiddenActionException();
            if (advertisement.ImageName != null)
                await _fileClient.DeleteAsync(advertisement.ImageName, cancellationToken);
            advertisement.ImageName = null;
            advertisement.ImageContentType = null;
            await _advertisementStore.UpdateAdvertisementAsync(cancellationToken);
            _cache.Remove(advertisementId);
        }

        public async Task EditAdvertisementAsync(Guid advertisementId, Guid userId, AdvertisementInputModel advertisementModel, CancellationToken cancellationToken)
        {
            Advertisement? advertisement = await _advertisementStore.GetAdvertisementByIdAsync(advertisementId, cancellationToken);
            if (advertisement == null)
                throw new IncorrectIdException();
            if (advertisement.UserId != userId)
                throw new ForbiddenActionException();
            advertisement.Text = advertisementModel.Text;
            advertisement.Number = advertisementModel.Number;
            await _advertisementStore.UpdateAdvertisementAsync(cancellationToken);
            _cache.Remove(advertisementId);
        }

        public async Task<double> RateAdvertisementAsync(Guid advertisementId, int mark, CancellationToken cancellationToken)
        {
            if (mark < 1 || mark > 5)
                throw new IncorrectMarkException();
            Advertisement? advertisement = await _advertisementStore.GetAdvertisementByIdAsync(advertisementId, cancellationToken);
            if (advertisement == null)
                throw new IncorrectIdException();
            if (advertisement.AlreadyRated == 0 || !advertisement.Rating.HasValue)
            {
                advertisement.AlreadyRated = 1;
                advertisement.Rating = mark;
                await _advertisementStore.UpdateAdvertisementAsync(cancellationToken);
                return mark;
            }
            double ratungSum = advertisement.Rating.Value * advertisement.AlreadyRated;
            advertisement.AlreadyRated += 1;
            advertisement.Rating = (ratungSum + mark) / (advertisement.AlreadyRated);
            await _advertisementStore.UpdateAdvertisementAsync(cancellationToken);
            _cache.Remove(advertisementId);
            return advertisement.Rating.Value;
        }
    }
}

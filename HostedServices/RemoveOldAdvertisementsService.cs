using Application.Models.ApplicationModels;
using Domain.Stores;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace HostedServices
{
    public class RemoveOldAdvertisementsService : BackgroundService
    {
        private readonly IAdvertisementStore _advertisementStore;
        private readonly int _advertisementsRemovationTicksInterval;
        public RemoveOldAdvertisementsService(IServiceScopeFactory scopeFactory, IOptions<ApplicationConfiguration> configuration) 
        { 
            _advertisementStore = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IAdvertisementStore>();
            _advertisementsRemovationTicksInterval = configuration.Value.AdvertisementsRemovationTicksInterval;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _advertisementStore.DeleteAdvertisementsAsync(await _advertisementStore.GetOldAdvertisementsAsync(stoppingToken), stoppingToken);
                await Task.Delay(_advertisementsRemovationTicksInterval, stoppingToken);
            }
        }
    }
}

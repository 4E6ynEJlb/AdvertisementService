using Application.Interfaces;
using Application.Services;
using Domain.Stores;
using HostedServices;
using Infrastructure;
using Infrastructure.Interfaces;
using Persistence.Repositories;

namespace API.Extensions
{
    internal static class ServicesConfiguration
    {
        internal static void ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddMemoryCache();
            builder.Services.AddScoped<IFileClient, FileClient>();
            builder.Services.AddScoped<IUserStore, UserRepository>();
            builder.Services.AddScoped<IAdvertisementStore, AdvertisementRepository>();
            builder.Services.AddScoped<IAdvertisementAdminService, AdvertisementsService>();
            builder.Services.AddScoped<IAdvertisementUserService, AdvertisementsService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IUserHostService, UsersService>();
            builder.Services.AddScoped<IUserUserService, UsersService>();
            builder.Services.AddHostedService<RemoveOldAdvertisementsService>();
        }
    }
}

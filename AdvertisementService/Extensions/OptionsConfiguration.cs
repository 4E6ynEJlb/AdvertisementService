using Application.Models.ApplicationModels;
using Infrastructure;

namespace API.Extensions
{
    internal static class OptionsConfiguration
    {
        internal static void ConfigureOptions(this WebApplicationBuilder builder)
        {
            builder.Configuration.AddJsonFile("DBOptions.json");
            builder.Configuration.AddJsonFile("ApplicationOptions.json");

            builder.Services.Configure<ApplicationConfiguration>(builder.Configuration.GetSection(ApplicationConfiguration.OPTIONS_NAME));
            builder.Services.Configure<HostCredentials>(builder.Configuration.GetSection(HostCredentials.OPTIONS_NAME));
            builder.Services.Configure<MinioOptions>(builder.Configuration.GetSection(MinioOptions.OPTIONS_NAME));
        }
    }
}

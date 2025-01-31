using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Minio.AspNetCore;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Grafana.Loki;
using Serilog.Sinks.GrafanaLoki;

namespace API.Extensions
{
    internal static class DBConfiguration
    {
        internal static void ConfigureDatabases(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<AdvertisementContext>(options =>
            {
                string? connectionString = builder.Configuration.GetSection("SQLConnectionStrings").GetValue<string>("DefaultConnection");
                string? user = Environment.GetEnvironmentVariable("POSTGRES_USER");
                string? password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");
                string? database = Environment.GetEnvironmentVariable("POSTGRES_DB");
                connectionString = string.Format(connectionString ?? throw new ArgumentNullException("SQL Connection String"), user, password, database);
                options.UseNpgsql(connectionString);
            });
            builder.Services.AddMinio(options =>
            {
                options.AccessKey = builder.Configuration.GetSection("MinioOptions").GetValue<string>("AccessKey") ?? throw new ArgumentNullException("MinIo Access Key");
                options.SecretKey = builder.Configuration.GetSection("MinioOptions").GetValue<string>("SecretKey") ?? throw new ArgumentNullException("MinIo Secret Key");
                options.Endpoint = builder.Configuration.GetSection("MinioOptions").GetValue<string>("Endpoint") ?? throw new ArgumentNullException("MinIo Endpoint");
            });
            ConfigurationManager configuration = builder.Configuration;
            GrafanaLokiCredentials lokiCredentials = new GrafanaLokiCredentials()
            {
                User = builder.Configuration.GetSection("LokiOptions").GetValue<string>("User") ?? throw new ArgumentNullException("Loki User"),
                Password = builder.Configuration.GetSection("LokiOptions").GetValue<string>("Password") ?? throw new ArgumentNullException("Loki Password")
            };
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .WriteTo.GrafanaLoki(
                    builder.Configuration.GetSection("LokiOptions").GetValue<string>("URI") ?? throw new ArgumentNullException("Loki URI"),
                    lokiCredentials,
                    new Dictionary<string, string> { { "app", "Serilog.Sinks.GrafanaLoki.Sample" } },
                    LogEventLevel.Information
                ).CreateLogger();
            builder.Host.UseSerilog();
            builder.Logging.AddSerilog();
        }
    }
}

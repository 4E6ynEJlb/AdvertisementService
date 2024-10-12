using API.Extensions;
using API.Middleware;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace AdvertisementService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(options => 
            { 
                options.ConfigureSwaggerGen();
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "API.xml"));
            });
            builder.ConfigureOptions();
            builder.ConfigureDatabases();
            builder.ConfigureServices();

            builder.Services.AddAuthorization(options => options.ConfigureRoles());
            builder.ConfigureAuthentication();

            var app = builder.Build();
            app.UseAuthentication();
            app.UseAuthorization();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<ExceptionHandlerMiddleware>();
            app.UseMiddleware<RequestLoggingMiddleware>();

            app.UseHttpsRedirection();
            app.MapControllers();
            using (IServiceScope scope = app.Services.CreateScope())
            {
                scope.ServiceProvider.
                    GetRequiredService<AdvertisementContext>().
                    Database.Migrate();
            }
            app.Run();
        }
    }
}

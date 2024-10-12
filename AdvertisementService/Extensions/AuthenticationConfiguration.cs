using Application.Models.ApplicationModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace API.Extensions
{
    internal static class AuthenticationConfiguration
    {
        public static void ConfigureRoles(this Microsoft.AspNetCore.Authorization.AuthorizationOptions options)
        {
            options.AddPolicy(Policies.USER, policy => policy.RequireRole(Roles.USER, Roles.ADMIN));
            options.AddPolicy(Policies.ADMIN, policy => policy.RequireRole(Roles.ADMIN));
            options.AddPolicy(Policies.HOST, policy => policy.RequireRole(Roles.HOST));
        }
        internal static void ConfigureSwaggerGen(this SwaggerGenOptions options)
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = @"Enter JWT token.",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                    },
                    new List<string>()
                }
            });            
        }
        internal static void ConfigureAuthentication(this WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
                options =>
                {
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = AuthOptions.Issuer,
                        ValidateAudience = true,
                        ValidAudience = AuthOptions.Client,
                        ValidateLifetime = true,
                        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                        ValidateIssuerSigningKey = true,
                    };
                    options.SaveToken = true;
                }
            );
        }
    }
}

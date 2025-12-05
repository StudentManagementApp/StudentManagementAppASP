using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using StudentManagementApp.Application.Settings;
using StudentManagementApp.Application.Services;

namespace StudentManagementApp.Infrastructure
{
    public static class AuthenticationExtensions
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            // bind settings
            services.Configure<JwtSettings>(configuration.GetSection("Jwt"));
            var jwt = configuration.GetSection("Jwt").Get<JwtSettings>();
            if (jwt == null) throw new InvalidOperationException("JWT settings are not configured.");

            var keyBytes = Encoding.UTF8.GetBytes(jwt.Key);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwt.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwt.Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromSeconds(30)
                };
            });

            services.AddScoped<ITokenService, TokenService>();

            return services;
        }
    }
}

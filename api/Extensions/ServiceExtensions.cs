
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using repository.Context;
using System.Text;
using System.Threading.RateLimiting;

namespace api.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureSwagger(WebApplicationBuilder builder)
        {
            //https://learn.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-7.0&tabs=visual-studio
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Turning Points APIs",
                    Description = "This API provides a list of operations for the system",
                    // Terms of Service = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Pablo Fagundes",
                    },
                    License = new OpenApiLicense
                    {
                        Name = "All rights reserved",
                    }
                });

                options.AddSecurityDefinition(name: "Bearer", securityScheme: new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Enter authentication as: `Bearer JWT-Token`",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        new List<string>()
                    }
                });
            });
        }

        public static void ConfigureContext(WebApplicationBuilder builder, string connectionString)
        {
            builder.Services.AddDbContext<TurningPointsContext>(options => options.UseMySql(connectionString,
                ServerVersion.AutoDetect(connectionString),
                builder => builder.EnableRetryOnFailure().MigrationsAssembly("api"))
            );
        }

        public static void ConfigureControllerPolicy(WebApplicationBuilder builder, int rateLimitQuantity, int rateLimitTime, int rateLimitQueue)
        {
            builder.Services.AddRateLimiter(options =>
            {
                options.AddPolicy("Default", context =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "anonymous",
                        factory: partition => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = rateLimitQuantity, // Request limit
                            Window = TimeSpan.FromMinutes(rateLimitTime), // Per minute
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            QueueLimit = rateLimitQueue, // Tolerance for requests in queue
                        }));
            });
        }

        public static void ConfigureJWT(WebApplicationBuilder builder, string key)
        {
            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true
                };
            });
        }

        public static (string, string, int, int, int) GetParameters(WebApplicationBuilder builder, string connection, string jwtKey, string rateLimitQuantity, string rateLimitTime, string rateLimitQueue)
        {
            if (string.IsNullOrEmpty(connection) || string.IsNullOrEmpty(jwtKey))
                throw new Exception("Connection and key must be provided");

            connection = builder.Configuration[connection] ?? string.Empty;
            jwtKey = builder.Configuration[jwtKey] ?? string.Empty;

            rateLimitQuantity = builder.Configuration[rateLimitQuantity] ?? string.Empty;
            rateLimitTime = builder.Configuration[rateLimitTime] ?? string.Empty;
            rateLimitQueue = builder.Configuration[rateLimitQueue] ?? string.Empty;

            if (string.IsNullOrEmpty(connection) || string.IsNullOrEmpty(jwtKey))
                throw new Exception("Error finding database connection or Key in configuration file");

            if (string.IsNullOrEmpty(rateLimitQuantity) || string.IsNullOrEmpty(rateLimitTime) || string.IsNullOrEmpty(rateLimitQueue))
                throw new Exception("Error finding RateLimit parameters in configuration file");
            else
            {
                if (int.TryParse(rateLimitQuantity, out int rateLimitQuantityInt) && int.TryParse(rateLimitTime, out int rateLimitTimeInt) && int.TryParse(rateLimitQueue, out int rateLimitQueueInt))
                {
                    return (connection, jwtKey, rateLimitQuantityInt, rateLimitTimeInt, rateLimitQueueInt);
                }
                else
                {
                    throw new Exception("Error parsing RateLimit parameters from configuration file, they need to be integers");
                }
            }
        }
    }
}

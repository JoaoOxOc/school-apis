using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GatewayAPI.Configuration
{
    public static class CorsConfig
    {
        public static IServiceCollection ConfigureCors(this IServiceCollection services, IConfiguration configuration)
        {
            string[] corsUrl = configuration.GetSection("ApplicationSettings:Cors").Get<string[]>();
            //services.AddCors(options => options.AddPolicy("AllowAnyOrigin", builder => builder.WithOrigins("http://webapi.descobrirsps.lan", "http://descobrirsps.lan").SetIsOriginAllowed((host) => true).AllowAnyMethod().AllowAnyHeader().WithExposedHeaders(new string[] { "X-Total-Count", "X-Challenge-Status", "X-Event-Type", "X-Upload-Count" }))); services.AddCors(options =>
            services.AddCors(options =>
            {
                options.AddPolicy("Default",
                    builder => builder.AllowAnyOrigin()
                        .WithOrigins(corsUrl)
                        .SetIsOriginAllowed((host) => true)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .WithExposedHeaders("X-Total-Count"));
            });

            return services;
        }

        public static void UseDefaultCors(this IApplicationBuilder app, IConfiguration configuration)
        {
            //app.UseCors(builder => builder.WithOrigins("http://webapi.descobrirsps.lan", "http://descobrirsps.lan").SetIsOriginAllowed((host) => true).AllowAnyMethod().AllowAnyHeader().WithExposedHeaders(new string[] { "X-Total-Count", "X-Challenge-Status", "X-Event-Type", "X-Upload-Count" }));

            app.UseCors("Default");
        }
    }
}

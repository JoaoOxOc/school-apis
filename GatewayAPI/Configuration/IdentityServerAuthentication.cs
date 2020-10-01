using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace GatewayAPI.Configuration
{
    public class IdentityServerAuthentication
    {
        public static Action<IdentityServerAuthenticationOptions> ConfigureIdentityServerAllowedApiResources(IServiceCollection services, IConfiguration configuration)
        {
            var identityBuilder = services.AddAuthentication();
            IdentityServerConfig identityServerConfig = new IdentityServerConfig();
            configuration.Bind("IdentityServerConfig", identityServerConfig);

            Action<IdentityServerAuthenticationOptions> ocelotOptions = null;

            if (identityServerConfig != null && identityServerConfig.Resources != null)
            {
                foreach (var resource in identityServerConfig.Resources)
                {
                    identityBuilder.AddIdentityServerAuthentication(resource.Key, options =>
                    {
                        options.Authority = $"{identityServerConfig.ServerIP}:{identityServerConfig.ServerPort}";
                        options.RequireHttpsMetadata = false;
                        options.ApiName = resource.Name;
                        options.ApiSecret = resource.ApiSecret;
                        options.SupportedTokens = SupportedTokens.Both;
                    });
                }

                ocelotOptions = o => {
                    o.Authority = $"{identityServerConfig.ServerIP}:{identityServerConfig.ServerPort}";
                    o.RequireHttpsMetadata = false;
                    o.ApiName = "ocelot_api";
                    o.ApiSecret = "GsNwK3mwlEDrCCBR5uDgcSakFrHWLNSoL38duHk0TNE=";
                    o.SupportedTokens = SupportedTokens.Both;
                };
            }

            return ocelotOptions;
        }
    }
}

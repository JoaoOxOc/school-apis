﻿using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace SingleSignonPage.Util

{
    public static class Clients
    {

        public static IEnumerable<Client> GetAdminClient(IConfiguration configuration, IConfigurationOptions configOptions)
        {
            IdentityServerConfig configs = configOptions.getIdentityServerConfig();
            return new List<Client>
            {
                /*
                 * JP Project ID4 Admin Client
                 */
                new Client
                {

                    ClientId = "IS4-Admin",
                    ClientName = "IS4-Admin",
                    ClientUri = configuration["ApplicationSettings:IS4AdminUi"],
                    AllowedGrantTypes = GrantTypes.Code,
                    RequireConsent = true,
                    AllowAccessTokensViaBrowser = false,
                    RequireClientSecret = false,
                    RequirePkce = true,
                    AllowPlainTextPkce = false,
                    RedirectUris = new[] {
                        $"{configuration["ApplicationSettings:IS4AdminUi"]}/login-callback",
                        $"{configuration["ApplicationSettings:IS4AdminUi"]}/silent-refresh.html"
                    },
                    AllowedCorsOrigins = { configuration.GetValue<string>("ApplicationSettings:IS4AdminUi")},
                    PostLogoutRedirectUris = {$"{configuration["ApplicationSettings:IS4AdminUi"]}",},
                    LogoUri = "https://jpproject.blob.core.windows.net/images/jplogo.png",
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "role",
                        "jp_api.is4"
                    }
                },

                /*
                 * User Management Client - OpenID Connect implicit flow client
                 */
                new Client {
                    ClientId = "UserManagementUI",
                    ClientName = "User Management UI",
                    ClientUri = configuration["ApplicationSettings:UserManagementURL"],
                    AllowedGrantTypes = GrantTypes.Code,
                    AllowAccessTokensViaBrowser = false,
                    RequireConsent = true,
                    RequirePkce = true,
                    AllowPlainTextPkce = false,
                    RequireClientSecret = false,
                    RedirectUris =new[] {
                        $"{configuration["ApplicationSettings:UserManagementURL"]}/login-callback",
                        $"{configuration["ApplicationSettings:UserManagementURL"]}/silent-refresh.html"
                    },
                    AllowedCorsOrigins = { configuration["ApplicationSettings:UserManagementURL"] },
                    PostLogoutRedirectUris =  { $"{configuration["ApplicationSettings:UserManagementURL"]}" },
                    LogoUri = "https://jpproject.blob.core.windows.net/images/usermanagement.jpg",
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "jp_api.user",
                    }
                },
                /*
                 * Postman for testing - OpenID Connect implicit flow client
                 */
                new Client {
                    ClientId = "postman-api",
                    ClientName = "Postman Test Client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowAccessTokensViaBrowser = false,
                    RequireConsent = true,
                    RequirePkce = false,
                    AllowPlainTextPkce = false,
                    RequireClientSecret = true,
                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret(configs.Clients.Where(x => x.Key == "postman_api").FirstOrDefault().ClientSecret.Sha256())
                    },
                    AccessTokenType = AccessTokenType.Jwt,
                    RedirectUris =new[] { "https://www.getpostman.com/oauth2/callback" },
                    AllowedCorsOrigins = { "https://www.getpostman.com" },
                    PostLogoutRedirectUris =  { "https://www.getpostman.com" },
                    EnableLocalLogin = true,
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "logs_api.edit",
                        "institution_api.read",
                        "institution_api.edit"
                    }
                },
                /*
                 * Swagger
                 */
                new Client
                {
                    ClientId = "Swagger",
                    ClientName = "Swagger UI",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    LogoUri = "https://jpproject.blob.core.windows.net/images/swagger.png",
                    AllowAccessTokensViaBrowser = true,
                    RedirectUris =
                    {
                        $"{configuration.GetValue<string>("ApplicationSettings:ResourceServerURL")}/swagger/oauth2-redirect.html"
                    },
                    AllowedScopes =
                    {
                        "jp_api.user",
                        "jp_api.is4",
                    }
                }

            };

        }
    }
}
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace SingleSignonPage.Util
{
    public static class ClientResources
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {

            return new List<IdentityResource>
            {
                // some standard scopes from the OIDC spec
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),

                new IdentityResource("username", new []{ "username"}),
                // custom identity resource with some consolidated claims
                new IdentityResource("role", "Roles", new[] { JwtClaimTypes.Role }){Description = "A list of associated roles to user."},

                // add additional identity resource
                new IdentityResource("is4-rights", "IdentityServer4 Admin Panel Permissions", new [] { "is4-rights"}),

                new IdentityResource("admin-rights", "Backoffice Admin Panel Permissions", new [] { "admin-rights"}),
                new IdentityResource("external-user", "Common permission for premium public content", new [] { "external-user"})
            };
        }

        public static IEnumerable<ApiResource> GetApiResources(IConfiguration configuration)
        {
            IdentityServerConfig identityServerConfig = configuration.GetValue<IdentityServerConfig>("IdentityServerConfig");
            return new[]
            {
                new ApiResource
                                {
                                    Name = "jp_api",
                                    DisplayName = "JP API",
                                    Description = "OAuth2 Server Management Api",
                                    ApiSecrets = { new Secret(identityServerConfig.Resources.Where(x => x.Key == "jp_api").FirstOrDefault().ApiSecret.Sha256()) },

                                    UserClaims =
                                    {
                                        IdentityServerConstants.StandardScopes.OpenId,
                                        IdentityServerConstants.StandardScopes.Profile,
                                        IdentityServerConstants.StandardScopes.Email,
                                        "is4-rights",
                                        "username",
                                        "role"
                                    },

                                    Scopes =
                                    {
                                        new Scope()
                                        {
                                            Name = "jp_api.user",
                                            DisplayName = "User Management - Full access",
                                            Description = "Full access to User Management",
                                            Required = true
                                        },
                                        new Scope()
                                        {
                                            Name = "jp_api.is4",
                                            DisplayName = "OAuth2 Server",
                                            Description = "Manage mode to IS4",
                                            Required = true
                                        }
                                    }
                                },
                new ApiResource
                                {
                                    Name = "logs_api",
                                    DisplayName = "Logging API",
                                    Description = "Logging Service Api",
                                    ApiSecrets = { new Secret(identityServerConfig.Resources.Where(x => x.Key == "logs_api").FirstOrDefault().ApiSecret.Sha256()) },

                                    UserClaims =
                                    {
                                        IdentityServerConstants.StandardScopes.OpenId,
                                        IdentityServerConstants.StandardScopes.Profile,
                                        IdentityServerConstants.StandardScopes.Email,
                                        "admin-rights",
                                        "username",
                                        "role"
                                    },

                                    Scopes =
                                    {
                                        new Scope()
                                        {
                                            Name = "logs_api.read",
                                            DisplayName = "Logging service - read access",
                                            Description = "Is only able to check logs",
                                            Required = true
                                        },
                                        new Scope()
                                        {
                                            Name = "logs_api.edit",
                                            DisplayName = "Logging service - full access",
                                            Description = "Manage mode for logging service",
                                            Required = true
                                        }
                                    }
                                },
                new ApiResource
                                {
                                    Name = "ocelot_api",
                                    DisplayName = "Ocelot Admin API",
                                    Description = "Ocelot Admin Api",
                                    ApiSecrets = { new Secret(identityServerConfig.Resources.Where(x => x.Key == "ocelot_api").FirstOrDefault().ApiSecret.Sha256()) },

                                    UserClaims =
                                    {
                                        IdentityServerConstants.StandardScopes.OpenId,
                                        IdentityServerConstants.StandardScopes.Profile,
                                        IdentityServerConstants.StandardScopes.Email,
                                        "admin-rights",
                                        "username",
                                        "role"
                                    },

                                    Scopes =
                                    {
                                        new Scope()
                                        {
                                            Name = "ocelot_api.read",
                                            DisplayName = "ocelot - read access",
                                            Description = "Is only able to check routes",
                                            Required = true
                                        },
                                        new Scope()
                                        {
                                            Name = "ocelot_api.edit",
                                            DisplayName = "ocelot - full access",
                                            Description = "Manage mode for ocelot service",
                                            Required = true
                                        }
                                    }
                                },
                new ApiResource
                                {
                                    Name = "institution_api",
                                    DisplayName = "Institutions Admin API",
                                    Description = "Institutions Admin Api",
                                    ApiSecrets = { new Secret(identityServerConfig.Resources.Where(x => x.Key == "institution_api").FirstOrDefault().ApiSecret.Sha256()) },

                                    UserClaims =
                                    {
                                        IdentityServerConstants.StandardScopes.OpenId,
                                        IdentityServerConstants.StandardScopes.Profile,
                                        IdentityServerConstants.StandardScopes.Email,
                                        "admin-rights",
                                        "username",
                                        "role"
                                    },

                                    Scopes =
                                    {
                                        new Scope()
                                        {
                                            Name = "institution_api.read",
                                            DisplayName = "Institutions - read access",
                                            Description = "Is only able to read institutions data",
                                            Required = true
                                        },
                                        new Scope()
                                        {
                                            Name = "institution_api.edit",
                                            DisplayName = "Institutions - full access",
                                            Description = "Manage mode for institutions data",
                                            Required = true
                                        }
                                    }
                                }
                        };
        }
    }
}
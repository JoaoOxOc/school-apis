﻿using IdentityServer4.Stores;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SingleSignonPage.Util
{
    public static class Extensions
    {
        /// <summary>
        /// Determines whether the client is configured to use PKCE.
        /// </summary>
        /// <param name="store">The store.</param>
        /// <param name="client_id">The client identifier.</param>
        /// <returns></returns>
        public static async Task<bool> IsPkceClientAsync(this IClientStore store, string client_id)
        {
            if (!string.IsNullOrWhiteSpace(client_id))
            {
                var client = await store.FindEnabledClientByIdAsync(client_id);
                return client?.RequirePkce == true;
            }

            return false;
        }

        public static bool IsBehindReverseProxy(this IWebHostEnvironment host, IConfiguration configuration)
        {
            var config = configuration["ASPNETCORE_REVERSEPROXY"];
            return !string.IsNullOrEmpty(config) && config.Equals("true");
        }

        public static void AddIfDontExist(this List<Claim> claims, Claim newClaim)
        {
            if (claims.Any(c => c.Type == newClaim.Type))
                return;

            claims.Add(newClaim);
        }


        public static void Merge(this List<Claim> claims, IEnumerable<Claim> newClaim)
        {
            foreach (var claim in newClaim)
            {
                if (claims.Any(c => c.Type == claim.Type))
                    return;

                claims.Add(claim);
            }
        }
    }
}

using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IS4
{
    public static class Configuration
    {
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client()
                {
                    ClientId = "Console",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = {new Secret("Secret".ToSha256())},
                    AllowedScopes = {"api"},
                },
                new Client()
                {
                    ClientId = "Asp",
                    ClientSecrets = {new Secret("Secret".ToSha256())},
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris =
                    {
                        "https://localhost:44377/signin-oidc",
                    },
                    AllowedScopes =
                    {
                        "api",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                    },
                    RequireConsent = false,
                    PostLogoutRedirectUris = { "https://localhost:44377/signout-callback-oidc" },
                    AlwaysIncludeUserClaimsInIdToken = true,
                },

            };
        }
        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
            {
                new ApiScope("api")
            };
        }
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),

            };
        }
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("api", "My Api", new [] { JwtClaimTypes.Name})
                {
                    Scopes ={ "api" }
                },
            };

        }
    }
}

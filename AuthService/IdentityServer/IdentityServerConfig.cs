using Contract.Constants;
using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace AuthService.IdentityServer
{
    public static class IdentityServerConfig
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };

        public static IEnumerable<ApiScope> ApiScopes =>
             new ApiScope[]
             {
                  new ApiScope(CustomIdentityServerConstants.ApiScopeName, 
                      CustomIdentityServerConstants.ApiScopeDisplayName)
             };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                // machine to machine client
                new Client
                {
                    ClientId = "client",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    // scopes that client has access to
                    AllowedScopes = { CustomIdentityServerConstants.ApiScopeName }
                },

                // interactive ASP.NET Core MVC client
                new Client
                {
                    ClientId = ClientIdConstants.User,
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Code,

                    RedirectUris = { "https://localhost:44367/signin-oidc" },

                    PostLogoutRedirectUris = { "https://localhost:44367/signout-callback-oidc" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        CustomIdentityServerConstants.ApiScopeName
                    }
                },
                new Client
                {
                    ClientId = ClientIdConstants.Admin,
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Code,

                    RedirectUris = { "https://localhost:44368/signin-oidc" },

                    PostLogoutRedirectUris = { "https://localhost:44368/signout-callback-oidc" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        CustomIdentityServerConstants.ApiScopeName
                    }
                },
                new Client
                {
                    ClientId = ClientIdConstants.Pharmacy,
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Code,

                    RedirectUris = { "https://localhost:44370/signin-oidc" },

                    PostLogoutRedirectUris = { "https://localhost:44370/signout-callback-oidc" },
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        CustomIdentityServerConstants.ApiScopeName
                    }
                },
                new Client
                {
                    ClientId = ClientIdConstants.Doctor,
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Code,

                    RedirectUris = { "https://localhost:44369/signin-oidc" },

                    PostLogoutRedirectUris = { "https://localhost:44369/signout-callback-oidc" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        CustomIdentityServerConstants.ApiScopeName
                    }
                },

                new Client
                {
                    ClientId = "swagger",
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Code,

                    RequireConsent = false,
                    RequirePkce = true,

                    RedirectUris =           { $"https://localhost:5001/swagger/oauth2-redirect.html" },
                    PostLogoutRedirectUris = { $"https://localhost:5001/swagger/oauth2-redirect.html" },
                    AllowedCorsOrigins =     { $"https://localhost:5001" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        CustomIdentityServerConstants.ApiScopeName
                    }
                }
            };
    }
}

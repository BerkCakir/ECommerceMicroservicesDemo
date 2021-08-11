// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<ApiResource> ApiResources =>
              new ApiResource[]
              {
                  new ApiResource("product_catalog"){Scopes={"product_catalog_full_permission" } },
                  new ApiResource("photo_stock"){Scopes={ "photo_stock_full_permission" } },
                  new ApiResource("shopping_cart"){Scopes={ "shopping_cart_full_permission" } },
                  new ApiResource("discount"){Scopes={ "discount_full_permission" } },
                  new ApiResource("order"){Scopes={ "order_full_permission" } },
                  new ApiResource("gateway"){Scopes={ "gateway_full_permission" } },
                  new ApiResource("payment"){Scopes={ "payment_full_permission" } },
                  new ApiResource(IdentityServerConstants.LocalApi.ScopeName)
              };

        public static IEnumerable<IdentityResource> IdentityResources =>
                   new IdentityResource[]
                   {
                       new IdentityResources.Email(),
                       new IdentityResources.OpenId(),
                       new IdentityResources.Profile(),
                       new IdentityResource(){ Name="roles", DisplayName="Roles", Description="Kullanıcı rolleri", UserClaims=new []{"role" } }
                   };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("product_catalog_full_permission","Catalog API erişimi"),
                new ApiScope("photo_stock_full_permission","Photo Stock API erişimi"),
                new ApiScope("shopping_cart_full_permission","Shopping Cart API erişimi"),
                new ApiScope("discount_full_permission","Discount API erişimi"),
                new ApiScope("order_full_permission","Order API erişimi"),
                new ApiScope("gateway_full_permission","Gateway erişimi"),
                new ApiScope("payment_full_permission","Payment API erişimi"),
                new ApiScope(IdentityServerConstants.LocalApi.ScopeName)
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientName="Asp.Net Core MVC",
                    ClientId = "WebClient",
                    ClientSecrets={new Secret("password123".Sha256())},
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "product_catalog_full_permission", "photo_stock_full_permission", "gateway_full_permission", IdentityServerConstants.LocalApi.ScopeName }
                },
                new Client
                {
                    ClientName="Asp.Net Core MVC",
                    ClientId = "WebClientForUser",
                    ClientSecrets={new Secret("password123".Sha256())},
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowedScopes = { "shopping_cart_full_permission","discount_full_permission","order_full_permission", "gateway_full_permission","payment_full_permission",
                                    IdentityServerConstants.StandardScopes.Email, IdentityServerConstants.StandardScopes.OpenId,
                                    IdentityServerConstants.StandardScopes.Profile, IdentityServerConstants.StandardScopes.OfflineAccess,
                                    IdentityServerConstants.LocalApi.ScopeName ,"roles"},
                    AccessTokenLifetime = 3600,
                    RefreshTokenExpiration = TokenExpiration.Absolute,
                    AbsoluteRefreshTokenLifetime = (int)(DateTime.Now.AddDays(60) - DateTime.Now).TotalSeconds,
                    RefreshTokenUsage = TokenUsage.ReUse,
                    AllowOfflineAccess = true
                }
            };
    }
}
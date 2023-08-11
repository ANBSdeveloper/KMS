using Autofac.Extensions.DependencyInjection;
using Cbms.AspNetCore;
using Cbms.Kms.WebApi.Infrastructure.Middlewares;
using Hangfire;
using Hangfire.Dashboard;
using Hangfire.SqlServer;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Cbms.Kms.Web.Configuration
{
    public static class ServiceBuilderExtension
    {
        public static IServiceCollection AddProfiler(this IServiceCollection services)
        {
            services.AddMiniProfiler(options =>
            {
                options.RouteBasePath = "/profiler";
                options.ColorScheme = StackExchange.Profiling.ColorScheme.Light;
                options.PopupRenderPosition = StackExchange.Profiling.RenderPosition.BottomLeft;
                options.PopupShowTimeWithChildren = true;
                options.PopupShowTrivial = true;
                options.SqlFormatter = new StackExchange.Profiling.SqlFormatters.InlineFormatter();
                options.ResultsAuthorize = request => request.HttpContext.User.IsInRole(CbmsConsts.AdminRole);
            })
           .AddEntityFramework();

            return services;
        }

        public static IServiceCollection AddHangfire(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration["ConnectionString"];
            services.AddHangfire(configuration => configuration
               .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
               .UseSimpleAssemblyNameTypeSerializer()
               .UseRecommendedSerializerSettings()
               .UseSqlServerStorage(connectionString, new SqlServerStorageOptions
               {
                   CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                   SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                   QueuePollInterval = TimeSpan.Zero,
                   UseRecommendedIsolationLevel = true,
                   DisableGlobalLocks = true
               })
            );

            services.AddHangfireServer(options =>
            {
                options.Queues = new[] { "integration", "notification", "default" };
            });
            return services;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            string identityServerUrl = configuration[AppConfig.IdentityServerConfigKey];
            services.AddSwaggerGen(options =>
            {
                //options.CustomSchemaIds(x => x.FullName);
                options.SwaggerDoc(AppConfig.ApiVersion, new OpenApiInfo
                {
                    Version = AppConfig.ApiVersion,
                    Title = AppConfig.ApiName,
                    Description = AppConfig.ApiName,
                    Contact = new OpenApiContact
                    {
                        Name = AppConfig.ApiName,
                        Email = string.Empty,
                        Url = new Uri("https://twitter.com/wmslight"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "MIT License",
                        Url = new Uri("https://cbms.vn/wmslight/license"),
                    }
                });

                options.DocInclusionPredicate((docName, description) => true);
                // Define the BearerAuth scheme that's in use
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    Type = SecuritySchemeType.OAuth2,
                    In = ParameterLocation.Header,
                    Flows = new OpenApiOAuthFlows
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri($"{identityServerUrl}/connect/authorize"),
                            TokenUrl = new Uri($"{identityServerUrl}/connect/token"),
                            Scopes = new Dictionary<string, string>
                            {
                                {AppConfig.ApiResourceName, AppConfig.ApiName}
                            },
                        }
                    }
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                },
                                Scheme = "oauth2",
                                Name = "Bearer",
                                In = ParameterLocation.Header,
                            },
                        new List<string>()
                    }
                });
                options.OperationFilter<AuthorizeCheckOperationFilter>();
            });

            return services;
        }

        public static IServiceCollection AddCors(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors((options) =>
            {
                IConfigurationSection corSection = configuration.GetSection("ClientAllowedCors");

                var cors = corSection.AsEnumerable().Where(p => p.Value != null).Select(p => p.Value);
                options.AddPolicy(AppConfig.CorsPolicyName,
                       builder => builder
                       .WithOrigins(cors.ToArray())
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .AllowCredentials());
            });
            return services;
        }

        public static IServiceCollection AddCbmsCore(this IServiceCollection services, IWebHostEnvironment environment, IConfiguration configuration)
        {
            IConfigurationSection corSection = configuration.GetSection("ClientAllowedCors");
            var cors = corSection.AsEnumerable().Where(p => p.Value != null).Select(p => p.Value);
            string identityServerUrl = configuration[AppConfig.IdentityServerConfigKey];
            services.ConfigureCbmsAspNetCore(new IdentityServerOptions()
            {
                MigrationAssemblyName = typeof(Startup).Assembly.GetName().Name,
                Certificate = new X509Certificate2(Path.Combine(environment.ContentRootPath, "certificate.pfx"), "P@ssw0rd"),
                ApiScopes = new ApiScope[] { new ApiScope() { Name = "api" }, new ApiScope() { Name = "integration" } },
                ApiResources = new ApiResource[]
                  {
                        new ApiResource(AppConfig.ApiResourceName, AppConfig.ApiName)
                        {
                            ApiSecrets = new List<Secret>() {
                                new Secret(AppConfig.ApiSecret.Sha256())
                            },
                            Scopes = new string [] { "api", "integration" }
                        }
                  },
                IdentityResources = new IdentityResource[]
                  {
                        new IdentityResources.OpenId(),
                        new IdentityResources.Profile(),
                  },
                Clients = new Client[]
                {
                    new Client
                    {
                        ClientId = "js",
                        AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                        AllowAccessTokensViaBrowser = true,
                        ClientSecrets =
                        {
                            new Secret(AppConfig.ApiSecret.Sha256())
                        },
                        AllowedCorsOrigins = cors.ToArray(),
                        AccessTokenType = AccessTokenType.Reference,
                        AllowOfflineAccess = true,
                        AccessTokenLifetime = 3600 * 48,
                        AllowedScopes = {
                            AppConfig.ApiResourceName,
                            IdentityServerConstants.StandardScopes.OpenId,
                            IdentityServerConstants.StandardScopes.Profile
                        }
                    },
                    new Client
                    {
                        ClientId = "reward_app",
                        AllowedGrantTypes = GrantTypes.ClientCredentials,
                        AllowAccessTokensViaBrowser = true,
                        ClientSecrets =
                        {
                            new Secret("A3AB094C4E8BDD76C31A09A6A0079BC0B0077A81B963E247947AF9C9EEE42503".Sha256())
                        },
                        AllowedCorsOrigins = cors.ToArray(),
                        AccessTokenType = AccessTokenType.Reference,
                        AllowOfflineAccess = true,
                        AccessTokenLifetime = 3600 * 48,
                        AllowedScopes = {
                           "integration",
                        }
                    },
                     new Client
                    {
                        ClientId = "airtable",
                        AllowedGrantTypes = GrantTypes.ClientCredentials,
                        AllowAccessTokensViaBrowser = true,
                        ClientSecrets =
                        {
                            new Secret("A3AB094C4E8BDD76C31A09A6A0079BC0B0077A81B963E247947AF9C9EEE42503".Sha256())
                        },
                        AllowedCorsOrigins = cors.ToArray(),
                        AccessTokenType = AccessTokenType.Reference,
                        AllowOfflineAccess = true,
                        AccessTokenLifetime = 3600 * 48,
                        AllowedScopes = {
                           "integration",
                        }
                    },
                    new Client
                    {
                        ClientId = "swagger",
                        AllowedGrantTypes = GrantTypes.Implicit,
                        AllowAccessTokensViaBrowser = true,
                        RedirectUris = new List<string>() { $"{identityServerUrl}/swagger/oauth2-redirect.html" },
                        AllowOfflineAccess = true,
                        AccessTokenLifetime = 3600 * 48,
                        AllowedScopes = {
                            AppConfig.ApiResourceName,
                            IdentityServerConstants.StandardScopes.OpenId,
                            IdentityServerConstants.StandardScopes.Profile
                        }
                    }
                }
            });
            return services;
        }

        public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            string identityServerUrl = configuration[AppConfig.IdentityServerConfigKey];
            bool ssl = string.IsNullOrEmpty(configuration["IdentityServerHttps"]) ? true : Convert.ToBoolean(configuration["IdentityServerHttps"]);
            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.SlidingExpiration = true;
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
           .AddCookie()
           .AddIdentityServerAuthentication(options =>
           {
               options.Authority = identityServerUrl;
               options.ApiName = AppConfig.ApiResourceName;
               options.ApiSecret = AppConfig.ApiSecret;
               options.RequireHttpsMetadata = ssl;
               options.EnableCaching = true;
               options.CacheDuration = TimeSpan.FromMinutes(10);
           });
            return services;
        }

        public static IApplicationBuilder UseDefaultCors(this IApplicationBuilder app)
        {
            app.UseCors(AppConfig.CorsPolicyName);
            return app;
        }

        public static IApplicationBuilder UserHangfire(this IApplicationBuilder app)
        {
            GlobalConfiguration.Configuration.UseAutofacActivator(app.ApplicationServices.GetAutofacRoot());

            app.UseHangfireDashboard("/hangfire", new DashboardOptions()
            {
                Authorization = new IDashboardAuthorizationFilter[]
             {
                    new HangfireDashboardJwtAuthorizationFilter()
             }
            });
            return app;
        }

        public static IApplicationBuilder UseSwagger(this IApplicationBuilder app, IConfiguration configuration)
        {
            var pathBase = configuration["PathBase"];

            string identityServerUrl = configuration[AppConfig.IdentityServerConfigKey];
            app.UseSwagger().UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint($"{ (!string.IsNullOrEmpty(pathBase) ? pathBase : string.Empty) }/swagger/v1/swagger.json", $"{AppConfig.ApiName} {AppConfig.ApiVersion}");
                options.DisplayRequestDuration();
                options.OAuthClientId("swagger");
                options.OAuthAppName($"{AppConfig.AppName} Swagger UI");
                options.OAuth2RedirectUrl($"{identityServerUrl}/swagger/oauth2-redirect.html");
            });
            return app;
        }
    }

    public class HangfireDashboardJwtAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();
            return httpContext.User.IsInRole(CbmsConsts.AdminRole);
        }
    }
}
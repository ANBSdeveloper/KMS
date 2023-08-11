using Autofac;
using Cbms.Application;
using Cbms.AspNetCore;
using Cbms.AspNetCore.Web.Filters;
using Cbms.AspNetCore.Web.FiltersExceptionHandling;
using Cbms.Kms.Application;
using Cbms.Kms.Domain.Localization;
using Cbms.Kms.Infrastructure;
using Cbms.Kms.Infrastructure.OracleProvider;
using Cbms.Kms.Infrastructure.PgSqlProvider;
using Cbms.Kms.Web.Configuration;
using Cbms.Localization;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Cbms.Kms.Web
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(typeof(ResultFilterAttribute));
                options.Filters.Add(typeof(ApiExceptionFilterAttribute));
            });
            services.AddRazorPages();
            services.AddHealthChecks();
            services.AddProfiler();
            services.AddHangfire(Configuration);
            services.AddSwagger(Configuration);
            services.AddCors(Configuration);
            services.AddCbmsCore(Environment, Configuration);
            services.AddAuthentication(Configuration);
            services.AddAuthorization(p =>
            {
                p.AddPolicy("api", (policy) => policy.RequireScope("api"));
                p.AddPolicy("integration", (policy) => policy.RequireScope("integration", "api"));
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.ConfigCbmsModules<AppDbContext>((options) =>
            {
                var localizationConfiguration = new LocalizationConfiguration()
                {
                    DefaultLocalizationName = "Kms"
                };
                LocalizationConfigurer.Configure(localizationConfiguration);
                options.LocalizationConfiguration = localizationConfiguration;
                options.EncryptionKey = Configuration["Security:TextEncryptionKey"];
            });

            builder.RegisterType<AppConfiguration>().As<IAppConfiguration>();

            var oracleOptions = new OracleConnectionOptions()
            {
                Host = Configuration["OracleConnection:Host"],
                Port = Configuration["OracleConnection:Port"] == string.Empty ? 0 : Int32.Parse(Configuration["OracleConnection:Port"]),
                Sid = Configuration["OracleConnection:Sid"],
                User = Configuration["OracleConnection:User"],
                Password = Configuration["OracleConnection:Password"]
            };
            builder.RegisterInstance(oracleOptions).SingleInstance();

            var pgOptions = new PqConnectionOptions()
            {
                ConnectionString = Configuration["PgConnection:ConnectionString"]
            };
            builder.RegisterInstance(pgOptions).SingleInstance();
        }

        public void Configure(IApplicationBuilder app)
        {
            var pathBase = Configuration["PathBase"];
            if (!string.IsNullOrEmpty(pathBase))
            {
                app.UsePathBase(pathBase);
            }

            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCbms();

            app.UseSwagger(Configuration);

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseCors(AppConfig.CorsPolicyName);

            app.UseRouting();

            app.UseCookiePolicy();

            app.UseIdentityServer();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseMiniProfiler();

            app.UserHangfire();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health");
                endpoints.MapHangfireDashboard();
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            app.EnqueueJobs();
        }
    }
}
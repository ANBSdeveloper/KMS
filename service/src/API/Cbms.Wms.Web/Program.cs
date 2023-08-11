// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Autofac;
using Autofac.Extensions.DependencyInjection;
using Cbms.Kms.Infrastructure;
using Cbms.Kms.Infrastructure.EntityFramework.Seed;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

namespace Cbms.Kms.Web
{
    public class Program
    {
        public static int Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
              .Enrich.FromLogContext()
              .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}")
              .CreateLogger();

            try
            {
                Log.Information("Starting up");

                var host = CreateHostBuilder(args).Build();

                CreateDbIfNotExists(host);

                host.Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static void CreateDbIfNotExists(IHost host)
        {
            using (var scope = host.Services.GetAutofacRoot().BeginLifetimeScope())
            {
                try
                {
                    var grantDbContext = scope.Resolve<PersistedGrantDbContext>();
                    grantDbContext.Database.Migrate();

                    var hostDbContext = scope.Resolve<AppDbContext>();
                    DbInitializer.Initialize(hostDbContext).GetAwaiter().GetResult();
                }
                catch (Exception ex)
                {
                    var logger = scope.Resolve<ILogger>();
                    logger.Error(ex, "An error occurred creating the DB.");
                }
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
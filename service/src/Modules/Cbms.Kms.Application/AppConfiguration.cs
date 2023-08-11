using Cbms.Dependency;
using Cbms.Kms.Domain;
using Cbms.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;

namespace Cbms.Kms.Application
{
    public class AppConfiguration : IAppConfiguration, ISingletonDependency
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _hostEnvironment;

        public AppConfiguration(IConfiguration configuration, IWebHostEnvironment hostEnvironment)
        {
            _configuration = configuration;
            _hostEnvironment = hostEnvironment;
        }

        public string ConnectionString => _configuration["ConnectionString"];

        public string ImageStoragePath => _configuration["ImageStoragePath"];
        public string DefaultLocalizationName => KmsConsts.LocalizationSourceName;
        public string WebClientUrl => _configuration["WebClientUrl"];
        public string WebClientResetPath => _configuration["WebClientResetPath"];
        public string WebContentRoot => _hostEnvironment.WebRootPath;
        public string DistributedLockFolder => "";

        public SmtpConfiguration Smtp => new SmtpConfiguration()
        {
            Email = _configuration["Smtp:Email"],
            Host = _configuration["Smtp:Host"],
            Password = _configuration["Smtp:Password"],
            Port = int.Parse(_configuration["Smtp:Port"]),
            Username = _configuration["Smtp:Username"],
            Ssl = string.IsNullOrEmpty(_configuration["Smtp:Ssl"]) ? false : Convert.ToBoolean(_configuration["Smtp:Ssl"])
        };
    }
}
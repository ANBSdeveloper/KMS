using Microsoft.Extensions.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace Cbms.Kms.Domain.Connection
{
	[ExcludeFromCodeCoverage]
	public static class AppSettingsConnect
	{
		public static IConfiguration AppSetting { get; }

		static AppSettingsConnect()
		{
			AppSetting = new ConfigurationBuilder()
					.SetBasePath(System.IO.Directory.GetCurrentDirectory())
					.AddJsonFile("appsettings.json")
					.Build();
		}		

		//DB Connection String
		public static string ConnectionString => (AppSetting["ConnectionString"]).ToString();
		public static string ImgSavePath => (AppSetting["Crm:ImgSavePath"]).ToString();
		public static string ImgLivePath => (AppSetting["Crm:ImgLivePath"]).ToString();
		public static string ImportApiKey => (AppSetting["ImportApiKey"]).ToString();
		public static string LogsPath => (AppSetting["Crm:LogsPath"]).ToString();
		public static string MegaService_ApiURL => (AppSetting["MegaService:ApiURL"]).ToString();
		public static string MegaService_ApiUserName => (AppSetting["MegaService:ApiUserName"]).ToString();
		public static string MegaService_ApiPassword => (AppSetting["MegaService:ApiPassword"]).ToString();
		public static string ViettelService_ApiURL => (AppSetting["ViettelService:ApiURL"]).ToString();
		public static string ViettelService_ApiUserName => (AppSetting["ViettelService:ApiUserName"]).ToString();
		public static string ViettelService_ApiPassword => (AppSetting["ViettelService:ApiPassword"]).ToString();
		public static string ViettelService_ApiViettelUserId => (AppSetting["ViettelService:ApiViettelUserId"]).ToString();
	}
}

using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using SecretSanta2._0.Models;

namespace SecretSanta2._0
{
	public class Program
	{
		public static void Main(string[] args)
		{
			Log.Logger = new LoggerConfiguration()
				.MinimumLevel.Debug()
				.WriteTo.Console()
				.CreateLogger();

			Helpers.LoggerHelper.Log("Starting Service");

			string json = File.ReadAllText(@"appsettings.json");
			JObject o = JObject.Parse(@json);
			AppSettingsModel.appSettings = JsonConvert.DeserializeObject<AppSettingsModel>(o["AppSettings"].ToString());

			CreateWebHostBuilder(args).Build().Run();
		}

		public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.UseStartup<Startup>();
	}
}

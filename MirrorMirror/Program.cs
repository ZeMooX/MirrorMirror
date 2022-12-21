using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace MirrorMirror
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Hello, World!");
			StartWebServer("http://localhost:5000");
			Console.ReadLine();
		}

		public static void StartWebServer(string url)
		{
			var host = WebHost.CreateDefaultBuilder()
				.ConfigureLogging(loggingBuilder => loggingBuilder.ClearProviders())
				.UseSetting(WebHostDefaults.SuppressStatusMessagesKey, "True")
				.UseStartup<ServerStartup>()
				.UseUrls(url)
				.Build();

			host.Run();
		}
	}
}
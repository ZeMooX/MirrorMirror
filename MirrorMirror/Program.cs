using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Spectre.Console;

namespace MirrorMirror
{
	internal class Program
	{
		static void Main(string[] args)
		{
			var arguments = args.ToList();
			var bindUrls = new List<string>();
			var ports = new List<int>() { 8080 };
			var ip = "127.0.0.1";

			ShowStartupBanner();

			// Check if help banner should be shown
			if (arguments.Count == 0 || arguments.Any(x => x.Contains("-h") || x.Contains("--help") || x.Contains("-help")))
			{
				PrintHelp();
				return;
			}

			// Parse IP
			if (arguments.Any(x => x.Contains("-i")) && arguments.Count >= 2)
			{
				var ipIndex = arguments.IndexOf("-i") + 1;
				ip = arguments[ipIndex];
			}

			// Parse Ports
			if (arguments.Any(x => x.Contains("-p")) && arguments.Count >= 2)
			{
				var portIndex = arguments.IndexOf("-p") + 1;
				ports.Clear();
				
				for (; portIndex < arguments.Count; portIndex++)
				{
					var parsedPort = 0;
					var success = Int32.TryParse(arguments[portIndex], out parsedPort);

					if(!success || parsedPort == 0) {
						continue;
					}

					ports.Add(parsedPort);
				}
			}

			// Setup URLs to bind to
			
			foreach (var port in ports)
			{
				bindUrls.Add($"http://{ip}:{port}");
			}

			StartWebServer(bindUrls);
			Console.ReadLine();
		}

		private static void PrintHelp()
		{
			AnsiConsole.MarkupLine(@"
[royalblue1]-h[/]               Print this help.
[royalblue1]-p[/]               Comma seperated list of ports to listen on. Default is 8080. (i.e. -p 1337,8080,1234)
[royalblue1]-i[/]               IP to bind to. Default is 127.0.0.1. (i.e. -i 192.168.1.123)");
		}

		private static void StartWebServer(IList<string> bindUrls)
		{
			var host = WebHost.CreateDefaultBuilder()
				.ConfigureLogging(loggingBuilder => loggingBuilder.ClearProviders())
				.UseSetting(WebHostDefaults.SuppressStatusMessagesKey, "True")
				.UseStartup<ServerStartup>()
				.UseUrls(bindUrls.ToArray())
				.Build();

			AnsiConsole.Status()
			.AutoRefresh(true)
			.Spinner(Spinner.Known.Aesthetic)
			.Start("[green]Waiting for requests..[/]", ctx =>
			{
				AnsiConsole.MarkupLine($"Web Server bound to:");

				foreach (var url in bindUrls)
				{
					AnsiConsole.MarkupLine($"[royalblue1]{url}[/]");
				}

				AnsiConsole.MarkupLine("");

				try {
					host.Run();
				} catch(Exception e) {
					AnsiConsole.MarkupLine("[red](!) Exception while starting Web Server:[/]");
					AnsiConsole.MarkupLine(e.Message);
					AnsiConsole.MarkupLine("");
				}
			});

			// Done
			AnsiConsole.MarkupLine("[red](!) Web Server terminated[/]");

		}

		private static void ShowStartupBanner()
		{

			AnsiConsole.MarkupLine(@"[royalblue1]
 __  __ _                     __  __ _                     
|  \/  (_)__ _ __ _  ___ __ _|  \/  (_)__ _ __ _  ___ __ _ 
| |\/| | |__` |__` |/ _ |__` | |\/| | |__` |__` |/ _ |__` |
| |  | | |  | |  | | (_) | | | |  | | |  | |  | | (_) | | |
|_|  |_|_|  |_|  |_|\___/  |_|_|  |_|_|  |_|  |_|\___/  |_|
                                                           
[/]");
			var rule = new Rule();
			rule.RuleStyle("red dim");
			AnsiConsole.Write(rule);
		}
	}
}
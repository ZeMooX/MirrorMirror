using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Spectre.Console;

namespace MirrorMirror
{
	internal class ServerStartup
	{
		public void Configure(IApplicationBuilder app)
		{
			//app.UseRouting();
			app.Use(async (context, next) =>
			{
				if(RequestCount == 0)
				{
					Console.Clear();
				}

				RequestCount++;
				var request = context.Request;

				// Parse Body
				request.EnableBuffering();
				if (request.Body.CanSeek)
				{
					request.Body.Position = 0;
				}
				var rawRequestBody = new StreamReader(context.Request.Body).ReadToEnd();


				// Parse Headers
				var processedHeaders = new List<string>();
				foreach(var header in request.Headers)
				{
					processedHeaders.Add($"[grey74]{header.Key}:[/] {header.Value}");
				}

				PrintTableHeader(context.Connection.RemoteIpAddress.ToString(), context.Connection.RemotePort, request.Headers.Count);

				AnsiConsole.Markup($"[indianred1]{request.Method}[/] {request.GetDisplayUrl()} {request.Protocol}" + Environment.NewLine +
								   $"{string.Join(Environment.NewLine, processedHeaders)}" + Environment.NewLine + ""
								   + Environment.NewLine +
								   $"{rawRequestBody}");
				
				await next();
			});
		}

		private void PrintTableHeader(string originIP, int originPort, int headerCount)
		{
			var table = new Table();
			table.Border = TableBorder.Rounded;
			table.BorderColor(Color.Violet);
			table.Expand();
			table.AddColumn($"# {RequestCount}");
			table.AddColumn($"Received: {DateTime.Now}");
			table.AddColumn($"Origin IP: {originIP}");
			table.AddColumn($"Origin Port: {originPort}");
			table.AddColumn($"Headers: {headerCount}");
			AnsiConsole.Write(table);
		}

		private static int RequestCount = 0;
	}
}
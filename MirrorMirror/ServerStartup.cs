using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace MirrorMirror
{
	internal class ServerStartup
	{
		public void Configure(IApplicationBuilder app)
		{
			//app.UseRouting();
			app.Use(async (context, next) =>
			{
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
					processedHeaders.Add($"{header.Key}: {header.Value}");
				}

				var rawHTTP =
				$"{request.Method} {request.Scheme}://{request.Host}{request.Path} {request.Protocol}" + Environment.NewLine +
				$"{string.Join(Environment.NewLine, processedHeaders)}" + Environment.NewLine + ""
				+ Environment.NewLine +
				$"{rawRequestBody}";
				
				Console.Write(rawHTTP);
				await next();
			});

			//app.UseEndpoint();
		}
	}
}
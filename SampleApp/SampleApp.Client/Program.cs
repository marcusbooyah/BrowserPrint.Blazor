using BrowserPrint;

using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace SampleApp.Client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            builder.Services.AddBrowserPrint();

            await builder.Build().RunAsync();
        }
    }
}

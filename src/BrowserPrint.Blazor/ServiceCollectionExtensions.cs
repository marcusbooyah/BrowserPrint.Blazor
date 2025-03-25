using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrowserPrint
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBrowserPrint(this IServiceCollection services)
        {
            services.AddScoped<BrowserPrintService>();

            return services;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ShoppingCart.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var b = CreateHostBuilder(args).Build();

            var s = b.Services.GetService(typeof(IAuthenticationSchemeProvider)) as IAuthenticationSchemeProvider;

            b.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

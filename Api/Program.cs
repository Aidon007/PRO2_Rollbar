using System.IO;
using System.Reflection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Rollbar;

namespace Library
{
    public class Program
    {
        public static void Main(string[] args)
        {

            try
            {
                RollbarLocator.RollbarInstance.Configure(new RollbarConfig("7c66967c2011452b937f75663090f09c"));
                RollbarLocator.RollbarInstance.Info("Rollbar is configured properly.");
                CreateWebHostBuilder(args).Build().Run();
            }
            catch (System.Exception ex)
            {
                RollbarLocator.RollbarInstance.Error(ex);
            }

        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var env = hostingContext.HostingEnvironment;
                    var appAssembly = Assembly.Load(new AssemblyName(env.ApplicationName));

                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        .AddUserSecrets(appAssembly, optional: true)
                        .AddEnvironmentVariables();
                })
                .UseStartup<Startup>();
    }
}

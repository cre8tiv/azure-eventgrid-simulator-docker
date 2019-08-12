using System;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using AzureEventGridSimulator.Extensions;
using AzureEventGridSimulator.Settings;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace AzureEventGridSimulator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var host = WebHost.CreateDefaultBuilder()
                                  .UseSimulatorSettings()
                                  .UseStartup<Startup>()
                                  .ConfigureLogging((hostingContext, logging) =>
                                  {
                                      logging.AddConsole(options =>
                                      {
                                          options.IncludeScopes = true;
                                          options.DisableColors = false;
                                      });
                                      logging.AddDebug();

                                      logging.SetMinimumLevel(LogLevel.Debug);

                                      logging.AddFilter("System", LogLevel.Warning);
                                      logging.AddFilter("Microsoft", LogLevel.Warning);
                                  })
                                  .UseKestrel(options =>
                                  {
                                      var simulatorSettings = (SimulatorSettings)options.ApplicationServices.GetService(typeof(SimulatorSettings));

                                      foreach (var topics in simulatorSettings.Topics)
                                      {
                                          options.Listen(IPAddress.Loopback, topics.Port,
                                                         listenOptions =>
                                                         {
                                                             if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                                                             {
                                                                 listenOptions.UseHttps(new X509Certificate2(@"cert.pfx", "password"));
                                                             }
                                                             else
                                                             {
                                                                 listenOptions.UseHttps(StoreName.My, "localhost", true);
                                                             }
                                                         });
                                      }
                                  })
                                  .Build();
                host.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}

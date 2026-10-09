using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using DorfInfoBot.API.Contexts;
using Microsoft.Extensions.Logging.Console;

namespace DorfInfoBot.API
{
    public class Program
    {

        public static void Main(string[] args)
        {
            
            try 
            {
                var host = CreateHostBuilder(args).Build();
                ILogger logger = host.Services.GetService<ILogger<Program>>();
                logger.LogInformation($"Initializing application ...");

                using (var scope = host.Services.CreateScope())
                {
                    try 
                    {
                        var context = scope.ServiceProvider.GetService<NewsContext>();
                        context.Database.Migrate();
                        logger.LogInformation($"Database migration executed.");
                    }
                    catch (Exception ex)
                    {
                        logger.LogCritical($"Database migration execution failed.", ex);
                    }
                }

                // run the application
                host.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Application stopped because of an exception.", ex);
                throw;
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(builder =>
                    builder.AddSimpleConsole(options =>
                        {
                            options.IncludeScopes = false;
                            options.SingleLine = true;
                            options.ColorBehavior = LoggerColorBehavior.Disabled;
                            options.TimestampFormat = "yyyy.MM.dd hh:mm:ss ";
                        }))
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

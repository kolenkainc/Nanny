using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nanny.Console.Commands;
using Nanny.Console.Database;
using Nanny.Console.IO;
using Serilog;
using Serilog.Core;

namespace Nanny.Console
{
    public class Startup
    {
        private IConfiguration _configuration;

        public Startup()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile(
                    $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json",
                    optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
        }

        public Logger CreateLogger()
        {
            return new LoggerConfiguration()
                .ReadFrom.Configuration(_configuration)
                .CreateLogger();
        }

        public IHost CreateHost(Serilog.ILogger logger)
        {
            return Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddLogging(l =>
                    {
                        l.ClearProviders();
                        l.AddSerilog(logger);
                    });
                    services.AddTransient<CommandList>();
                    services.AddTransient<HelpCommand>();
                    services.AddTransient<VersionCommand>();
                    services.AddTransient<LoginCommand>();
                    services.AddTransient<IPrinter, ConsolePrinter>();
                    services.AddTransient<IScanner, ConsoleScanner>();
                    services.AddDbContext<ApplicationContext>();
                })
                .Build();
        }
    }
}

using System;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nanny.Console.Commands;
using Nanny.Console.Commands.ExternalServices;
using Nanny.Console.Database;
using Nanny.Console.IO;
using Polly;
using Polly.Extensions.Http;
using Serilog;
using Serilog.Core;

namespace Nanny.Console
{
    public class Startup
    {
        private IConfiguration _configuration;

        public Startup()
        {
            var fs = new FileSystem();
            _configuration = new ConfigurationBuilder()
                .SetBasePath(fs.WorkingDirectory())
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
                    services.AddTransient<VersionCommand>();
                    services.AddTransient<LoginCommand>();
                    services.AddTransient<HelpCommand>();
                    services.AddTransient<WorklogCommand>();
                    services.AddTransient<CommandList>();
                    services.AddTransient<IPrinter, ConsolePrinter>();
                    services.AddTransient<IScanner, ConsoleScanner>();
                    services.AddHttpClient<IJira, Jira>()
                        .AddPolicyHandler(GetRetryPolicy());
                    services.AddDbContext<ApplicationContext>();
                })
                .Build();
        }
        
        static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }
    }
}

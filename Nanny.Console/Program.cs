using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nanny.Console.Commands;
using Nanny.Console.Database;
using Serilog;

namespace Nanny.Console
{
    public class Program
    {
        private IHost _host;
        private static ILogger _logger;

        public Program()
        {
            Startup startup = new Startup();
            _logger = startup.CreateLogger();
            _host = startup.CreateHost(_logger);
            using (var scope = _host.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
                db.Database.Migrate();
            }
        }

        static void Main(string[] args)
        {
            try
            {
                Program program = new Program();
                _logger.Information("Getting started...");
                program.Run(args);
            }
            catch (Exception ex)    
            {
                _logger.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public void Run(string[] args)
        {
            ActivatorUtilities.CreateInstance<CommandList>(_host.Services)
                .Find(args, ActivatorUtilities.CreateInstance<HelpCommand>(_host.Services))
                .Execute();
        }
    }
}

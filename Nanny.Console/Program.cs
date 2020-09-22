using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nanny.Console.Commands;
using Nanny.Console.Database;
using Nanny.Console.Printers;
using Serilog;

namespace Nanny.Console
{
    public class Program
    {
        private IHost _host;
        private ILogger _logger;

        public Program()
        {
            try
            {
                Startup startup = new Startup();
                _logger = startup.CreateLogger();
                _logger.Information("Getting started...");
                _host = startup.CreateHost(_logger);
                using (var scope = _host.Services.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
                    db.Database.Migrate();
                }
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

        static void Main(string[] args)
        {
            Program program = new Program();
            program.Run(args);
        }

        public void Run(string[] args)
        {
            var list = ActivatorUtilities.CreateInstance<CommandList>(_host.Services);
            ActivatorUtilities.CreateInstance<ConsolePrinter>(
                _host.Services,
                list.Find(args, ActivatorUtilities.CreateInstance<HelpCommand>(_host.Services))
                )
                .Print();
        }
    }
}

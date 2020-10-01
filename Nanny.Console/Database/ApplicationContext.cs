using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nanny.Console.IO;

namespace Nanny.Console.Database
{
    public class ApplicationContext : DbContext
    {
        private ILoggerFactory _loggerFactory;

        public ApplicationContext(DbContextOptions<ApplicationContext> options, ILoggerFactory loggerFactory)
            : base(options)
        {
            _loggerFactory = loggerFactory;
        }
    
        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            var logger = _loggerFactory.CreateLogger<ApplicationContext>();
            var fs = new FileSystem();
            var connectionString = $"Data Source={Path.Combine(fs.WorkingDirectory(), "nanny.db")}";
            logger.LogInformation("ConnectionString: {@connectionString}", connectionString);
            builder.UseSqlite(connectionString);
            builder.UseLoggerFactory(_loggerFactory);
        }

        public virtual DbSet<Property> Properties { get; set; }
    }
}

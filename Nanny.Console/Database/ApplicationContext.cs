using System.IO;
using Microsoft.EntityFrameworkCore;
using Nanny.Console.IO;

namespace Nanny.Console.Database
{
    public class ApplicationContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            var fs = new FileSystem();
            builder.UseSqlite($"Data Source={Path.Combine(fs.WorkingDirectory(), "nanny.db")}");
        }

        public virtual DbSet<Property> Properties { get; set; }
    }
}

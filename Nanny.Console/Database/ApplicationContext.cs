using Microsoft.EntityFrameworkCore;

namespace Nanny.Console.Database
{
    public class ApplicationContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlite("Data Source=nanny.db");
        }

        public DbSet<Property> Properties { get; set; }
    }
}

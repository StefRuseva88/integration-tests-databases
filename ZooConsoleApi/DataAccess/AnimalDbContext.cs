using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ZooConsoleAPI.Data.Model;

namespace ZooConsoleAPI.DataAccess
{
    public class AnimalDbContext : DbContext
    {
        public virtual DbSet<Animal> Animals { get; set; }


        public AnimalDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public AnimalDbContext(IConfigurationRoot configurationRoot)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}

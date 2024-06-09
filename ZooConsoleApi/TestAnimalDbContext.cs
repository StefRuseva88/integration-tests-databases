using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ZooConsoleAPI.DataAccess;

namespace ZooConsoleAPI.IntegrationTests.NUnit
{
    public class TestAnimalDbContext : AnimalDbContext
    {
        public TestAnimalDbContext()
            : base(new ConfigurationBuilder().AddInMemoryCollection().Build())
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseInMemoryDatabase("TestDatabase");
            }
        }
    }
}

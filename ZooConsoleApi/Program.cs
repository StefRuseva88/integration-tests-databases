using Microsoft.EntityFrameworkCore;
using ZooConsoleAPI.Business;
using ZooConsoleAPI.DataAccess;

namespace ZooConsoleAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                var engine = new Engine();

                var contextFactory = new AnimalDbContextFactory();

                using var context = contextFactory.CreateDbContext(args);

                await context.Database.MigrateAsync();

                var animalsRepository = new AnimalRepository(context);
                var animalsManager = new AnimalsManager(animalsRepository);

                await DatabaseSeeder.SeedDatabaseAsync(context, animalsManager);

                await engine.Run(animalsManager);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured: {ex.Message}");
            }
        }
    }
}

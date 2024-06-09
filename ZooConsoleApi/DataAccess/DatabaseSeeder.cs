using System.Text.Json;
using ZooConsoleAPI.Business;
using ZooConsoleAPI.Data.Model;

namespace ZooConsoleAPI.DataAccess
{
    public class DatabaseSeeder
    {
        public static async Task SeedDatabaseAsync(AnimalDbContext context, AnimalsManager animalsManager)
        {
            if (context.Animals.Count() == 0)
            {
                string jsonFilePath = Path.Combine("Data", "Seed", "animals.json");
                string jsonData = File.ReadAllText(jsonFilePath);

                var animals = JsonSerializer.Deserialize<List<Animal>>(jsonData);

                if (animals != null)
                {
                    foreach (var animal in animals)
                    {
                        if (!context.Animals.Any(a => a.CatalogNumber == animal.CatalogNumber))
                        {
                            var newAnimal = new Animal()
                            {
                                Name = animal.Name,
                                CatalogNumber = animal.CatalogNumber,
                                Type = animal.Type,
                                Age = animal.Age,
                                Breed = animal.Breed,
                                Gender = animal.Gender,
                                IsHealthy = animal.IsHealthy,
                            };
                            await animalsManager.AddAsync(newAnimal);
                        }
                    }

                    await context.SaveChangesAsync();
                }
            }
        }
    }
}

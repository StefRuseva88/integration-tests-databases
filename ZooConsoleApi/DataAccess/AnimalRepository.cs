using Microsoft.EntityFrameworkCore;
using ZooConsoleAPI.Data.Model;
using ZooConsoleAPI.DataAccess.Contracts;

namespace ZooConsoleAPI.DataAccess
{
    public class AnimalRepository : IAnimalRepository
    {
        private readonly AnimalDbContext context;

        public AnimalRepository(AnimalDbContext context)
        {
            this.context = context;
        }
        public async Task AddAnimalAsync(Animal animal)
        {
            await context.Animals.AddAsync(animal);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAnimalAsync(string catalogNumber)
        {
            var animal = await context.Animals.FirstOrDefaultAsync(a => a.CatalogNumber == catalogNumber);
            if (animal != null)
            {
                context.Animals.Remove(animal);
                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Animal>> GetAllAnimalsAsync()
        {
            var animals = await context.Animals.ToListAsync();
            return animals;
        }

        public async Task<Animal> GetAnimalByCatalogNumberAsync(string catalogNumber)
        {
            var animal = await context.Animals.FirstOrDefaultAsync(a => a.CatalogNumber == catalogNumber);
            return animal;
        }

        public async Task<IEnumerable<Animal>> SearchAnimalsByType(string type)
        {
            var animal = await context.Animals.Where(a => a.Type == type).ToListAsync();
            return animal;
        }

        public async Task UpdateAnimalAsync(Animal animal)
        {
            context.Animals.Update(animal);
            await context.SaveChangesAsync();
        }
    }
}

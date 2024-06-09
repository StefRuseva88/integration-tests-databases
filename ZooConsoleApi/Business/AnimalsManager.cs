using System.ComponentModel.DataAnnotations;
using ZooConsoleAPI.Business.Contracts;
using ZooConsoleAPI.Data.Model;
using ZooConsoleAPI.DataAccess.Contracts;

namespace ZooConsoleAPI.Business
{
    public class AnimalsManager : IAnimalsManager
    {
        private readonly IAnimalRepository repository;

        public AnimalsManager(IAnimalRepository repository)
        {
            this.repository = repository;
        }

        public async Task AddAsync(Animal animal)
        {
            if (!IsValid(animal))
            {
                throw new ValidationException("Invalid animal!");
            }
            else
            {
                await repository.AddAnimalAsync(animal);
            }
        }

        public Task DeleteAsync(string catalogNumber)
        {
            if (string.IsNullOrWhiteSpace(catalogNumber))
            {
                throw new ArgumentException("Catalog number cannot be empty.");
            }

            return repository.DeleteAnimalAsync(catalogNumber);
        }

        public async Task<IEnumerable<Animal>> GetAllAsync()
        {
            var animals = await repository.GetAllAnimalsAsync();

            if (!animals.Any())
            {
                throw new KeyNotFoundException("No animal found.");
            }

            return animals;
        }

        public async Task<Animal> GetSpecificAsync(string catalogNumber)
        {
            if (string.IsNullOrWhiteSpace(catalogNumber))
            {
                throw new ArgumentException("Catalog number cannot be empty.");
            }

            var animal = await repository.GetAnimalByCatalogNumberAsync(catalogNumber);

            if (animal == null)
            {
                throw new KeyNotFoundException($"No animal found with catalog number: {catalogNumber}");
            }

            return animal; 
        }

        public async Task<IEnumerable<Animal>> SearchByTypeAsync(string type)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                throw new ArgumentException("Animal type cannot be empty.");
            }

            var animals = await repository.SearchAnimalsByType(type);

            if (animals == null || !animals.Any())
            {
                throw new KeyNotFoundException("No animal found with the given type.");
            }

            return animals;
        }

        public async Task UpdateAsync(Animal animal)
        {
            if (!IsValid(animal))
            {
                throw new ValidationException("Invalid animal!");
            }

            await repository.UpdateAnimalAsync(animal);
        }

        private bool IsValid(Animal animal)
        {
            if (animal == null)
            {
                return false;
            }

            var validateResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(animal);

            if (!Validator.TryValidateObject(animal, validationContext, validateResults, true))
            {
                foreach (var validationResult in validateResults)
                {
                    Console.WriteLine($"Validation Error: {validationResult.ErrorMessage}");
                }
                return false;
            }

            return true;
        }
    }
}

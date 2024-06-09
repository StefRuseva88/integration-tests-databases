using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Xml.Linq;
using ZooConsoleAPI.Business;
using ZooConsoleAPI.Business.Contracts;
using ZooConsoleAPI.Data.Model;
using ZooConsoleAPI.DataAccess;

namespace ZooConsoleAPI.IntegrationTests.NUnit
{
    public class IntegrationTests
    {
        private TestAnimalDbContext dbContext;
        private IAnimalsManager animalsManager;

        [SetUp]
        public void SetUp()
        {
            this.dbContext = new TestAnimalDbContext();
            this.animalsManager = new AnimalsManager(new AnimalRepository(this.dbContext));
        }


        [TearDown]
        public void TearDown()
        {
            this.dbContext.Database.EnsureDeleted();
            this.dbContext.Dispose();
        }


        //positive test
        [Test]
        public async Task AddAnimalAsync_ShouldAddNewAnimal()
        {
            // Arrange
            var newAnimal = new Animal
            {
                Type = "Mammal",
                CatalogNumber = "01HNTWXTQYTY",
                IsHealthy = true,
                Name = "Simba",
                Breed = "Lion",
                Gender = "Male",
                Age = 15
            };

            // Act
            await animalsManager.AddAsync(newAnimal);

            // Assert
            var addedAnimal = dbContext.Animals.FirstOrDefault(a => a.CatalogNumber == newAnimal.CatalogNumber);
            Assert.NotNull(addedAnimal);
            Assert.That(addedAnimal.Name, Is.EqualTo(newAnimal.Name));
            Assert.That(addedAnimal.Type, Is.EqualTo(newAnimal.Type));
        }

        //Negative test
        [Test]
        public async Task AddAnimalAsync_TryToAddAnimalWithInvalidCredentials_ShouldThrowException()
        {
            // Arrange
            var invalidAnimal = new Animal(); 

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await animalsManager.AddAsync(invalidAnimal));

        }

        [Test]
        public async Task DeleteAnimalAsync_WithValidCatalogNumber_ShouldRemoveAnimalFromDb()
        {
            // Arrange
            var newAnimal = new Animal
            {
                Type = "Mammal",
                CatalogNumber = "01HNTWXTQYTY",
                IsHealthy = true,
                Name = "Simba",
                Breed = "Lion",
                Gender = "Male",
                Age = 15
            };
            await dbContext.Animals.AddAsync(newAnimal);
            await dbContext.SaveChangesAsync();

            // Act
            await animalsManager.DeleteAsync(newAnimal.CatalogNumber);

            // Assert
            var deletedAnimal = dbContext.Animals.FirstOrDefault(a => a.CatalogNumber == newAnimal.CatalogNumber);
            Assert.That(deletedAnimal, Is.Null);
        }

        [Test]
        public async Task DeleteAnimalAsync_TryToDeleteWithNullOrWhiteSpaceCatalogNumber_ShouldThrowException()
        {
            // Arrange & Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await animalsManager.DeleteAsync(null));
            Assert.ThrowsAsync<ArgumentException>(async () => await animalsManager.DeleteAsync(""));
            Assert.ThrowsAsync<ArgumentException>(async () => await animalsManager.DeleteAsync("   "));
        }

        [Test]
        public async Task GetAllAsync_WhenAnimalsExist_ShouldReturnAllAnimals()
        {
            // Arrange
            var animal1 = new Animal
            {
                Type = "Mammal",
                CatalogNumber = "01HNTWXTR07A",
                IsHealthy = true,
                Name = "Simba",
                Breed = "Lion",
                Gender = "Male",
                Age = 15
            };
            var animal2 = new Animal
            {
                Type = "Eagle",
                CatalogNumber = "01HNTWXTR07A",
                IsHealthy = true,
                Name = "Seraphine",
                Breed = "Golden",
                Gender = "Female",
                Age = 8
            };
            await dbContext.Animals.AddRangeAsync(animal1, animal2);
            await dbContext.SaveChangesAsync();

            // Act
            var animals = await animalsManager.GetAllAsync();

            // Assert
            Assert.IsNotNull(animals);
            Assert.That(animals.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetAllAsync_WhenNoAnimalsExist_ShouldThrowKeyNotFoundException()
        {
            // Act & Assert
            var exception = Assert.ThrowsAsync<KeyNotFoundException>(async () => await animalsManager.GetAllAsync());
            Assert.That(exception.Message, Is.EqualTo("No animal found."));
        }

        [Test]
        public async Task SearchByTypeAsync_WithExistingType_ShouldReturnMatchingAnimals()
        {
            // Arrange
            var animal1 = new Animal
            {
                Type = "Mammal",
                CatalogNumber = "01HNTWXTR07A",
                IsHealthy = true,
                Name = "Simba",
                Breed = "Lion",
                Gender = "Male",
                Age = 15
            };
            var animal2 = new Animal
            {
                Type = "Mammal",
                CatalogNumber = "01HNTWXTR07A",
                IsHealthy = true,
                Name = "Simbad",
                Breed = "Tiger",
                Gender = "Male",
                Age = 12
            };
            await dbContext.Animals.AddRangeAsync(animal1, animal2);
            await dbContext.SaveChangesAsync();

            // Act
            var mammals = await animalsManager.SearchByTypeAsync("Mammal");

            // Assert
            Assert.IsNotNull(mammals);
            Assert.That(mammals.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task SearchByTypeAsync_WithNonExistingType_ShouldThrowKeyNotFoundException()
        {
            // Act & Assert
            var exception = Assert.ThrowsAsync<KeyNotFoundException>(async () => await animalsManager.SearchByTypeAsync("NonExistingType"));
            Assert.That(exception.Message, Is.EqualTo("No animal found with the given type."));
        }

        [Test]
        public async Task GetSpecificAsync_WithValidCatalogNumber_ShouldReturnAnimal()
        {
            // Arrange
            var animal = new Animal
            {
                Type = "Mammal",
                CatalogNumber = "01HNTWXTR07A",
                IsHealthy = true,
                Name = "Simba",
                Breed = "Lion",
                Gender = "Male",
                Age = 15
            };
            await dbContext.Animals.AddAsync(animal);
            await dbContext.SaveChangesAsync();

            // Act
            var retrievedAnimal = await animalsManager.GetSpecificAsync("01HNTWXTR07A");

            // Assert
            Assert.IsNotNull(retrievedAnimal);
            Assert.That(retrievedAnimal.Name, Is.EqualTo("Simba"));
        }

        [Test]
        public async Task GetSpecificAsync_WithInvalidCatalogNumber_ShouldThrowKeyNotFoundException()
        {
            // Act & Assert
            var exception = Assert.ThrowsAsync<KeyNotFoundException>(async () => await animalsManager.GetSpecificAsync("NonExistingCatalogNumber"));
            Assert.That(exception.Message, Is.EqualTo("No animal found with catalog number: NonExistingCatalogNumber"));
        }

        [Test]
        public async Task UpdateAsync_WithValidAnimal_ShouldUpdateAnimal()
        {
            // Arrange
            var animal = new Animal
            {
                Type = "Mammal",
                CatalogNumber = "01HNTWXTR07A",
                IsHealthy = true,
                Name = "Simba",
                Breed = "Lion",
                Gender = "Male",
                Age = 15
            };
            await dbContext.Animals.AddAsync(animal);
            await dbContext.SaveChangesAsync();

            // Update
            animal.Name = "Updated Lion";

            // Act
            await animalsManager.UpdateAsync(animal);

            // Assert
            var updatedAnimal = await dbContext.Animals.FirstOrDefaultAsync(a => a.CatalogNumber == "01HNTWXTR07A");
            Assert.IsNotNull(updatedAnimal);
            Assert.That(updatedAnimal.Name, Is.EqualTo("Updated Lion"));
        }

        [Test]
        public async Task UpdateAsync_WithInvalidAnimal_ShouldThrowValidationException()
        {
            // Arrange
            var invalidAnimal = new Animal(); 

            // Act & Assert
            var exception = Assert.ThrowsAsync<ValidationException>(async () => await animalsManager.UpdateAsync(invalidAnimal));
            Assert.That(exception.Message, Is.EqualTo("Invalid animal!"));
        }
    }
}


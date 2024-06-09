using ZooConsoleAPI.Business.Contracts;
using ZooConsoleAPI.Data.Model;

namespace ZooConsoleAPI.Business
{
    public class Engine : IEngine
    {
        public async Task Run(IAnimalsManager animalsManager)
        {
            bool exitRequested = false;
            while (!exitRequested)
            {
                Console.WriteLine($"{Environment.NewLine}Choose an option:");
                Console.WriteLine("1: Add Animal");
                Console.WriteLine("2: Delete Animal");
                Console.WriteLine("3: List All Animals");
                Console.WriteLine("4: Update Animal");
                Console.WriteLine("5: Find Animal by Type");
                Console.WriteLine("X: Exit");
                Console.Write("Enter your choice: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await AddAnimal(animalsManager);
                        break;
                    case "2":
                        await DeleteAnimal(animalsManager);
                        break;
                    case "3":
                        await ListAllAnimals(animalsManager);
                        break;
                    case "4":
                        await UpdateAnimal(animalsManager);
                        break;
                    case "5":
                        await FindAnimalByType(animalsManager);
                        break;
                    case "X":
                    case "x":
                        exitRequested = true;
                        break;
                    default:
                        Console.WriteLine("Invalid choice, please try again.");
                        break;
                }

                static async Task AddAnimal(IAnimalsManager animalsManager)
                {
                    Console.WriteLine("Adding a new animal:");

                    Console.Write("Enter Name: ");
                    var name = Console.ReadLine();

                    Console.Write("Enter Breed: ");
                    var breed = Console.ReadLine();

                    Console.Write("Enter Animal Type: ");
                    var type = Console.ReadLine();

                    Console.Write("Enter Animal Age: ");
                    var age = int.Parse(Console.ReadLine());

                    Console.Write("Choice Animal Gender: ");
                    var gender = Console.ReadLine();

                    Console.Write("Enter Animal Status (write 'yes' for healthy or any other value for unhealthy): ");
                    var status = Console.ReadLine();

                    var IsHealthy = false;
                    if (status.ToLower() == "yes")
                    {
                        IsHealthy = true;
                    }
                    else
                    {
                        IsHealthy = false;
                    }


                    Console.Write("Enter Catalog Number: ");
                    var catalogNumber = Console.ReadLine();

                    var newAnimal = new Animal
                    {
                        Type = type,
                        CatalogNumber = catalogNumber,
                        IsHealthy = IsHealthy,
                        Name = name,
                        Breed = breed,
                        Gender = gender,
                        Age = age
                    };

                    await animalsManager.AddAsync(newAnimal);
                    Console.WriteLine("Animal added successfully.");
                }

                static async Task DeleteAnimal(IAnimalsManager animalsManager)
                {
                    Console.Write("Enter Catalog Number to delete Animal entity from the data: ");
                    string catalogNumber = Console.ReadLine();
                    await animalsManager.DeleteAsync(catalogNumber);
                    Console.WriteLine("Animal deleted successfully.");
                }

                static async Task ListAllAnimals(IAnimalsManager animalsManager)
                {
                    var animals = await animalsManager.GetAllAsync();
                    if (animals.Any())
                    {
                        foreach (var animal in animals)
                        {
                            Console.WriteLine($"Catalog number: {animal.CatalogNumber}, Name: {animal.Name}, Age: {animal.Age}, Breed: {animal.Breed}, Type: {animal.Type}, Gender {animal.Gender}, Status: {(animal.IsHealthy ? "healthy" : "unhealthy")}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No such an animal.");
                    }
                }

                static async Task UpdateAnimal(IAnimalsManager animalsManager)
                {
                    Console.Write("Enter catalog number of the animal to update: ");
                    string catalogNumber = Console.ReadLine();
                    var animalToUpdate = await animalsManager.GetSpecificAsync(catalogNumber);
                    if (animalToUpdate == null)
                    {
                        Console.WriteLine("Animal not found.");
                        return;
                    }
                    
                    Console.Write("Enter new Name (leave blank to keep current): ");
                    var name = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(name))
                    {
                        animalToUpdate.Name = name;
                    }

                    Console.Write("Enter new type (leave blank to keep current): ");
                    var type = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(type))
                    {
                        animalToUpdate.Type = type;
                    }

                    Console.Write("Enter new Breed (leave blank to keep current): ");
                    var breed = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(breed))
                    {
                        animalToUpdate.Breed = breed;
                    }

                    Console.Write("Enter new Age (leave blank to keep current): ");

                    if (int.TryParse(Console.ReadLine(), out int age))
                    {
                        animalToUpdate.Age = age;
                    }
                    else
                    {
                        Console.WriteLine("Invalid age value! It will keep current value.");
                    }

                    Console.Write("Is animal healthy? (enter 'yes', 'no' or leave blank to keep current): ");
                    var status = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(status))
                    {
                        if(status.ToLower() == "yes")
                        {
                            animalToUpdate.IsHealthy = true;
                        }
                        else if (status.ToLower() == "no")
                        {
                            animalToUpdate.IsHealthy = false;
                        }
                        
                    }

                    Console.Write("Change the gender (leave blank to keep current): ");
                    var gender = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(gender))
                    {
                        animalToUpdate.Gender = gender;
                    }

                    await animalsManager.UpdateAsync(animalToUpdate);
                    Console.WriteLine("Animal updated successfully.");
                }

                static async Task FindAnimalByType(IAnimalsManager animalsManager)
                {
                    Console.Write("Enter animal type: ");
                    string type = Console.ReadLine();
                    var animals = await animalsManager.SearchByTypeAsync(type);

                    if (animals.Any())
                    {
                        foreach (var animal in animals)
                        {
                            Console.WriteLine();
                            Console.WriteLine($"Name: {animal.Name}, Type: {animal.Type}, Breed: {animal.Breed}");
                            Console.WriteLine($"--Age: {animal.Age}, Gender: {animal.Gender}");
                            Console.WriteLine($"---Status: {(animal.IsHealthy ? "healthy" : "unhealthy")}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No animal found with the given type fragment.");
                    }
                }
            }
        }
    }
}

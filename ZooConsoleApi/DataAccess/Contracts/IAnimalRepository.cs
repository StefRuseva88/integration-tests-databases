using ZooConsoleAPI.Data.Model;

namespace ZooConsoleAPI.DataAccess.Contracts
{
    public interface IAnimalRepository
    {
        Task<Animal> GetAnimalByCatalogNumberAsync(string catalogNumber);
        Task<IEnumerable<Animal>> GetAllAnimalsAsync();
        Task<IEnumerable<Animal>> SearchAnimalsByType(string type);
        Task AddAnimalAsync(Animal animal);
        Task UpdateAnimalAsync(Animal animal);
        Task DeleteAnimalAsync(string catalogNumber);
    }
}

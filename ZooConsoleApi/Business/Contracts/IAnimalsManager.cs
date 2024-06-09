using ZooConsoleAPI.Data.Model;

namespace ZooConsoleAPI.Business.Contracts
{
    public interface IAnimalsManager
    {
        Task AddAsync(Animal animal);
        Task<IEnumerable<Animal>> GetAllAsync();
        Task<IEnumerable<Animal>> SearchByTypeAsync(string type);
        Task<Animal> GetSpecificAsync(string catalogNumber);
        Task UpdateAsync(Animal animal);
        Task DeleteAsync(string catalogNumber);
    }
}

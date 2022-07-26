
namespace Domain.Repositories;

public interface IStrongDomainRepository<T>
{

    Task<List<T>> GetAllAsync();

    Task<T> GetByIdAsync(int id); 

    Task<string> CreateAsync(T obj);

    Task<string> DeleteAsync(T obj);

    Task<string> UpdateAsync(T obj);

    Task<int> SaveChangesAsync();

}


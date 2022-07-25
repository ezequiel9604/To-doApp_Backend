
namespace Domain.Repositories;

public interface IStrongDomainRepository<T>
{

    Task<List<T>> GetAllAsync();

    Task<T> GetByIdAsync(int id);

    Task<T> SaveChangesAsync();

    void CreateAsync(T obj);

    void DeleteAsync(T obj);

    void UpdateAsync(T obj);

}


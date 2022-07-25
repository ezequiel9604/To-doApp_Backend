
namespace Domain.Repositories;

public interface IStrongDomainRepository<T>
{

    Task<List<T>> GetAllAsync();

    Task<T> GetByIdAsync(int id);

    void SaveChangesAsync();

    void CreateAsync(T obj);

    void Delete(T obj);

    void Update(T obj);

}



namespace Domain.Repositories;

public interface IWeakDomainRepository<T>
{

    Task<T> GetByName(string name);

}


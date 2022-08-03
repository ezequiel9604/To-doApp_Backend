
using Domain.DTOs;

namespace Domain.Repositories;

public interface IUTaskRepository : IStrongDomainRepository<UTaskDTO>
{

    Task<List<UTaskDTO>> GetByUserId(int id);

}

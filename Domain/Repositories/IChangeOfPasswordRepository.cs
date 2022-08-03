
using Domain.DTOs;

namespace Domain.Repositories;

public interface IChangeOfPasswordRepository : IStrongDomainRepository<ChangeOfPasswordDTO>
{

    Task<List<ChangeOfPasswordDTO>> GetByUserId(int id);

}
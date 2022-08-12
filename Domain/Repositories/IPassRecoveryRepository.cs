
using Domain.DTOs;

namespace Domain.Repositories;

public interface IPassRecoveryRepository : IStrongDomainRepository<PassRecoveryDTO>
{

    Task<List<PassRecoveryDTO>> GetByUserId(int id);

}


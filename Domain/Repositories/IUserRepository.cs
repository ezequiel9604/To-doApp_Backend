
using Domain.DTOs;
using Domain.Entities;

namespace Domain.Repositories;

public interface IUserRepository : IStrongDomainRepository<UserDTO>
{
    Task<bool> CheckUserExists(string email);
}
    


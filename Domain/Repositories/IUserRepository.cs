
using Domain.DTOs;

namespace Domain.Repositories;

public interface IUserRepository : IStrongDomainRepository<UserDTO>
{

    Task<string> SignupAsync(UserDTO obj);

    Task<string> LogoutAsync(string email);

    Task<bool> CheckUserExists(string email);

    Task<object> LoginAsync(string email, string password);

    Task<string> SendMailForgotPassword(string email);

    Task<string> RestorePassword(string email, string password, string code);

}
    


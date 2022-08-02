
using Domain.Models;

namespace Domain.Repositories;

public interface IMailRepository
{

    Task SendMailAsync(MailRequest obj);

}


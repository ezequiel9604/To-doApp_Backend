
using Domain.Models;

namespace Domain.Repositories;

public interface IMailRepository
{

    Task SendEmailAsync(MailRequest obj);

}


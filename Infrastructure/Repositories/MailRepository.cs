
using MimeKit;
using MailKit;
using Domain.Models;
using Domain.Repositories;

namespace Infrastructure.Repositories;

public class MailRepository : IMailRepository
{
    public async Task SendEmailAsync(MailRequest obj)
    {
        
        throw new NotImplementedException();

    }
}


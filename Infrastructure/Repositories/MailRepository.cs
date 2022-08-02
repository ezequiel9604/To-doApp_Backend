
using MimeKit;
using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Domain.Models;
using Domain.Repositories;
using Infrastructure.Settings;
using Microsoft.Extensions.Options;
using System.Net.Sockets;

namespace Infrastructure.Repositories;

public class MailRepository : IMailRepository
{

    private readonly MailSettings _mailSettings;

    public MailRepository(IOptions<MailSettings> mailSettings)
    {
        _mailSettings = mailSettings.Value;
    }

    public async Task SendMailAsync(MailRequest mailRequest)
    {

        try
        {
            var message = new MimeMessage();
            message.Sender = MailboxAddress.Parse(_mailSettings.UserName);
            message.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            message.Subject = mailRequest.Subject;

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = mailRequest.Body;
            message.Body = bodyBuilder.ToMessageBody();

            var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.UserName, _mailSettings.Password);
            await smtp.SendAsync(message);

            smtp.Disconnect(true);
            smtp.Dispose();

        }
        catch (SocketException ex)
        {
            throw new SocketException(ex.ErrorCode);
        }
        catch (SmtpCommandException ex)
        {
            throw new SmtpCommandException(ex.ErrorCode, ex.StatusCode, ex.Message);
        }
        catch (SmtpProtocolException ex)
        {
            throw new SmtpProtocolException(ex.Message, ex);
        }

    }
}


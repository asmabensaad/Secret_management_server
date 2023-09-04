using DataAccess.Models.AuthService;

namespace Services.Auth.Repository;

public interface IEmailSender
{
    void SendEmail (Message message);
    Task SendEmailAsync(Message message);
}
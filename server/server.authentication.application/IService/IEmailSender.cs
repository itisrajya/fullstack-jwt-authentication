namespace server.authentication.application.IService;
public interface IEmailSender
{
    Task SendEmailAsync(string toEmail, string subject, string htmlBody);
}

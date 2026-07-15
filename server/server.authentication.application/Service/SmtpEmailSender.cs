namespace server.authentication.application.Service;

/// <summary>
/// SMTP-based implementation of <see cref="IEmailSender"/>.
/// Configuration is read from the "Smtp" section in appsettings.json.
/// TODO: Replace with a production-grade transactional email provider (e.g. SendGrid) if higher deliverability is required.
/// </summary>
public class SmtpEmailSender : IEmailSender
{
    private readonly IConfiguration _configuration;

    public SmtpEmailSender(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string htmlBody)
    {
        var smtpSection = _configuration.GetSection("Smtp");
        var host = smtpSection["Host"];
        var port = int.Parse(smtpSection["Port"] ?? "25");
        var username = smtpSection["Username"];
        var password = smtpSection["Password"];
        var fromAddress = string.IsNullOrEmpty(username) ? "no-reply@one-direction.dev" : username;

        using var client = new SmtpClient(host, port);
        if (!string.IsNullOrEmpty(username))
        {
            client.Credentials = new NetworkCredential(username, password);
            client.EnableSsl = true;
        }

        using var message = new MailMessage(fromAddress, toEmail, subject, htmlBody)
        {
            IsBodyHtml = true
        };

        await client.SendMailAsync(message);
    }
}

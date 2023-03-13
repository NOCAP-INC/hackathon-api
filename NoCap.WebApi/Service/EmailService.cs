using System.Net;
using System.Net.Mail;
using NoCap.Configs;
using NoCap.Request;

namespace NoCap.Service;

public class EmailService
{
    public async Task SendEmailAsync(EmailRequest emailRequest, SMTPConfig config)
    {
        MailMessage message = new MailMessage(config.SenderEmail,
            emailRequest.RecipientEmail, emailRequest.Body,emailRequest.Subject);
        message.IsBodyHtml = config.IsBodyHtml;
        
        SmtpClient smtpClient = new SmtpClient(config.ServerAddress, config.Port);
        smtpClient.EnableSsl = config.EnableSsl;
        smtpClient.UseDefaultCredentials = config.UseDefaultCredentials;
        smtpClient.Credentials = new NetworkCredential(config.SenderEmail, config.SenderPassword);
        
        smtpClient.Send(message);
    }
}
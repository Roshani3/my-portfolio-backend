using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Portfolio.Backend.Models;

namespace Portfolio.Backend.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(ContactRequest request)
        {
            var email = new MimeMessage();

            // Always send FROM your configured Gmail
            email.From.Add(MailboxAddress.Parse(_config["EmailSettings:SenderEmail"]));

            // Always send TO this fixed email
            email.To.Add(MailboxAddress.Parse("roshanipatil4212@gmail.com"));

            // Allow reply directly to user
            email.ReplyTo.Add(MailboxAddress.Parse(request.Email));

            email.Subject = $"Portfolio Inquiry from {request.Name}";

            email.Body = new TextPart("html")
            {
                Text = $@"
            <h3>Portfolio Contact Message</h3>
            <p><b>Name:</b> {request.Name}</p>
            <p><b>Email:</b> {request.Email}</p>
            <p><b>Message:</b><br/>{request.Message}</p>"
            };

            using var smtp = new SmtpClient();

            await smtp.ConnectAsync(
                _config["EmailSettings:SmtpServer"],
                int.Parse(_config["EmailSettings:SmtpPort"]),
                SecureSocketOptions.StartTls
            );

            await smtp.AuthenticateAsync(
                _config["EmailSettings:SenderEmail"],
                _config["EmailSettings:AppPassword"]
            );

            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}

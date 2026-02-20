// using MailKit.Net.Smtp;
// using MailKit.Security;
// using MimeKit;
// using Portfolio.Backend.Models;

// namespace Portfolio.Backend.Services
// {
//     public class EmailService : IEmailService
//     {
//         private readonly IConfiguration _config;

//         public EmailService(IConfiguration config)
//         {
//             _config = config;
//         }

//         // public async Task SendEmailAsync(ContactRequest request)
//         // {
//         //     var email = new MimeMessage();

//         //     // Always send FROM your configured Gmail
//         //     email.From.Add(MailboxAddress.Parse(_config["EmailSettings:SenderEmail"]));

//         //     // Always send TO this fixed email
//         //     email.To.Add(MailboxAddress.Parse("roshanipatil4212@gmail.com"));

//         //     // Allow reply directly to user
//         //     email.ReplyTo.Add(MailboxAddress.Parse(request.Email));

//         //     email.Subject = $"Portfolio Inquiry from {request.Name}";

//         //     email.Body = new TextPart("html")
//         //     {
//         //         Text = $@"
//         //     <h3>Portfolio Contact Message</h3>
//         //     <p><b>Name:</b> {request.Name}</p>
//         //     <p><b>Email:</b> {request.Email}</p>
//         //     <p><b>Message:</b><br/>{request.Message}</p>"
//         //     };

//         //     using var smtp = new SmtpClient();

//         //     await smtp.ConnectAsync(
//         //         _config["EmailSettings:SmtpServer"],
//         //         int.Parse(_config["EmailSettings:SmtpPort"]),
//         //         SecureSocketOptions.StartTls
//         //     );

//         //     await smtp.AuthenticateAsync(
//         //         _config["EmailSettings:SenderEmail"],
//         //         _config["EmailSettings:AppPassword"]
//         //     );

//         //     await smtp.SendAsync(email);
//         //     await smtp.DisconnectAsync(true);
//         // }
//     }
// }


using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Portfolio.Backend.Models;

namespace Portfolio.Backend.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;

        public EmailService(IConfiguration config)
        {
            _config = config;
            _httpClient = new HttpClient();
        }

        public async Task SendEmailAsync(ContactRequest request)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _config["Resend:ApiKey"]);

            var payload = new
            {
                from = "onboarding@resend.dev",
                to = "roshanipatil4212@gmail.com",
                reply_to = request.Email,
                subject = $"Portfolio Inquiry from {request.Name}",
                html = $@"
                    <h3>Portfolio Contact Message</h3>
                    <p><b>Name:</b> {request.Name}</p>
                    <p><b>Email:</b> {request.Email}</p>
                    <p><b>Subject:</b> {request.Subject}</p>
                    <p><b>Message:</b><br/>{request.Message}</p>"
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://api.resend.com/emails", content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Resend API error: {error}");
            }
        }
    }
}
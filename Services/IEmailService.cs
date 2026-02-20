using Portfolio.Backend.Models;

namespace Portfolio.Backend.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(ContactRequest request);
    }
}

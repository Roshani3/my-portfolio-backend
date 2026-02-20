using Microsoft.AspNetCore.Mvc;
using Portfolio.Backend.Models;
using Portfolio.Backend.Services;

namespace Portfolio.Backend.Controllers
{
    [ApiController]
    [Route("api/Contact")]
    public class ContactController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public ContactController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] ContactRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                await _emailService.SendEmailAsync(request);
                return Ok(new { message = "Email sent successfully" });
            }
            catch(Exception e)
            {
                return StatusCode(500, "Error sending email");
            }
        }
    }
}

using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using NaSpacerDo.Logic.Services;

namespace NaSpacerDo.Identity
{
    public class EmailService : IIdentityMessageService
    {
        private readonly IEmailService emailService;

        public EmailService(IEmailService emailService)
        {
            this.emailService = emailService;
        }

        public Task SendAsync(IdentityMessage message)
        {
            emailService.Send(message.Destination, message.Subject, message.Body);

            return Task.FromResult(0);
        }
    }
}
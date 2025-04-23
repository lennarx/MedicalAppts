using MediatR;
using MedicalAppts.Core.Contracts;
using MedicalAppts.Core.Events;
using Microsoft.Extensions.Logging;

namespace MedicalAppts.Core.EventHandlers
{
    public class UserBlockedEventHandler(IEmailService emailService, ILogger<UserBlockedEventHandler> logger) : INotificationHandler<UserBlockedEvent>
    {
        private readonly IEmailService _emailService = emailService;
        private readonly ILogger<UserBlockedEventHandler> _logger = logger;

        public async Task Handle(UserBlockedEvent notification, CancellationToken cancellationToken)
        {
            var subject = "Account Blocked Notification";
            var body = $"Your account has been blocked. Please contact support for further assistance.";

            await _emailService.SendEmailAsync(notification.Email, subject, body);
            _logger.LogInformation($"Block notification email sent to {notification.Email}", notification.Email);
        }
    }
}

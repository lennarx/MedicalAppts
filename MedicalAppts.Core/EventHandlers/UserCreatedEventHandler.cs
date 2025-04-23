using MediatR;
using MedicalAppts.Core.Contracts;
using MedicalAppts.Core.Events;
using Microsoft.Extensions.Logging;

namespace MedicalAppts.Core.EventHandlers
{
    public class UserCreatedEventHandler(IEmailService emailService, ILogger<UserCreatedEventHandler> logger) : INotificationHandler<UserCreatedEvent>
    {
        private readonly IEmailService _emailService = emailService;
        private readonly ILogger<UserCreatedEventHandler> _logger = logger;

        public async Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
        {
            var subject = "Welcome to the Medical Appointment System!";
            var body = $"Hi there! Your account has been successfully created. You can now start booking appointments.";

            await _emailService.SendEmailAsync(notification.Email, subject, body);
            _logger.LogInformation($"Welcome email sent to {notification.Email}");
        }
    }

}

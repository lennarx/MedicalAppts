using MediatR;
using MedicalAppts.Core.Contracts;
using MedicalAppts.Core.Events;
using Microsoft.Extensions.Logging;

namespace MedicalAppts.Core.EventHandlers
{
    public class AppointmentCreatedEventHandler(IEmailService emailService, ILogger<AppointmentCreatedEventHandler> logger) : INotificationHandler<AppointmentCreatedEvent>
    {
        private readonly IEmailService _emailService = emailService;
        private readonly ILogger<AppointmentCreatedEventHandler> _logger = logger;
        public async Task Handle(AppointmentCreatedEvent notification, CancellationToken cancellationToken)
        {
            var subject = "Appointment Confirmation";
            var body = $"Dear patient, your appointment with Dr. {notification.Doctor} on {notification.DateTime:dd/MM/yyyy HH:mm} has been successfully scheduled.";
            await _emailService.SendEmailAsync(notification.PatientEmail, subject, body);
            _logger.LogInformation("Email sent for AppointmentCreatedEvent");
        }
    }
}

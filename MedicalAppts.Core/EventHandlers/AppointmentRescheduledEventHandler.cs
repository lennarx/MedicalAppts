using MediatR;
using MedicalAppts.Core.Contracts;
using MedicalAppts.Core.Events;
using Microsoft.Extensions.Logging;

namespace MedicalAppts.Core.EventHandlers
{
    public class AppointmentRescheduledEventHandler(IEmailService emailService, ILogger<AppointmentRescheduledEventHandler> logger) : INotificationHandler<AppointmentRescheduledEvent>
    {
        private readonly IEmailService _emailService = emailService;
        private readonly ILogger<AppointmentRescheduledEventHandler> _logger = logger;

        public async Task Handle(AppointmentRescheduledEvent notification, CancellationToken cancellationToken)
        {
            var subject = "Appointment Rescheduled";
            var body = $"Dear patient, your appointment with Dr. {notification.Doctor}has been rescheduled to {notification.DateTime:dd/MM/yyyy HH:mm}.";
            await _emailService.SendEmailAsync(notification.PatientEmail, subject, body);
            _logger.LogInformation("Email sent for AppointmentRescheduledEvent");
        }
    }
}

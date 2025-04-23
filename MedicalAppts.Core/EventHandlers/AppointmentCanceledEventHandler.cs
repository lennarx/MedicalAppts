using MediatR;
using MedicalAppts.Core.Contracts;
using MedicalAppts.Core.Events;
using Microsoft.Extensions.Logging;

namespace MedicalAppts.Core.EventHandlers
{
    public class AppointmentCanceledEventHandler(IEmailService emailService, ILogger<AppointmentCanceledEventHandler> logger) : INotificationHandler<AppointmentCanceledEvent>
    {
        private readonly IEmailService _emailService = emailService;
        private readonly ILogger<AppointmentCanceledEventHandler> _logger = logger;

        public async Task Handle(AppointmentCanceledEvent notification, CancellationToken cancellationToken)
        {
            var subject = "Appointment Cancellation";
            var body = $"Dear patient, your appointment with Dr. {notification.Doctor} on {notification.DateTime:dd/MM/yyyy HH:mm} has been canceled.";
            await _emailService.SendEmailAsync(notification.PatientEmail, subject, body);
            _logger.LogInformation("Email sent for AppointmentCanceledEvent");
        }
    }
}

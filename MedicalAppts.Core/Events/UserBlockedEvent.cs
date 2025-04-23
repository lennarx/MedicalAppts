using MediatR;

namespace MedicalAppts.Core.Events
{
    public class UserBlockedEvent : INotification
    {
        public string Email { get; }
        public UserBlockedEvent(string email)
        {
            Email = email;
        }
    }
}

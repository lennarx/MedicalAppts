using MediatR;

namespace MedicalAppts.Core.Events
{
    public class UserCreatedEvent : INotification
    {
        public string Email { get; }
        public UserCreatedEvent(string email)
        {
            Email = email;
        }
    }
}

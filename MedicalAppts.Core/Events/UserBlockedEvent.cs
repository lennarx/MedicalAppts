using MediatR;

namespace MedicalAppts.Core.Events
{
    public class UserBlockedEvent : INotification
    {
        public int UserId { get; }
        public string Email { get; }
        public UserBlockedEvent(int userId, string email)
        {
            UserId = userId;
            Email = email;
        }
    }
}

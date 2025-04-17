using MedicalAppts.Core.Enums;

namespace MedicalAptts.UseCases.Users
{
    public class LoginForm
    {
        public int UserId { get; }
        public string Email { get; }
        public string Password { get; }
        public UserRole UserRole { get; }
        public LoginForm(string email, string password, int userId, UserRole userRole)
        {
            Email = email;
            Password = password;
            UserRole = userRole;
            UserId = userId;
        }
    }
}

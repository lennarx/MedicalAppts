using MedicalAppts.Core.Enums;

namespace MedicalAptts.UseCases.Users
{
    public class LoginForm
    {
        public string Email { get; }
        public string Password { get; }
        public LoginForm(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}

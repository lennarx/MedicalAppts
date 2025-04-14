using MediatR;
using MedicalAppts.Core;
using MedicalAppts.Core.Enums;

namespace MedicalAptts.UseCases.Users.Login
{
    public class LoginCommand : IRequest<Result<string, Error>>
    {
        public int UserId { get; }
        public string Email { get; }
        public string Password { get; }
        public UserRole UserRole { get; }
        public LoginCommand(int userId, string email, string password, UserRole userRole)
        {
            UserId = userId;
            Email = email;
            Password = password;
            UserRole = userRole;
        }
    }
}

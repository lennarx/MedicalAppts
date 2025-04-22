using MediatR;
using MedicalAppts.Core;
using MedicalAppts.Core.Enums;

namespace MedicalAptts.UseCases.Users.Login
{
    public class LoginCommand : IRequest<Result<string, Error>>
    {
        public string Email { get; }
        public string Password { get; }
        public LoginCommand(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}

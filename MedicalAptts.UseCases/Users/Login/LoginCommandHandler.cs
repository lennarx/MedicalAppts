using MediatR;
using MedicalAppts.Core;
using MedicalAppts.Core.Contracts;
using MedicalAppts.Core.Contracts.Repositories;
using MedicalAppts.Core.Entities;
using MedicalAppts.Core.Enums;
using MedicalAppts.Core.Errors;
using MedicalAppts.Core.Events;
using Microsoft.AspNet.Identity;
using Microsoft.Extensions.Logging;

namespace MedicalAptts.UseCases.Users.Login
{
    public class LoginCommandHandler(IDoctorsRepository doctorsRepository, IPatientsRepository patientRepository, ILogger logger, ICacheService cacheService, IMediator mediator, IJwtService jwtService) : IRequestHandler<LoginCommand, Result<string, Error>>
    {
        private readonly ILogger<LoginCommandHandler> _logger;
        private readonly IDoctorsRepository _doctorsRepository = doctorsRepository;
        private readonly IPatientsRepository _patientRepository = patientRepository;
        private readonly ICacheService _cacheService = cacheService;
        private readonly IMediator _mediator;
        private readonly IJwtService _jwtService;
        public async Task<Result<string, Error>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            IUserRepository userRepository;
            if (request.UserRole == UserRole.DOCTOR)
            {
                userRepository = _doctorsRepository;
            }
            else
            {
                userRepository = _patientRepository;
            }
            var user = await userRepository.GetUserByIdAsync(request.UserId);

            if (user == null)
            {
                _logger.LogError($"User with ID {request.UserId} not found.");
                return Result<string, Error>.Failure(LoginErrors.UserNotFound);
            }

            if (!user.Email.Equals(request.Email))
            {
                return Result<string, Error>.Failure(LoginErrors.EmailOrPasswordIncorrect);
            }

            var hasher = new PasswordHasher();
            var loginResult = hasher.VerifyHashedPassword(user.PasswordHash, request.Password);

            if (loginResult != PasswordVerificationResult.Success)
            {
                var cachekey = $"cacheEntry_{"Login"}_{user.Email}_{user.Id}";
                int tries = Convert.ToInt32(await _cacheService.GetAsync(cachekey)) + 1;
                _cacheService.SetAsync(cachekey, tries.ToString());

                if (tries >= 3)
                {
                    await _mediator.Publish(new UserBlockedEvent(user.Id, user.Email), cancellationToken);
                    _logger.LogError($"Too many login attempts, user: {user.Email} is blocked");
                    user.UserStatus = UserStatus.BLOCKED;
                    await userRepository.UpdateUserAsync(user);
                    return LoginErrors.UserBlocked;
                }
                else
                {
                    _logger.LogError($"Wrong password or email login attempt for user {user.Email}");
                    return LoginErrors.EmailOrPasswordIncorrect;
                }
            }

            return _jwtService.GenerateToken(user.Email, request.UserRole.ToString());
        }
    }
}

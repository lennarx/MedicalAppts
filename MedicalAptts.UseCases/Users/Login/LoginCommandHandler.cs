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
    public class LoginCommandHandler(IUserRepository userRepository, ILogger<LoginCommandHandler> logger, ICacheService cacheService, IMediator mediator, IJwtService jwtService) : IRequestHandler<LoginCommand, Result<string, Error>>
    {
        private readonly ILogger<LoginCommandHandler> _logger = logger;
        private readonly ICacheService _cacheService = cacheService;
        private readonly IMediator _mediator = mediator;
        private readonly IJwtService _jwtService = jwtService;
        private readonly IUserRepository _userRepository = userRepository;
        public async Task<Result<string, Error>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = _userRepository.GetFiltered(x => x.Email.Equals(request.Email), true).FirstOrDefault();

            if (user == null)
            {
                _logger.LogError($"User with email {request.Email} not found.");
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
                    await _userRepository.UpdateAsync(user);
                    return LoginErrors.UserBlocked;
                }
                else
                {
                    _logger.LogError($"Wrong password or email login attempt for user {user.Email}");
                    return LoginErrors.EmailOrPasswordIncorrect;
                }
            }

            return _jwtService.GenerateToken(user.Email, user.UserRole.ToString());
        }
    }
}

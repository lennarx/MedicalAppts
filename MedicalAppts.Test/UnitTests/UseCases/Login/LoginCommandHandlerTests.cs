using System.Threading;
using Moq;
using MedicalAptts.UseCases.Users.Login;
using Microsoft.Extensions.Logging;
using MediatR;
using MedicalAppts.Core.Contracts.Repositories;
using MedicalAppts.Core.Contracts;
using MedicalAppts.Core.Entities;
using MedicalAppts.Core.Enums;
using MedicalAppts.Core.Errors;
using MedicalAppts.Core.Events;
using Microsoft.AspNet.Identity;

public class LoginCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepoMock = new();
    private readonly Mock<ICacheService> _cacheMock = new();
    private readonly Mock<IMediator> _mediatorMock = new();
    private readonly Mock<ITokenService> _tokenServiceMock = new();
    private readonly Mock<ILogger<LoginCommandHandler>> _loggerMock = new();

    private LoginCommandHandler CreateHandler() =>
        new(_userRepoMock.Object, _loggerMock.Object, _cacheMock.Object, _mediatorMock.Object, _tokenServiceMock.Object);

    [Fact]
    public async Task Returns_UserNotFound_WhenEmailDoesNotExist()
    {
        var handler = CreateHandler();
        _userRepoMock.Setup(x => x.GetFiltered(It.IsAny<Func<User, bool>>(), true)).Returns(new List<User>().AsQueryable());

        var result = await handler.Handle(new LoginCommand("invalid@email.com", "1234"), CancellationToken.None);

        Assert.Equal(LoginErrors.UserNotFound.Message, result.Error?.Message);
    }

    [Fact]
    public async Task Returns_EmailOrPasswordIncorrect_WhenPasswordMismatch()
    {
        var handler = CreateHandler();
        var user = new User { Email = "valid@email.com", PasswordHash = new PasswordHasher().HashPassword("correct") };
        _userRepoMock.Setup(x => x.GetFiltered(It.IsAny<Func<User, bool>>(), true)).Returns(new[] { user }.AsQueryable());

        var result = await handler.Handle(new LoginCommand(user.Email, "wrong"), CancellationToken.None);

        Assert.Equal(LoginErrors.EmailOrPasswordIncorrect.Message, result.Error?.Message);
    }

    [Fact]
    public async Task Blocks_User_If_Password_Attempts_Reach_3()
    {
        var handler = CreateHandler();
        var user = new User { Id = 1, Email = "test@blocked.com", PasswordHash = new PasswordHasher().HashPassword("1234") };
        _userRepoMock.Setup(x => x.GetFiltered(It.IsAny<Func<User, bool>>(), true)).Returns(new[] { user }.AsQueryable());
        _cacheMock.Setup(x => x.GetAsync<int>(It.IsAny<string>())).ReturnsAsync(2);
        _cacheMock.Setup(x => x.SetAsync(It.IsAny<string>(), It.IsAny<string>(), null));
        _userRepoMock.Setup(x => x.UpdateAsync(It.IsAny<User>()));        

        var result = await handler.Handle(new LoginCommand(user.Email, "wrong"), CancellationToken.None);

        Assert.Equal(LoginErrors.UserBlocked.Message, result.Error?.Message);
        _mediatorMock.Verify(m => m.Publish(It.IsAny<UserBlockedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Returns_Token_WhenLoginSuccessful()
    {
        var handler = CreateHandler();
        var user = new User { Id = 1, Email = "login@ok.com", PasswordHash = new PasswordHasher().HashPassword("1234"), UserRole = UserRole.ADMIN };
        _userRepoMock.Setup(x => x.GetFiltered(It.IsAny<Func<User, bool>>(), true)).Returns(new[] { user }.AsQueryable());
        _cacheMock.Setup(x => x.GetAsync<int>(It.IsAny<string>())).ReturnsAsync(0);
        _tokenServiceMock.Setup(x => x.GenerateToken(user.Email, user.UserRole.ToString())).Returns("mock-token");

        var result = await handler.Handle(new LoginCommand(user.Email, "1234"), CancellationToken.None);

        Assert.Equal("mock-token", result.Value);
    }
}

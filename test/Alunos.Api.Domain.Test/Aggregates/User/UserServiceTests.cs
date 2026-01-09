using Alunos.Api.Domain.Aggregates.User;
using Alunos.Api.Domain.Aggregates.User.DTOs;
using Alunos.Api.Domain.Aggregates.User.Entities;
using Alunos.Api.Domain.SeedWork.ErrorResult;
using Microsoft.IdentityModel.Tokens;

namespace Alunos.Api.Domain.Test.Aggregates.User;

public class UserServiceTests
{
    private readonly IUserRepository _userRepository;
    private readonly UserService _userService;
    private readonly Fixture _fixture;

    public UserServiceTests()
    {
        _fixture = new Fixture();
        _userRepository = Substitute.For<IUserRepository>();
        _userService = new UserService(_userRepository);
    }

    [Fact]
    public async Task AuthenticateAsync_ShouldReturnToken_WhenCredentialsAreValid()
    {
        // Arrange
        var email = "test@example.com";
        var password = "password";
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
        var user = new Alunos.Api.Domain.Aggregates.User.Entities.User
        {
            Id = Guid.NewGuid(),
            Email = email,
            PasswordHash = passwordHash,
            IsActive = true
        };
        _userRepository.GetUserByEmailAsync(email).Returns(user);

        var secretKey = "super_secret_key_super_secret_key_super_secret_key";
        var issuer = "issuer";
        var audience = "audience";
        var expirationHours = 1;

        // Act
        var result = await _userService.AuthenticateAsync(email, password, secretKey, issuer, audience, expirationHours);

        // Assert
        Assert.NotNull(result.Item1);
        Assert.Equal(email, result.Item1.Email);
        Assert.False(string.IsNullOrEmpty(result.Item1.Token));
    }

    [Fact]
    public async Task AuthenticateAsync_ShouldReturnUnauthorized_WhenPasswordIsInvalid()
    {
        // Arrange
        var email = "test@example.com";
        var password = "password";
        var passwordHash = BCrypt.Net.BCrypt.HashPassword("wrong_password");
        var user = new Alunos.Api.Domain.Aggregates.User.Entities.User
        {
            Id = Guid.NewGuid(),
            Email = email,
            PasswordHash = passwordHash,
            IsActive = true
        };
        _userRepository.GetUserByEmailAsync(email).Returns(user);

        // Act
        var result = await _userService.AuthenticateAsync(email, password, "key", "issuer", "audience", 1);

        // Assert
        Assert.Null(result.Item1);
        Assert.Equal(ErrorCode.Unauthorized, result.Item2.StatusCode);
    }

    [Fact]
    public async Task RegisterAsync_ShouldReturnUser_WhenRegistrationIsSuccessful()
    {
        // Arrange
        var request = _fixture.Create<RegisterRequest>();
        _userRepository.GetUserByEmailAsync(request.Email).Returns((Alunos.Api.Domain.Aggregates.User.Entities.User?)null);

        // Act
        var result = await _userService.RegisterAsync(request);

        // Assert
        Assert.NotNull(result.Item1);
        Assert.Equal(request.Email, result.Item1.Email);
        await _userRepository.Received(1).AddAsync(Arg.Any<Alunos.Api.Domain.Aggregates.User.Entities.User>());
    }

    [Fact]
    public async Task RegisterAsync_ShouldReturnConflict_WhenUserAlreadyExists()
    {
        // Arrange
        var request = _fixture.Create<RegisterRequest>();
        var user = new Alunos.Api.Domain.Aggregates.User.Entities.User { Email = request.Email };
        _userRepository.GetUserByEmailAsync(request.Email).Returns(user);

        // Act
        var result = await _userService.RegisterAsync(request);

        // Assert
        Assert.Null(result.Item1);
        Assert.Equal(ErrorCode.Conflict, result.Item2.StatusCode);
    }
}

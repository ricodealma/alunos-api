using Alunos.Api.Domain.Aggregates.User.Entities;
using Alunos.Api.Infra.Data.User.Entities;
using Alunos.Api.Infra.Repositories;

namespace Alunos.Api.Infra.Test.Repositories;

public class UserRepositoryTests
{
    private readonly IUserDao _userDao;
    private readonly UserRepository _userRepository;
    private readonly Fixture _fixture;

    public UserRepositoryTests()
    {
        _fixture = new Fixture();
        _userDao = Substitute.For<IUserDao>();
        _userRepository = new UserRepository(_userDao);
    }

    [Fact]
    public async Task GetUserByEmailAsync_ShouldReturnUser_WhenUserExists()
    {
        // Arrange
        var email = "test@example.com";
        var userDto = _fixture.Create<UserDto>();
        userDto.Email = email;
        _userDao.GetUserByEmailAsync(email).Returns(userDto);

        // Act
        var result = await _userRepository.GetUserByEmailAsync(email);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userDto.Id, result.Id);
        Assert.Equal(userDto.Email, result.Email);
    }

    [Fact]
    public async Task GetUserByEmailAsync_ShouldReturnNull_WhenUserDoesNotExist()
    {
        // Arrange
        var email = "test@example.com";
        _userDao.GetUserByEmailAsync(email).Returns((UserDto?)null);

        // Act
        var result = await _userRepository.GetUserByEmailAsync(email);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task AddAsync_ShouldCallDaoAddAsync()
    {
        // Arrange
        var user = _fixture.Create<User>();

        // Act
        await _userRepository.AddAsync(user);

        // Assert
        await _userDao.Received(1).AddAsync(Arg.Is<UserDto>(u =>
            u.Id == user.Id &&
            u.Email == user.Email &&
            u.PasswordHash == user.PasswordHash &&
            u.IsActive == user.IsActive
        ));
    }
}

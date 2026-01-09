using Alunos.Api.Domain.Aggregates.User;
using Alunos.Api.Infra.Data.User.Entities;

namespace Alunos.Api.Infra.Repositories;

using DomainUser = Alunos.Api.Domain.Aggregates.User.Entities.User;

public sealed class UserRepository(IUserDao userDao) : IUserRepository
{
    private readonly IUserDao _userDao = userDao;

    public async Task<DomainUser?> GetUserByEmailAsync(string email)
    {
        var userDto = await _userDao.GetUserByEmailAsync(email);
        
        if (userDto == null) return null;

        return new DomainUser
        {
            Id = userDto.Id,
            Email = userDto.Email,
            PasswordHash = userDto.PasswordHash,
            IsActive = userDto.IsActive,
            CreatedAt = userDto.CreatedAt,
            UpdatedAt = userDto.UpdatedAt
        };
    }

    public async Task AddAsync(DomainUser user)
    {
        var userDto = new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            PasswordHash = user.PasswordHash,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };

        await _userDao.AddAsync(userDto);
    }
}

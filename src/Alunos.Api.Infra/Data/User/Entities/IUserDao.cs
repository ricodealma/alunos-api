using Alunos.Api.Domain.Aggregates.User.Entities;

namespace Alunos.Api.Infra.Data.User.Entities;

public interface IUserDao
{
    Task<UserDto?> GetUserByEmailAsync(string email);
    Task AddAsync(UserDto userDto);
}

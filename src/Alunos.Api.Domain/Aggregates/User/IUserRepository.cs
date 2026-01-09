using UserEntity = Alunos.Api.Domain.Aggregates.User.Entities.User;

namespace Alunos.Api.Domain.Aggregates.User;

public interface IUserRepository
{
    Task<UserEntity?> GetUserByEmailAsync(string email);
    Task AddAsync(UserEntity user);
}

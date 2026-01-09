using Microsoft.EntityFrameworkCore;
using Alunos.Api.Infra.Data.Aluno;

namespace Alunos.Api.Infra.Data.User.Entities;

public sealed class UserDao(IAlunoContext context) : IUserDao
{
    private readonly IAlunoContext _context = context;

    public async Task<UserDto?> GetUserByEmailAsync(string email)
    {
        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
    }

    public async Task AddAsync(UserDto userDto)
    {
        await _context.Users.AddAsync(userDto);
        await _context.SaveChangesAsync();
    }
}

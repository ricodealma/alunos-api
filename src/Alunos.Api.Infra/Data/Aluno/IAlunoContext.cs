using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Alunos.Api.Infra.Data.Aluno.Entities;
using Alunos.Api.Infra.Data.User.Entities;

namespace Alunos.Api.Infra.Data.Aluno
{
    public interface IAlunoContext
    {
        DbSet<AlunoDto> Aluno { get; set; }
        DbSet<UserDto> Users { get; set; }

        DatabaseFacade Database { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        DbSet<TEntity> Set<TEntity>() where TEntity : class;
    }
}

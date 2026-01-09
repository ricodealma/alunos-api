using Microsoft.EntityFrameworkCore;
using Alunos.Api.Domain.SeedWork;
using Alunos.Api.Infra.Data.Aluno.Entities;
using Alunos.Api.Domain.Aggregates.User.Entities;

namespace Alunos.Api.Infra.Data.Aluno
{
    public class AlunoContext(DbContextOptions<AlunoContext> options, EnvironmentKey environmentKey) : DbContext(options), IAlunoContext
    {
        private readonly EnvironmentKey _environmentKey = environmentKey;
        public DbSet<AlunoDto> Aluno { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = _environmentKey.PostgresInformation.ConnectionString;

            optionsBuilder.UseNpgsql(connectionString);

            if (EnvironmentKey.TypeInformation == EnvironmentKey.Type.DEV)
            {
                optionsBuilder
                    .EnableDetailedErrors()
                    .EnableSensitiveDataLogging();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AlunoDto>()
                .Property(o => o.Id)
                .ValueGeneratedNever();

            // User entity configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Email).HasMaxLength(255).IsRequired();
                entity.Property(e => e.PasswordHash).HasMaxLength(255).IsRequired();
                entity.Property(e => e.IsActive).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
            });
        }
    }
}

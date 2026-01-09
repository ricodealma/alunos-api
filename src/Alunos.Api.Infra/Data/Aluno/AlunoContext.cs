using Microsoft.EntityFrameworkCore;
using Alunos.Api.Domain.SeedWork;
using Alunos.Api.Infra.Data.Aluno.Entities;
using Alunos.Api.Infra.Data.User.Entities;

namespace Alunos.Api.Infra.Data.Aluno
{
    public class AlunoContext(DbContextOptions<AlunoContext> options, EnvironmentKey environmentKey) : DbContext(options), IAlunoContext
    {
        private readonly EnvironmentKey _environmentKey = environmentKey;
        public DbSet<AlunoDto> Aluno { get; set; }
        public DbSet<UserDto> Users { get; set; }

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

            // User configuration
            modelBuilder.Entity<UserDto>(entity =>
            {
                entity.ToTable("usuario");
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.Id)
                    .HasColumnName("id");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(255)
                    .IsRequired();
                
                entity.HasIndex(e => e.Email).IsUnique();

                entity.Property(e => e.PasswordHash)
                    .HasColumnName("password_hash")
                    .HasMaxLength(255)
                    .IsRequired();

                entity.Property(e => e.IsActive)
                    .HasColumnName("ativo")
                    .IsRequired();

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("data_criacao")
                    .IsRequired();
                    
                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("data_atualizacao");
            });

            modelBuilder.Entity<AlunoDto>(entity =>
            {
                entity.ToTable("aluno");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnName("id");

                entity.Property(e => e.Nome)
                    .HasColumnName("nome")
                    .HasMaxLength(255)
                    .IsRequired();

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(255)
                    .IsRequired();

                entity.Property(e => e.Serie)
                    .HasColumnName("serie")
                    .HasMaxLength(100)
                    .IsRequired();
            });
        }
    }
}

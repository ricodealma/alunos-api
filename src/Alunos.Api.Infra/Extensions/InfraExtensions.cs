using Microsoft.Extensions.DependencyInjection;
using Alunos.Api.Domain.Aggregates.Aluno;
using Alunos.Api.Infra.Data.Aluno;
using Alunos.Api.Infra.Data.Aluno.Entities;
using Alunos.Api.Infra.Repositories;

namespace Alunos.Api.Infra.Extensions
{
    public static class InfraServicesExtensions
    {
        private static void AddDaos(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IAlunoDao, AlunoDao>();
            serviceCollection.AddScoped<Alunos.Api.Infra.Data.User.Entities.IUserDao, Alunos.Api.Infra.Data.User.Entities.UserDao>();
        }

        private static void AddRepositories(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IAlunoRepository, AlunoRepository>();
            serviceCollection.AddScoped<Alunos.Api.Domain.Aggregates.User.IUserRepository, UserRepository>();
        }

        private static void AddPersistence(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddDbContext<IAlunoContext, AlunoContext>();
        }

        public static void AddInfra(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddDaos();
            serviceCollection.AddRepositories();
            serviceCollection.AddPersistence();
        }
    }
}

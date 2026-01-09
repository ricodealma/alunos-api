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
        }

        private static void AddRepositories(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IAlunoRepository, AlunoRepository>();
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

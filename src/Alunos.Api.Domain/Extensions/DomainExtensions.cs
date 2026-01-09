using Alunos.Api.Domain.Aggregates.Aluno;
using Alunos.Api.Domain.SeedWork;
using Microsoft.Extensions.DependencyInjection;

namespace Alunos.Api.Domain.Extensions
{
    public static class DomainExtensions
    {
        public static void AddDomain(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<EnvironmentKey>();
            serviceCollection.AddScoped<IAlunoService, AlunoService>();
        }
    }
}

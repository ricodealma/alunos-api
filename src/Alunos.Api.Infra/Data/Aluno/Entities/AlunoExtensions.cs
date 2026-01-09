using Alunos.Api.Domain.Aggregates.Aluno.Entities;

namespace Alunos.Api.Infra.Data.Aluno.Entities
{
    public static class AlunoExtensions
    {
        public static AlunoDto ToDto(this AlunoModel aluno)
        {
            return new()
            {
                Id = aluno.Id,
                Nome = aluno.Nome,
                Email = aluno.Email,
                Serie = aluno.Serie
            };
        }

        public static AlunoModel ToDomain(this AlunoDto dto)
        {
            return new AlunoModel
            {
                Id = dto.Id,
                Nome = dto.Nome,
                Email = dto.Email,
                Serie = dto.Serie
            };
        }

        public static List<AlunoModel> ToDomain(this List<AlunoDto> alunoDtos) => alunoDtos.Select(ToDomain).ToList();
    }
}

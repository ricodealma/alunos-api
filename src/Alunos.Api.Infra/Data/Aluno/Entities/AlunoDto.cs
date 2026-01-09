using System.ComponentModel.DataAnnotations.Schema;

namespace Alunos.Api.Infra.Data.Aluno.Entities
{
    [Table("aluno")]
    public record AlunoDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Serie { get; set; } = string.Empty;
    }
}

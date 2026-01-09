namespace Alunos.Api.Domain.Aggregates.Aluno.Entities.Filter
{
    public sealed class FilterPaging
    {
        public int Page { get; set; } = 1;
        public int PerPage { get; set; } = 10;
    }
}

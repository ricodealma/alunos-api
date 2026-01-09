namespace Alunos.Api.Domain.Aggregates.Aluno.Entities.Filter;

public sealed class Filter
{
    public FilterPaging Paging { get; set; } = new();
    public Guid? Id { get; set; }
    public string? Nome { get; set; }
    public string? Email { get; set; }
    public string? Serie { get; set; }
}

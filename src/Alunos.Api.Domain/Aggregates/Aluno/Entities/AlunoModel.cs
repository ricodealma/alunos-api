namespace Alunos.Api.Domain.Aggregates.Aluno.Entities;

/// <summary>
/// Representa um aluno no sistema
/// </summary>
public sealed class AlunoModel
{
    /// <summary>
    /// Identificador único do aluno (UUID v7)
    /// </summary>
    /// <example>01933e1c-8e5e-7a4d-9a8c-3b2e1f0d9c8b</example>
    public Guid Id { get; set; }

    /// <summary>
    /// Nome completo do aluno
    /// </summary>
    /// <example>João Silva</example>
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Endereço de e-mail do aluno (único no sistema)
    /// </summary>
    /// <example>joao.silva@example.com</example>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Série/Ano escolar do aluno
    /// </summary>
    /// <example>5ª Série</example>
    public string Serie { get; set; } = string.Empty;
}

public static class AlunoModelExtensions
{
    public static AlunoModel ToModel(this AlunoCreateRequest request)
    {
        return new AlunoModel
        {
            Id = Guid.NewGuid(),
            Nome = request.Nome,
            Email = request.Email,
            Serie = request.Serie
        };
    }
}

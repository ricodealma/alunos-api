namespace Alunos.Api.Domain.Aggregates.Aluno.Entities;

/// <summary>
/// Dados necessários para criar um novo aluno
/// </summary>
public sealed class AlunoCreateRequest
{
    /// <summary>
    /// Nome completo do aluno (obrigatório)
    /// </summary>
    /// <example>João Silva</example>
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Endereço de e-mail do aluno - deve ser único (obrigatório)
    /// </summary>
    /// <example>joao.silva@example.com</example>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Série/Ano escolar do aluno (obrigatório)
    /// </summary>
    /// <example>5ª Série</example>
    public string Serie { get; set; } = string.Empty;
}

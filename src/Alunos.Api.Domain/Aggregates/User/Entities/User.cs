namespace Alunos.Api.Domain.Aggregates.User.Entities;

/// <summary>
/// Representa um usuário autenticado no sistema
/// </summary>
public sealed class User
{
    /// <summary>
    /// Identificador único do usuário (UUID)
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Endereço de e-mail do usuário (único no sistema)
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Hash BCrypt da senha do usuário
    /// </summary>
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>
    /// Data e hora de criação do usuário
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Data e hora da última atualização
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Indica se a conta do usuário está ativa
    /// </summary>
    public bool IsActive { get; set; } = true;
}

namespace Alunos.Api.Domain.Aggregates.User.DTOs;

/// <summary>
/// Request para autenticação de usuário
/// </summary>
public sealed class LoginRequest
{
    /// <summary>
    /// E-mail do usuário
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Senha do usuário (texto plano)
    /// </summary>
    public string Password { get; set; } = string.Empty;
}

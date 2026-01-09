namespace Alunos.Api.Domain.Aggregates.User.DTOs;

/// <summary>
/// Response da autenticação bem-sucedida
/// </summary>
public sealed class LoginResponse
{
    /// <summary>
    /// Token JWT para autenticação
    /// </summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// E-mail do usuário autenticado
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Data e hora de expiração do token
    /// </summary>
    public DateTime ExpiresAt { get; set; }
}

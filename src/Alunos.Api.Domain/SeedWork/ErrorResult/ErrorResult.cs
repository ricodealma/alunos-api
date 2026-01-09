using System.Text.Json.Serialization;

namespace Alunos.Api.Domain.SeedWork.ErrorResult
{
    /// <summary>
    /// Representa um erro retornado pela API
    /// </summary>
    public sealed class ErrorResult : IErrorResult
    {
        /// <summary>
        /// Indica se houve erro (não retornado no JSON)
        /// </summary>
        [JsonIgnore]
        public bool Error { get; set; }

        /// <summary>
        /// Identificador do recurso relacionado ao erro
        /// </summary>
        /// <example>01933e1c-8e5e-7a4d-9a8c-3b2e1f0d9c8b</example>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Tipo/Código HTTP do erro
        /// </summary>
        /// <example>NotFound</example>
        public string Type { get => StatusCode.ToString(); }

        /// <summary>
        /// Mensagem descritiva do erro
        /// </summary>
        /// <example>Aluno não encontrado para o ID fornecido</example>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Código HTTP do erro
        /// </summary>
        public ErrorCode StatusCode { get; set; }
    }
}

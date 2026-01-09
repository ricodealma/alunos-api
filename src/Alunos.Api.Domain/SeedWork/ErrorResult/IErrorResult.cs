using System.Text.Json.Serialization;

namespace Alunos.Api.Domain.SeedWork.ErrorResult
{
    public interface IErrorResult
    {
        [JsonIgnore]
        bool Error { get; set; }
        string Id { get; set; }
        string Message { get; set; }
        string Type { get => StatusCode.ToString(); }
        ErrorCode StatusCode { get; set; }
    }
}

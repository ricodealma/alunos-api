using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using Alunos.Api.Domain.Aggregates.Aluno;
using Alunos.Api.Domain.Aggregates.Aluno.Entities;
using Alunos.Api.Domain.Aggregates.Aluno.Entities.Filter;
using Alunos.Api.Domain.SeedWork.ErrorResult;

namespace Alunos.Api.App.Extensions
{
    public static class EndpointsExtensions
    {
        public static void AddEndpoints(this IEndpointRouteBuilder endpoints)
        {
            // Public endpoints
            endpoints.MapGet("/health", HealthCheck).AllowAnonymous();

            // Authentication endpoints
            endpoints.AddAuthEndpoints();

            // Protected CRUD endpoints
            endpoints.MapPost("/api/v1/alunos", CreateAluno).RequireAuthorization();
            endpoints.MapPut("/api/v1/alunos/{id}", UpdateAlunoById).RequireAuthorization();
            endpoints.MapDelete("/api/v1/alunos/{id}", DeleteAlunoById).RequireAuthorization();
            endpoints.MapGet("/api/v1/alunos", GetAlunosByFilter).RequireAuthorization();
        }

        [SwaggerOperation(
            Summary = "Health Check",
            Description = "Verifica se a aplicação está operando corretamente.",
            OperationId = "HealthCheck",
            Tags = ["Health"]
        )]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public static IResult HealthCheck() => Results.Ok("Healthy");

        [SwaggerOperation(
            Summary = "Cria um novo Aluno",
            Description = "Cria um novo registro de Aluno com os dados fornecidos. Campos obrigatórios: Nome, Email e Serie.",
            OperationId = "CreateAluno",
            Tags = ["Aluno"]
        )]
        [ProducesResponseType(typeof(AlunoModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        public static async Task<IResult> CreateAluno([FromBody] AlunoCreateRequest aluno, [FromServices] IAlunoService alunoService)
        {
            var (result, error) = await alunoService.InsertAlunoAsync(aluno);

            if (result is null || error.Error)
                return GenerateErrorResult(error);

            return Results.Created($"/api/v1/alunos/{result.Id}", result);
        }

        [SwaggerOperation(
            Summary = "Atualiza um Aluno",
            Description = "Atualiza os dados do Aluno identificado pelo ID. Todos os campos do modelo devem ser enviados.",
            OperationId = "UpdateAluno",
            Tags = ["Aluno"]
        )]
        [ProducesResponseType(typeof(AlunoModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        public static async Task<IResult> UpdateAlunoById([FromRoute] Guid id, [FromBody] AlunoModel aluno, [FromServices] IAlunoService alunoService)
        {
            var result = await alunoService.UpdateAlunoByIdAsync(id, aluno);
            if (result.Item1 is null || result.Item2.Error)
                return GenerateErrorResult(result.Item2);

            return Results.Created($"/api/v1/alunos/{result.Item1.Id}", result.Item1);
        }

        [SwaggerOperation(
            Summary = "Deleta um Aluno",
            Description = "Remove o Aluno identificado pelo ID. Retorna os dados do aluno que foi removido.",
            OperationId = "DeleteAluno",
            Tags = ["Aluno"]
        )]
        [ProducesResponseType(typeof(AlunoModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        public static async Task<IResult> DeleteAlunoById([FromRoute] Guid id, [FromServices] IAlunoService alunoService)
        {
            var result = await alunoService.DeleteAlunoByIdAsync(id);
            if (result.Item1 is null || result.Item2.Error)
                return GenerateErrorResult(result.Item2);

            return Results.Ok(result.Item1);
        }

        [SwaggerOperation(
            Summary = "Consulta Alunos com base em filtros",
            Description = "Retorna uma lista paginada de Alunos. Todos os filtros são opcionais. Suporta busca por ID, Nome (parcial), Email (parcial) e Serie (parcial).",
            OperationId = "GetAlunosByFilter",
            Tags = ["Aluno"]
        )]
        [ProducesResponseType(typeof(Domain.SeedWork.Paging.Search<AlunoModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        public static async Task<IResult> GetAlunosByFilter(
            [FromServices] IAlunoService alunoService,
            [FromQuery, SwaggerParameter("ID único do aluno (opcional)")] Guid? id,
            [FromQuery, SwaggerParameter("Nome do aluno - busca parcial (opcional)")] string? nome,
            [FromQuery, SwaggerParameter("Email do aluno - busca parcial (opcional)")] string? email,
            [FromQuery, SwaggerParameter("Serie do aluno - busca parcial (opcional)")] string? serie,
            [FromQuery, SwaggerParameter("Número da página (padrão: 1)")] int page = 1,
            [FromQuery, SwaggerParameter("Itens por página (padrão: 10)")] int size = 10)
        {
            var filter = new Filter()
            {
                Id = id,
                Nome = nome,
                Email = email,
                Serie = serie,
                Paging = new()
                {
                    Page = page,
                    PerPage = size
                }
            };
            var result = await alunoService.SelectAlunoByFilterAsync(filter);
            if (result.Item2 is not null && result.Item2.Error)
                return GenerateErrorResult(result.Item2);

            return Results.Ok(result.Item1);
        }

        private static IResult GenerateErrorResult(ErrorResult errorResult) => errorResult.StatusCode switch
        {
            ErrorCode.Undefined => Results.Problem(JsonConvert.SerializeObject(errorResult), statusCode: 500),
            ErrorCode.NotFound => Results.NotFound(errorResult),
            ErrorCode.BadRequest => Results.BadRequest(errorResult),
            ErrorCode.Unauthorized => Results.Unauthorized(),
            ErrorCode.Forbidden => Results.Forbid(),
            ErrorCode.InternalServerError => Results.Problem(JsonConvert.SerializeObject(errorResult), statusCode: 500),
            ErrorCode.UnprocessableEntity => Results.UnprocessableEntity(errorResult),
            _ => Results.Problem(JsonConvert.SerializeObject(errorResult), statusCode: 422)
        };
    }
}

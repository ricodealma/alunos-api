using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Alunos.Api.Domain.Aggregates.User.DTOs;
using Alunos.Api.Domain.SeedWork.ErrorResult;

namespace Alunos.Api.App.Extensions;

public static class AuthEndpoints
{
    public static void AddAuthEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/auth/login", Login)
            .AllowAnonymous()
            .WithTags("Authentication")
            .WithName("Login")
            .Produces<LoginResponse>(StatusCodes.Status200OK)
            .Produces<ErrorResult>(StatusCodes.Status401Unauthorized)
            .Produces<ErrorResult>(StatusCodes.Status403Forbidden);

        endpoints.MapPost("/api/auth/register", Register)
            .AllowAnonymous()
            .WithTags("Authentication")
            .WithName("Register")
            .Produces<RegisterResponse>(StatusCodes.Status201Created)
            .Produces<ErrorResult>(StatusCodes.Status400BadRequest)
            .Produces<ErrorResult>(StatusCodes.Status409Conflict);
    }

    [SwaggerOperation(
        Summary = "Autentica um usuário",
        Description = "Valida credenciais e retorna um token JWT para autenticação."
    )]
    private static async Task<IResult> Login(
        [FromBody] LoginRequest request,
        [FromServices] IConfiguration configuration,
        [FromServices] Alunos.Api.Domain.Aggregates.User.IUserService userService)
    {
        // Validation
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
        {
            return Results.BadRequest(new ErrorResult
            {
                Message = "Email and password are required",
                StatusCode = ErrorCode.BadRequest
            });
        }

        // Validate email format
        if (!IsValidEmail(request.Email))
        {
            return Results.BadRequest(new ErrorResult
            {
                Message = "Invalid email format",
                StatusCode = ErrorCode.BadRequest
            });
        }

        // JWT Settings (moved inside to pass to service, though ideally service should have config injected)
        // For now, passing config values to service method to keep service pure of IConfiguration if wanted,
        // or we could inject IConfiguration into Service. Service implementation above assumes args.
        // Let's use the args we created in IUserService signature.
        
        var jwtSettings = configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey not configured");
        var issuer = jwtSettings["Issuer"] ?? "alunos-api";
        var audience = jwtSettings["Audience"] ?? "alunos-web";
        var expirationHours = int.Parse(jwtSettings["ExpirationHours"] ?? "24");

        var (response, error) = await userService.AuthenticateAsync(
            request.Email, 
            request.Password,
            secretKey,
            issuer,
            audience,
            expirationHours);

        if (response is null || error.Error)
        {
            return error.StatusCode switch
            {
                ErrorCode.Unauthorized => Results.Json(error, statusCode: 401),
                ErrorCode.Forbidden => Results.Json(error, statusCode: 403),
                _ => Results.BadRequest(error)
            };
        }

        return Results.Ok(response);
    }



    [SwaggerOperation(
        Summary = "Registra um novo usuário",
        Description = "Cria uma nova conta de usuário."
    )]
    private static async Task<IResult> Register(
        [FromBody] RegisterRequest request,
        [FromServices] Alunos.Api.Domain.Aggregates.User.IUserService userService)
    {
        // Validation
        if (string.IsNullOrWhiteSpace(request.Name) || 
            string.IsNullOrWhiteSpace(request.Email) || 
            string.IsNullOrWhiteSpace(request.Password))
        {
            return Results.BadRequest(new ErrorResult
            {
                Message = "Name, email and password are required",
                StatusCode = ErrorCode.BadRequest
            });
        }

        if (!IsValidEmail(request.Email))
        {
            return Results.BadRequest(new ErrorResult
            {
                Message = "Invalid email format",
                StatusCode = ErrorCode.BadRequest
            });
        }

        var (response, error) = await userService.RegisterAsync(request);

        if (response is null || error.Error)
        {
            return error.StatusCode switch
            {
                ErrorCode.Conflict => Results.Conflict(error),
                _ => Results.BadRequest(error)
            };
        }

        return Results.Created($"/api/users/{response.Id}", response);
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}

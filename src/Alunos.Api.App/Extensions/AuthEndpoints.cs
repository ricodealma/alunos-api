using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Swashbuckle.AspNetCore.Annotations;
using Alunos.Api.Domain.Aggregates.User.DTOs;
using Alunos.Api.Domain.Aggregates.User.Entities;
using Alunos.Api.Domain.SeedWork.ErrorResult;
using Alunos.Api.Infra.Data.Aluno;

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
            .Produces<ErrorResult>(StatusCodes.Status400BadRequest)
            .Produces<ErrorResult>(StatusCodes.Status401Unauthorized)
            .Produces<ErrorResult>(StatusCodes.Status403Forbidden);
    }

    [SwaggerOperation(
        Summary = "Autentica um usuário",
        Description = "Valida credenciais e retorna um token JWT para autenticação."
    )]
    private static async Task<IResult> Login(
        [FromBody] LoginRequest request,
        [FromServices] IConfiguration configuration,
        [FromServices] IAlunoContext context)
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

        // Find user (case-insensitive email)
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Email.ToLower() == request.Email.ToLower());

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            return Results.Json(new ErrorResult
            {
                Message = "Invalid email or password",
                StatusCode = ErrorCode.Unauthorized
            }, statusCode: 401);
        }

        if (!user.IsActive)
        {
            return Results.Json(new ErrorResult
            {
                Message = "Account is inactive",
                StatusCode = ErrorCode.Forbidden
            }, statusCode: 403);
        }

        // Generate JWT token
        var jwtSettings = configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey not configured");
        var issuer = jwtSettings["Issuer"] ?? "alunos-api";
        var audience = jwtSettings["Audience"] ?? "alunos-web";
        var expirationHours = int.Parse(jwtSettings["ExpirationHours"] ?? "24");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiresAt = DateTime.UtcNow.AddHours(expirationHours);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return Results.Ok(new LoginResponse
        {
            Token = tokenString,
            Email = user.Email,
            ExpiresAt = expiresAt
        });
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

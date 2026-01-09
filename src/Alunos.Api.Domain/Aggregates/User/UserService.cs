using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Alunos.Api.Domain.Aggregates.User.DTOs;
using Alunos.Api.Domain.SeedWork.ErrorResult;
using Microsoft.IdentityModel.Tokens;

namespace Alunos.Api.Domain.Aggregates.User;

public sealed class UserService(IUserRepository userRepository) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<Tuple<LoginResponse?, ErrorResult>> AuthenticateAsync(string email, string password, string secretKey, string issuer, string audience, int expirationHours)
    {
        // Find user
        var user = await _userRepository.GetUserByEmailAsync(email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        {
            return new(null, new ErrorResult
            {
                Message = "Invalid email or password",
                StatusCode = ErrorCode.Unauthorized
            });
        }

        if (!user.IsActive)
        {
            return new(null, new ErrorResult
            {
                Message = "Account is inactive",
                StatusCode = ErrorCode.Forbidden
            });
        }

        // Generate JWT
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

        return new(new LoginResponse
        {
            Token = tokenString,
            Email = user.Email,
            ExpiresAt = expiresAt
        }, new());
    }

    public async Task<Tuple<RegisterResponse?, ErrorResult>> RegisterAsync(RegisterRequest request)
    {
        // Check if user already exists
        var existingUser = await _userRepository.GetUserByEmailAsync(request.Email);
        if (existingUser != null)
        {
            return new(null, new ErrorResult
            {
                Message = "Email already registered",
                StatusCode = ErrorCode.Conflict
            });
        }

        // Hash password
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        // Create Entity
        var newUser = new Entities.User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            PasswordHash = passwordHash,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        // Save
        await _userRepository.AddAsync(newUser);

        return new(new RegisterResponse(newUser.Id, request.Name, newUser.Email), new());
    }
}

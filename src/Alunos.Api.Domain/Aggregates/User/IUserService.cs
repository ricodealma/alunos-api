using Alunos.Api.Domain.Aggregates.User.DTOs;
using Alunos.Api.Domain.SeedWork.ErrorResult;

namespace Alunos.Api.Domain.Aggregates.User;

public interface IUserService
{
    Task<Tuple<LoginResponse?, ErrorResult>> AuthenticateAsync(string email, string password, string secretKey, string issuer, string audience, int expirationHours);
    Task<Tuple<RegisterResponse?, ErrorResult>> RegisterAsync(RegisterRequest request);
}

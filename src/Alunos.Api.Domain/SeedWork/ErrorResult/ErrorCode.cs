namespace Alunos.Api.Domain.SeedWork.ErrorResult
{
    public enum ErrorCode
    {
        Undefined = 0,
        NotFound = 404,
        BadRequest = 400,
        Unauthorized = 401,
        Forbidden = 403,
        Conflict = 409,
        UnprocessableEntity = 422,
        InternalServerError = 500
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Alunos.Api.Domain.SeedWork.Paging;
using Alunos.Api.Domain.SeedWork;
using Newtonsoft.Json;
using System.Linq.Expressions;
using Alunos.Api.Domain.Aggregates.Aluno.Entities.Filter;
using Alunos.Api.Domain.SeedWork.ErrorResult;

namespace Alunos.Api.Infra.Data.Aluno.Entities
{
    public class AlunoDao(ILogger<AlunoDao> logger, IAlunoContext alunoContext) : IAlunoDao
    {
        private readonly ILogger<AlunoDao> _logger = logger;
        private readonly IAlunoContext _alunoContext = alunoContext;

        public async Task<Tuple<Search<AlunoDto>?, ErrorResult>> SelectByFilterAsync(Filter filter)
        {
            try
            {
                Search<AlunoDto> search = new();
                int skip = Math.Abs(filter.Paging.PerPage * (filter.Paging.Page - 1));

                List<Expression<Func<AlunoDto, bool>>> filters = [];

                if (filter.Id != null)
                    filters.Add(x => x.Id == filter.Id);

                if (!string.IsNullOrEmpty(filter.Nome))
                    filters.Add(x => EF.Functions.Like(x.Nome, $"%{filter.Nome}%"));

                if (!string.IsNullOrEmpty(filter.Email))
                    filters.Add(x => EF.Functions.Like(x.Email, $"%{filter.Email}%"));

                if (!string.IsNullOrEmpty(filter.Serie))
                    filters.Add(x => EF.Functions.Like(x.Serie, $"%{filter.Serie}%"));

                var whereFilter = DynamicFilter.GenerateFilter(filters) ?? (x => true);

                var totalQuery = _alunoContext.Aluno.Where(whereFilter).AsNoTracking().Select(x => x.Id);
                search.Paging.CurrentPage = filter.Paging.Page;
                search.Paging.PerPage = filter.Paging.PerPage > default(int) ? filter.Paging.PerPage : 10;
                search.Paging.Total = await totalQuery.CountAsync();
                search.Paging.Pages = Convert.ToInt32(Math.Ceiling((double)search.Paging.Total / filter.Paging.PerPage));
                search.Paging.Pages = search.Paging.Total > default(int) && search.Paging.Pages == default ? 1 : search.Paging.Pages;

                IQueryable<AlunoDto?> query;

                var baseQuery = _alunoContext.Aluno
                                 .Where(whereFilter)
                                 .AsNoTracking();

                query = baseQuery
                    .OrderBy(x => x.Nome)
                    .ThenBy(x => x.Id);

                query = query.Skip(skip).Take(search.Paging.PerPage);
                search.Data = await query.ToListAsync();

                if (search.Data == null)
                    return Tuple.Create<Search<AlunoDto>?, ErrorResult>(null, new()
                    {
                        Error = true,
                        StatusCode = ErrorCode.NotFound,
                        Message = "No alunos found for the given filter."
                    });

                return Tuple.Create<Search<AlunoDto>?, ErrorResult>(search, new());
            }
            catch (Exception e)
            {
                _logger.LogError(JsonConvert.SerializeObject(e));
                return Tuple.Create<Search<AlunoDto>?, ErrorResult>(null, new()
                {
                    Error = true,
                    StatusCode = ErrorCode.InternalServerError,
                    Message = $"Failed to retrieve alunos with error: {JsonConvert.SerializeObject(e)}"
                });
            }
        }

        public async Task<Tuple<AlunoDto?, ErrorResult>> InsertAsync(AlunoDto aluno)
        {
            await using var transaction = await _alunoContext.Database.BeginTransactionAsync();
            try
            {
                var result = await _alunoContext.Aluno.AddAsync(aluno);
                await _alunoContext.SaveChangesAsync();

                if (result.Entity.Id == default)
                {
                    await transaction.RollbackAsync();
                    return new(null, new()
                    {
                        Error = true,
                        StatusCode = ErrorCode.InternalServerError,
                        Message = $"Unexpected Error While inserting aluno: {JsonConvert.SerializeObject(aluno)}"
                    });
                }

                await transaction.CommitAsync();
                return new(result.Entity, new());
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError($"Unexpected error: {ex.Message} - {JsonConvert.SerializeObject(aluno)}");
                return new(null, new()
                {
                    Error = true,
                    StatusCode = ErrorCode.InternalServerError,
                    Message = $"{JsonConvert.SerializeObject(aluno)}"
                });
            }
        }

        public async Task<Tuple<AlunoDto?, ErrorResult>> PutAlunoAsync(Guid id, AlunoDto aluno)
        {
            try
            {
                var currentAluno = await _alunoContext.Aluno.FindAsync(id);

                if (currentAluno == null)
                    return new(null, new()
                    {
                        Error = true,
                        Id = id.ToString(),
                        Message = "Couldn't find aluno for that id",
                        StatusCode = ErrorCode.NotFound
                    });

                currentAluno.Nome = aluno.Nome;
                currentAluno.Email = aluno.Email;
                currentAluno.Serie = aluno.Serie;

                await _alunoContext.SaveChangesAsync();

                return new(aluno, new());
            }
            catch (Exception e)
            {
                _logger.LogError(JsonConvert.SerializeObject(e));
                return new(null, new()
                {
                    Error = true,
                    StatusCode = ErrorCode.InternalServerError,
                    Message = $"Failed to update aluno with error: {JsonConvert.SerializeObject(e)}"
                });
            }
        }

        public async Task<Tuple<AlunoDto?, ErrorResult>> DeleteAlunoByIdAsync(Guid id)
        {
            try
            {
                var alunoQuery = _alunoContext.Aluno.Where(a => a.Id == id);

                var aluno = await alunoQuery.FirstOrDefaultAsync();
                var deletedRows = await alunoQuery.ExecuteDeleteAsync();

                if (deletedRows == 0)
                    return new(null, new()
                    {
                        Error = true,
                        Id = id.ToString(),
                        Message = "No rows were deleted",
                        StatusCode = ErrorCode.NotFound
                    });

                await _alunoContext.SaveChangesAsync();

                return new(aluno, new());
            }
            catch (Exception e)
            {
                _logger.LogError(JsonConvert.SerializeObject(e));
                return new(null, new()
                {
                    Error = true,
                    StatusCode = ErrorCode.InternalServerError,
                    Message = $"Failed to delete aluno with error: {JsonConvert.SerializeObject(e)}"
                });
            }
        }
    }
}

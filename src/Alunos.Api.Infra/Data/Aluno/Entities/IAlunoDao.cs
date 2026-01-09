using Alunos.Api.Domain.Aggregates.Aluno.Entities.Filter;
using Alunos.Api.Domain.SeedWork.ErrorResult;
using Alunos.Api.Domain.SeedWork.Paging;

namespace Alunos.Api.Infra.Data.Aluno.Entities
{
    public interface IAlunoDao
    {
        Task<Tuple<AlunoDto?, ErrorResult>> InsertAsync(AlunoDto alunoDto);
        Task<Tuple<Search<AlunoDto>?, ErrorResult>> SelectByFilterAsync(Filter filter);
        Task<Tuple<AlunoDto?, ErrorResult>> PutAlunoAsync(Guid alunoId, AlunoDto aluno);
        Task<Tuple<AlunoDto?, ErrorResult>> DeleteAlunoByIdAsync(Guid id);
    }
}

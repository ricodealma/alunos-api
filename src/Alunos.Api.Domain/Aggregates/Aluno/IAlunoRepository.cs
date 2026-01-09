using Alunos.Api.Domain.Aggregates.Aluno.Entities;
using Alunos.Api.Domain.Aggregates.Aluno.Entities.Filter;
using Alunos.Api.Domain.SeedWork.ErrorResult;
using Alunos.Api.Domain.SeedWork.Paging;

namespace Alunos.Api.Domain.Aggregates.Aluno
{
    public interface IAlunoRepository
    {
        Task<Tuple<AlunoModel?, ErrorResult>> DeleteAlunoByIdAsync(Guid id);
        Task<Tuple<AlunoModel?, ErrorResult>> InsertAlunoAsync(AlunoModel aluno);
        Task<Tuple<Search<AlunoModel>?, ErrorResult>> SelectAlunoByFilterAsync(Filter filter);
        Task<Tuple<AlunoModel?, ErrorResult>> UpdateAlunoAsync(Guid id, AlunoModel request);
    }
}

using Alunos.Api.Domain.Aggregates.Aluno.Entities;
using Alunos.Api.Domain.Aggregates.Aluno.Entities.Filter;
using Alunos.Api.Domain.SeedWork.ErrorResult;
using Alunos.Api.Domain.SeedWork.Paging;

namespace Alunos.Api.Domain.Aggregates.Aluno
{
    public interface IAlunoService
    {
        Task<Tuple<AlunoModel?, ErrorResult>> InsertAlunoAsync(AlunoCreateRequest aluno);
        Task<Tuple<AlunoModel?, ErrorResult>> UpdateAlunoByIdAsync(Guid id, AlunoModel aluno);
        Task<Tuple<Search<AlunoModel>?, ErrorResult>> SelectAlunoByFilterAsync(Filter filter);
        Task<Tuple<AlunoModel?, ErrorResult>> DeleteAlunoByIdAsync(Guid id);
    }
}

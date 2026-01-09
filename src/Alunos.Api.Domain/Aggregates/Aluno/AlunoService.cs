using Alunos.Api.Domain.SeedWork.ErrorResult;
using Alunos.Api.Domain.Aggregates.Aluno.Entities;
using Alunos.Api.Domain.Aggregates.Aluno.Entities.Filter;
using Alunos.Api.Domain.SeedWork.Paging;

namespace Alunos.Api.Domain.Aggregates.Aluno
{
    public sealed class AlunoService(IAlunoRepository alunoRepository) : IAlunoService
    {
        private readonly IAlunoRepository _alunoRepository = alunoRepository;

        public async Task<Tuple<AlunoModel?, ErrorResult>> InsertAlunoAsync(AlunoCreateRequest request)
            => await _alunoRepository.InsertAlunoAsync(request.ToModel());

        public async Task<Tuple<AlunoModel?, ErrorResult>> UpdateAlunoByIdAsync(Guid id, AlunoModel aluno)
            => await _alunoRepository.UpdateAlunoAsync(id, aluno);

        public async Task<Tuple<Search<AlunoModel>?, ErrorResult>> SelectAlunoByFilterAsync(Filter filter)
            => await _alunoRepository.SelectAlunoByFilterAsync(filter);

        public async Task<Tuple<AlunoModel?, ErrorResult>> DeleteAlunoByIdAsync(Guid id)
            => await _alunoRepository.DeleteAlunoByIdAsync(id);
    }
}

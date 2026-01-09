using Alunos.Api.Domain.SeedWork.ErrorResult;
using Alunos.Api.Domain.Aggregates.Aluno;
using Alunos.Api.Domain.Aggregates.Aluno.Entities;
using Alunos.Api.Infra.Data.Aluno.Entities;
using Alunos.Api.Domain.Aggregates.Aluno.Entities.Filter;
using Alunos.Api.Domain.SeedWork.Paging;

namespace Alunos.Api.Infra.Repositories
{
    public sealed class AlunoRepository(IAlunoDao alunoDao) : IAlunoRepository
    {
        private readonly IAlunoDao _alunoDao = alunoDao;

        public async Task<Tuple<AlunoModel?, ErrorResult>> DeleteAlunoByIdAsync(Guid id)
        {
            var (result, error) = await _alunoDao.DeleteAlunoByIdAsync(id);

            if (result is null)
                return new(null, error);

            return new(result.ToDomain(), new());
        }

        public async Task<Tuple<AlunoModel?, ErrorResult>> InsertAlunoAsync(AlunoModel aluno)
        {
            var (result, error) = await _alunoDao.InsertAsync(aluno.ToDto());

            if (result is null)
                return new(null, error);

            return new(result.ToDomain(), new());
        }

        public async Task<Tuple<Search<AlunoModel>?, ErrorResult>> SelectAlunoByFilterAsync(Filter filter)
        {
            var (searchAlunos, searchError) = await _alunoDao.SelectByFilterAsync(filter);

            if (searchAlunos is null)
                return new(null, searchError);

            Search<AlunoModel> searchAluno = new()
            {
                Paging = searchAlunos.Paging,
                Data = searchAlunos.Data.ToDomain()
            };

            return new(searchAluno, new());
        }

        public async Task<Tuple<AlunoModel?, ErrorResult>> UpdateAlunoAsync(Guid id, AlunoModel request)
        {
            var (updatedAluno, updateError) = await _alunoDao.PutAlunoAsync(id, request.ToDto());

            if (updatedAluno is null)
                return new(null, updateError);

            return new(updatedAluno.ToDomain(), new());
        }
    }
}

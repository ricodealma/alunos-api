using Alunos.Api.Domain.Aggregates.Aluno.Entities;
using Alunos.Api.Domain.Aggregates.Aluno.Entities.Filter;
using Alunos.Api.Domain.SeedWork.ErrorResult;
using Alunos.Api.Domain.SeedWork.Paging;
using Alunos.Api.Infra.Data.Aluno.Entities;
using Alunos.Api.Infra.Repositories;

namespace Alunos.Api.Infra.Test.Repositories;

public class AlunoRepositoryTests
{
    private readonly IAlunoDao _alunoDao;
    private readonly AlunoRepository _alunoRepository;
    private readonly Fixture _fixture;

    public AlunoRepositoryTests()
    {
        _fixture = new Fixture();
        _alunoDao = Substitute.For<IAlunoDao>();
        _alunoRepository = new AlunoRepository(_alunoDao);
    }

    [Fact]
    public async Task DeleteAlunoByIdAsync_ShouldReturnAluno_WhenDeletionIsSuccessful()
    {
        // Arrange
        var id = Guid.NewGuid();
        var alunoDto = _fixture.Create<AlunoDto>();
        _alunoDao.DeleteAlunoByIdAsync(id).Returns(new Tuple<AlunoDto?, ErrorResult>(alunoDto, new ErrorResult()));

        // Act
        var result = await _alunoRepository.DeleteAlunoByIdAsync(id);

        // Assert
        Assert.NotNull(result.Item1);
        Assert.Equal(alunoDto.Id, result.Item1.Id);
        Assert.Equal(alunoDto.Nome, result.Item1.Nome);
        Assert.Equal(alunoDto.Email, result.Item1.Email);
    }

    [Fact]
    public async Task DeleteAlunoByIdAsync_ShouldReturnError_WhenDeletionFails()
    {
        // Arrange
        var id = Guid.NewGuid();
        var errorResult = new ErrorResult { Message = "Not Found" };
        _alunoDao.DeleteAlunoByIdAsync(id).Returns(new Tuple<AlunoDto?, ErrorResult>(null, errorResult));

        // Act
        var result = await _alunoRepository.DeleteAlunoByIdAsync(id);

        // Assert
        Assert.Null(result.Item1);
        Assert.Equal(errorResult, result.Item2);
    }

    [Fact]
    public async Task InsertAlunoAsync_ShouldReturnAluno_WhenInsertionIsSuccessful()
    {
        // Arrange
        var alunoModel = _fixture.Create<AlunoModel>();
        var alunoDto = new AlunoDto { Id = alunoModel.Id, Nome = alunoModel.Nome, Email = alunoModel.Email, Serie = alunoModel.Serie ?? string.Empty };
        _alunoDao.InsertAsync(Arg.Any<AlunoDto>()).Returns(new Tuple<AlunoDto?, ErrorResult>(alunoDto, new ErrorResult()));

        // Act
        var result = await _alunoRepository.InsertAlunoAsync(alunoModel);

        // Assert
        Assert.NotNull(result.Item1);
        Assert.Equal(alunoModel.Id, result.Item1.Id);
    }

    [Fact]
    public async Task InsertAlunoAsync_ShouldReturnError_WhenInsertionFails()
    {
        // Arrange
        var alunoModel = _fixture.Create<AlunoModel>();
        var errorResult = new ErrorResult { Message = "Error" };
        _alunoDao.InsertAsync(Arg.Any<AlunoDto>()).Returns(new Tuple<AlunoDto?, ErrorResult>(null, errorResult));

        // Act
        var result = await _alunoRepository.InsertAlunoAsync(alunoModel);

        // Assert
        Assert.Null(result.Item1);
        Assert.Equal(errorResult, result.Item2);
    }

    [Fact]
    public async Task SelectAlunoByFilterAsync_ShouldReturnSearch_WhenSelectionIsSuccessful()
    {
        // Arrange
        var filter = new Filter();
        var alunoDtos = _fixture.CreateMany<AlunoDto>().ToList();
        var searchDto = new Search<AlunoDto>
        {
            Data = alunoDtos,
            Paging = new Paging { Total = 10 }
        };
        _alunoDao.SelectByFilterAsync(filter).Returns(new Tuple<Search<AlunoDto>?, ErrorResult>(searchDto, new ErrorResult()));

        // Act
        var result = await _alunoRepository.SelectAlunoByFilterAsync(filter);

        // Assert
        Assert.NotNull(result.Item1);
        Assert.Equal(searchDto.Paging.Total, result.Item1.Paging.Total);
        Assert.Equal(alunoDtos.Count, result.Item1.Data.Count());
    }

    [Fact]
    public async Task SelectAlunoByFilterAsync_ShouldReturnError_WhenSelectionFails()
    {
        // Arrange
        var filter = new Filter();
        var errorResult = new ErrorResult { Message = "Error" };
        _alunoDao.SelectByFilterAsync(filter).Returns(new Tuple<Search<AlunoDto>?, ErrorResult>(null, errorResult));

        // Act
        var result = await _alunoRepository.SelectAlunoByFilterAsync(filter);

        // Assert
        Assert.Null(result.Item1);
        Assert.Equal(errorResult, result.Item2);
    }

    [Fact]
    public async Task UpdateAlunoAsync_ShouldReturnAluno_WhenUpdateIsSuccessful()
    {
        // Arrange
        var id = Guid.NewGuid();
        var alunoModel = _fixture.Create<AlunoModel>();
        var alunoDto = new AlunoDto { Id = id, Nome = alunoModel.Nome, Email = alunoModel.Email, Serie = alunoModel.Serie ?? string.Empty };
        _alunoDao.PutAlunoAsync(Arg.Any<Guid>(), Arg.Any<AlunoDto>()).Returns(new Tuple<AlunoDto?, ErrorResult>(alunoDto, new ErrorResult()));

        // Act
        var result = await _alunoRepository.UpdateAlunoAsync(id, alunoModel);

        // Assert
        Assert.NotNull(result.Item1);
        Assert.Equal(id, result.Item1.Id);
    }

    [Fact]
    public async Task UpdateAlunoAsync_ShouldReturnError_WhenUpdateFails()
    {
        // Arrange
        var id = Guid.NewGuid();
        var alunoModel = _fixture.Create<AlunoModel>();
        var errorResult = new ErrorResult { Message = "Error" };
        _alunoDao.PutAlunoAsync(Arg.Any<Guid>(), Arg.Any<AlunoDto>()).Returns(new Tuple<AlunoDto?, ErrorResult>(null, errorResult));

        // Act
        var result = await _alunoRepository.UpdateAlunoAsync(id, alunoModel);

        // Assert
        Assert.Null(result.Item1);
        Assert.Equal(errorResult, result.Item2);
    }
}

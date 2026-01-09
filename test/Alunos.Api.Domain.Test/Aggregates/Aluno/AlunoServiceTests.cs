using Alunos.Api.Domain.Aggregates.Aluno;
using Alunos.Api.Domain.Aggregates.Aluno.Entities;
using Alunos.Api.Domain.Aggregates.Aluno.Entities.Filter;
using Alunos.Api.Domain.SeedWork.ErrorResult;
using Alunos.Api.Domain.SeedWork.Paging;

namespace Alunos.Api.Domain.Test.Aggregates.Aluno;

public class AlunoServiceTests
{
    private readonly IAlunoRepository _alunoRepository;
    private readonly AlunoService _alunoService;
    private readonly Fixture _fixture;

    public AlunoServiceTests()
    {
        _fixture = new Fixture();
        _fixture.Register<IPaging>(() => new Paging());
        _alunoRepository = Substitute.For<IAlunoRepository>();
        _alunoService = new AlunoService(_alunoRepository);
    }

    [Fact]
    public async Task InsertAlunoAsync_ShouldReturnAluno_WhenInsertionIsSuccessful()
    {
        // Arrange
        var request = _fixture.Create<AlunoCreateRequest>();
        var alunoModel = request.ToModel();
        _alunoRepository.InsertAlunoAsync(Arg.Any<AlunoModel>()).Returns(new Tuple<AlunoModel?, ErrorResult>(alunoModel, new ErrorResult()));

        // Act
        var result = await _alunoService.InsertAlunoAsync(request);

        // Assert
        Assert.NotNull(result.Item1);
        Assert.Equal(alunoModel.Nome, result.Item1.Nome);
        Assert.Equal(alunoModel.Email, result.Item1.Email);
    }

    [Fact]
    public async Task InsertAlunoAsync_ShouldReturnError_WhenInsertionFails()
    {
        // Arrange
        var request = _fixture.Create<AlunoCreateRequest>();
        var errorResult = new ErrorResult { Message = "Error" };
        _alunoRepository.InsertAlunoAsync(Arg.Any<AlunoModel>()).Returns(new Tuple<AlunoModel?, ErrorResult>(null, errorResult));

        // Act
        var result = await _alunoService.InsertAlunoAsync(request);

        // Assert
        Assert.Null(result.Item1);
        Assert.Equal(errorResult, result.Item2);
    }

    [Fact]
    public async Task UpdateAlunoByIdAsync_ShouldReturnAluno_WhenUpdateIsSuccessful()
    {
        // Arrange
        var id = Guid.NewGuid();
        var alunoModel = _fixture.Create<AlunoModel>();
        _alunoRepository.UpdateAlunoAsync(id, alunoModel).Returns(new Tuple<AlunoModel?, ErrorResult>(alunoModel, new ErrorResult()));

        // Act
        var result = await _alunoService.UpdateAlunoByIdAsync(id, alunoModel);

        // Assert
        Assert.NotNull(result.Item1);
        Assert.Equal(alunoModel.Id, result.Item1.Id);
    }

    [Fact]
    public async Task UpdateAlunoByIdAsync_ShouldReturnError_WhenUpdateFails()
    {
        // Arrange
        var id = Guid.NewGuid();
        var alunoModel = _fixture.Create<AlunoModel>();
        var errorResult = new ErrorResult { Message = "Error" };
        _alunoRepository.UpdateAlunoAsync(id, alunoModel).Returns(new Tuple<AlunoModel?, ErrorResult>(null, errorResult));

        // Act
        var result = await _alunoService.UpdateAlunoByIdAsync(id, alunoModel);

        // Assert
        Assert.Null(result.Item1);
        Assert.Equal(errorResult, result.Item2);
    }

    [Fact]
    public async Task SelectAlunoByFilterAsync_ShouldReturnSearch_WhenSelectionIsSuccessful()
    {
        // Arrange
        var filter = new Filter();
        var searchResult = _fixture.Create<Search<AlunoModel>>();
        _alunoRepository.SelectAlunoByFilterAsync(filter).Returns(new Tuple<Search<AlunoModel>?, ErrorResult>(searchResult, new ErrorResult()));

        // Act
        var result = await _alunoService.SelectAlunoByFilterAsync(filter);

        // Assert
        Assert.NotNull(result.Item1);
        Assert.Equal(searchResult.Paging.Total, result.Item1.Paging.Total);
    }

    [Fact]
    public async Task DeleteAlunoByIdAsync_ShouldReturnAluno_WhenDeletionIsSuccessful()
    {
        // Arrange
        var id = Guid.NewGuid();
        var alunoModel = _fixture.Create<AlunoModel>();
        _alunoRepository.DeleteAlunoByIdAsync(id).Returns(new Tuple<AlunoModel?, ErrorResult>(alunoModel, new ErrorResult()));

        // Act
        var result = await _alunoService.DeleteAlunoByIdAsync(id);

        // Assert
        Assert.NotNull(result.Item1);
        Assert.Equal(alunoModel.Id, result.Item1.Id);
    }
}

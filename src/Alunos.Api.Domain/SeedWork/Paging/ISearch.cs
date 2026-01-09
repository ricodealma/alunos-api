namespace Alunos.Api.Domain.SeedWork.Paging
{
    public interface ISearch<T>
    {
        IPaging Paging { get; set; }
        List<T> Data { get; set; }
    }
}

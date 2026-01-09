namespace Alunos.Api.Domain.SeedWork.Paging
{
    /// <summary>
    /// Resultado de busca paginada
    /// </summary>
    /// <typeparam name="T">Tipo dos itens retornados</typeparam>
    public sealed class Search<T>() : ISearch<T>
    {
        /// <summary>
        /// Informações de paginação (página atual, total de páginas, etc.)
        /// </summary>
        public IPaging Paging { get; set; } = new Paging();

        /// <summary>
        /// Lista de itens da página atual
        /// </summary>
        public List<T> Data { get; set; } = [];
    }
}

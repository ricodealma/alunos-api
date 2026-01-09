namespace Alunos.Api.Domain.SeedWork.Paging
{
    /// <summary>
    /// Informações de paginação de uma busca
    /// </summary>
    public sealed class Paging : IPaging
    {
        /// <summary>
        /// Total de itens encontrados (todas as páginas)
        /// </summary>
        /// <example>150</example>
        public int Total { get; set; }

        /// <summary>
        /// Página atual
        /// </summary>
        /// <example>1</example>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Quantidade de itens por página
        /// </summary>
        /// <example>10</example>
        public int PerPage { get; set; }

        /// <summary>
        /// Total de páginas disponíveis
        /// </summary>
        /// <example>15</example>
        public int Pages { get; set; }
    }
}

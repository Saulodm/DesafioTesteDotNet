namespace Core.Reports
{
    public interface ICsvExporter
    {
        /// <summary>
        /// Exporta uma sequência de objetos do tipo T para CSV usando apenas as propriedades especificadas.
        /// </summary>
        /// <typeparam name="T">Tipo dos itens</typeparam>
        /// <param name="items">Coleção de itens a exportar</param>
        /// <param name="propertyNames">Nomes das propriedades (ex.: "Name","Price")</param>
        /// <param name="writer">TextWriter alvo (arquivo, memória, stream)</param>
        Task ExportToCsvAsync<T>(IEnumerable<T> items, IEnumerable<string> propertyNames, TextWriter writer);
    }
}

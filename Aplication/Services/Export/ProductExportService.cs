using Core.Interfaces;
using Core.Models;
using Core.Reports;
using Microsoft.Extensions.Logging;

namespace Aplication.Services.Export
{
    /// <summary>
    /// Serviço de alto nível que orquestra exportação de produtos para CSV.
    /// </summary>
    public class ProductExportService: IProductExportService
    {
        private readonly ICsvExporter _csvExporter;
        private readonly IProductService _productService;
        private readonly ILogger<ProductExportService> _logger;

        public ProductExportService(ICsvExporter csvExporter, IProductService productService, ILogger<ProductExportService> logger)
        {
            _csvExporter = csvExporter;
            _productService = productService;
            _logger = logger;
        }

        /// <summary>
        /// Exporta produtos (todos) para o caminho informado usando apenas as propriedades selecionadas.
        /// </summary>
        public async Task ExportProductsAsync(IEnumerable<string> propertyNames, string filePath)
        {
            if (propertyNames == null) throw new ArgumentNullException(nameof(propertyNames));
            var products = await _productService.GetAll(); // supondo que retorna IEnumerable<Product>

            _logger.LogInformation("Iniciando export de {Count} produtos para {Path}", products is ICollection<Product> coll ? coll.Count : -1, filePath);

            using var fs = File.Open(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
            using var sw = new StreamWriter(fs, System.Text.Encoding.UTF8);
            await _csvExporter.ExportToCsvAsync<Product>(products, propertyNames, sw).ConfigureAwait(false);

            _logger.LogInformation("Export concluído: {Path}", filePath);
        }
    }
}

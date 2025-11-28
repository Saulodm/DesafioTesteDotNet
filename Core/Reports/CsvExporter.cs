using System.Reflection;
using Microsoft.Extensions.Logging;

namespace Core.Reports
{
    public class CsvExporter : ICsvExporter
    {
        private readonly ILogger<CsvExporter> _logger;

        public CsvExporter(ILogger<CsvExporter> logger)
        {
            _logger = logger;
        }

        public async Task ExportToCsvAsync<T>(IEnumerable<T> items, IEnumerable<string> propertyNames, TextWriter writer)
        {
            if (propertyNames == null) throw new ArgumentNullException(nameof(propertyNames));
            if (writer == null) throw new ArgumentNullException(nameof(writer));

            var propsRequested = propertyNames.ToList();
            var type = typeof(T);

            // Resolve PropertyInfos (case-insensitive) and keep order
            var propMap = new List<PropertyInfo?>();
            foreach (var name in propsRequested)
            {
                var pi = type.GetProperty(name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                propMap.Add(pi);
                if (pi == null)
                {
                    _logger?.LogWarning("Propriedade '{PropName}' não encontrada em tipo {TypeName}. Será exportada coluna vazia.", name, type.Name);
                }
            }

            // Escreve cabeçalho
            await writer.WriteLineAsync(string.Join(",", propsRequested.Select(EscapeCsv))).ConfigureAwait(false);

            // Escreve linhas
            foreach (var item in items)
            {
                var values = new List<string>(propMap.Count);
                for (int i = 0; i < propMap.Count; i++)
                {
                    var pi = propMap[i];
                    if (pi == null)
                    {
                        values.Add(string.Empty);
                        continue;
                    }

                    try
                    {
                        var raw = pi.GetValue(item);
                        var str = raw switch
                        {
                            null => string.Empty,
                            DateTime dt => dt.ToString("o"), // ISO format
                            IFormattable f => f.ToString(null, System.Globalization.CultureInfo.InvariantCulture),
                            _ => raw.ToString()
                        };
                        values.Add(EscapeCsv(str ?? string.Empty));
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError(ex, "Erro ao ler propriedade '{Prop}' do tipo {TypeName}", pi.Name, type.Name);
                        values.Add(string.Empty);
                    }
                }

                await writer.WriteLineAsync(string.Join(",", values)).ConfigureAwait(false);
            }

            await writer.FlushAsync().ConfigureAwait(false);
        }

        private static string EscapeCsv(string input)
        {
            if (input == null) return string.Empty;
            bool mustQuote = input.Contains(',') || input.Contains('"') || input.Contains('\n') || input.Contains('\r');
            if (mustQuote)
            {
                var escaped = input.Replace("\"", "\"\"");
                return $"\"{escaped}\"";
            }
            return input;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IProductExportService
    {
        Task ExportProductsAsync(IEnumerable<string> propertyNames, string filePath);
    }
}

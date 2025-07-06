using GlobalAnalytics.Data.Exporters;
using GlobalAnalytics.Lib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalAnalytics.Data.Factories
{
    public class ExporterFactory : IExporterFactory
    {
        public IExporter GetExporter(string format)
        {
            return format.ToLower() switch
            {
                "csv" => new CsvExporter(),
                "json" => new JsonExporter(),
                "pdf" => new PdfExporter(),
                _ => throw new NotSupportedException("Invalid export format")
            };
        }
    }
}

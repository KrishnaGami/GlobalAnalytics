using GlobalAnalytics.Lib.DTOs;
using GlobalAnalytics.Lib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalAnalytics.Data.Services
{
    public class ExportService : IExportService
    {
        private readonly IExporterFactory _factory;

        public ExportService(IExporterFactory factory)
        {
            _factory = factory;
        }

        public byte[] Export(string format, List<ClientDto> data)
        {
            var exporter = _factory.GetExporter(format);
            return exporter.Export(data);
        }
    }
}

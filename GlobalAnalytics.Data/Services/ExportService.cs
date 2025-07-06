using GlobalAnalytics.Lib.DTOs;
using GlobalAnalytics.Lib.Interfaces;
using log4net;

namespace GlobalAnalytics.Data.Services
{
    /// <summary>
    /// Coordinates export logic using the appropriate exporter resolved via factory.
    /// Supports formats: CSV, JSON, PDF via IExporterFactory.
    /// </summary>
    public class ExportService : IExportService
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(ExportService));
        private readonly IExporterFactory _factory;

        public ExportService(IExporterFactory factory)
        {
            _factory = factory;
        }

        /// <summary>
        /// Delegates export operation to a format-specific exporter via factory.
        /// </summary>
        /// <param name="format">Export format (e.g., csv, json, pdf)</param>
        /// <param name="data">Client data to export</param>
        /// <returns>Exported byte array</returns>
        public byte[] Export(string format, List<ClientDto> data)
        {
            _logger.Info($"Export requested. Format: {format?.ToUpper()}, Record count: {data?.Count ?? 0}");

            if (string.IsNullOrWhiteSpace(format))
            {
                _logger.Warn("Export failed: format is null or empty.");
                return Array.Empty<byte>();
            }

            if (data == null || !data.Any())
            {
                _logger.Warn("Export failed: No data available to export.");
                return Array.Empty<byte>();
            }

            try
            {
                var exporter = _factory.GetExporter(format.ToLower());
                _logger.Info($"Using {exporter.GetType().Name} for exporting.");
                return exporter.Export(data);
            }
            catch (Exception ex)
            {
                _logger.Error($"Export failed for format: {format}", ex);
                return Array.Empty<byte>();
            }
        }
    }
}

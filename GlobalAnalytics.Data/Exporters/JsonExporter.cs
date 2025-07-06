using GlobalAnalytics.Lib.DTOs;
using GlobalAnalytics.Lib.Interfaces;
using log4net;
using System.Text.Json;

namespace GlobalAnalytics.Data.Exporters
{
    public class JsonExporter : IExporter
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(JsonExporter));

        public byte[] Export(List<ClientDto> data)
        {
            _logger.Info($"Generating JSON export. Rows: {data.Count}");

            try
            {
                var json = JsonSerializer.Serialize(data, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                _logger.Info("JSON export generation completed.");
                return System.Text.Encoding.UTF8.GetBytes(json);
            }
            catch (Exception ex)
            {
                _logger.Error("Error during JSON export.", ex);
                return Array.Empty<byte>();
            }
        }
    }
}

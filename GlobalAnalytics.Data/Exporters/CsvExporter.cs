using GlobalAnalytics.Lib.DTOs;
using GlobalAnalytics.Lib.Interfaces;
using log4net;
using System.Text;

namespace GlobalAnalytics.Data.Exporters
{
    public class CsvExporter : IExporter
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(CsvExporter));

        public byte[] Export(List<ClientDto> data)
        {
            _logger.Info($"Generating CSV export. Rows: {data.Count}");

            try
            {
                var sb = new StringBuilder();
            sb.AppendLine("Name,Email,Phone,Country,City,PostalCode,Revenue,IsActive,Industry,ClientSince,CreatedDate,UpdatedDate");

            foreach (var c in data)
                {
                sb.AppendLine($"{c.Name},{c.Email},{c.Phone},{c.Country},{c.City},{c.PostalCode},{c.Revenue},{c.IsActive},{c.Industry},{c.ClientSince:yyyy-MM-dd},{c.CreatedDate:yyyy-MM-dd},{c.UpdatedDate:yyyy-MM-dd}");
                }

                _logger.Info("CSV export generation completed.");
                return Encoding.UTF8.GetBytes(sb.ToString());
            }
            catch (Exception ex)
            {
                _logger.Error("Error during CSV export.", ex);
                return Array.Empty<byte>();
            }
        }
    }
}
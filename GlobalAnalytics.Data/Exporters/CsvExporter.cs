using GlobalAnalytics.Lib.DTOs;
using GlobalAnalytics.Lib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalAnalytics.Data.Exporters
{
    public class CsvExporter : IExporter
    {
        public byte[] Export(List<ClientDto> data)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Name,Email,Phone,Country,City,PostalCode,Revenue,IsActive,Industry,ClientSince,CreatedDate,UpdatedDate");

            foreach (var c in data)
            {
                sb.AppendLine($"{c.Name},{c.Email},{c.Phone},{c.Country},{c.City},{c.PostalCode},{c.Revenue},{c.IsActive},{c.Industry},{c.ClientSince:yyyy-MM-dd},{c.CreatedDate:yyyy-MM-dd},{c.UpdatedDate:yyyy-MM-dd}");
            }

            return Encoding.UTF8.GetBytes(sb.ToString());
        }
    }
}
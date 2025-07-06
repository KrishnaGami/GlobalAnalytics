using GlobalAnalytics.Lib.DTOs;
using GlobalAnalytics.Lib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GlobalAnalytics.Data.Exporters
{
    public class JsonExporter : IExporter
    {
        public byte[] Export(List<ClientDto> data)
        {
            return JsonSerializer.SerializeToUtf8Bytes(data);
        }
    }
}

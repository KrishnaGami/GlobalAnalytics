using GlobalAnalytics.Lib.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalAnalytics.Lib.Interfaces
{
    public interface IExportService
    {
        byte[] Export(string format, List<ClientDto> data);
    }
}

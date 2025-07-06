using GlobalAnalytics.Lib.DTOs;
using GlobalAnalytics.Lib.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GlobalAnalytics.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;
        private readonly IExportService _exportService;

        public ClientController(IClientService clientService, IExportService exportService)
        {
            _clientService = clientService;
            _exportService = exportService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetClients([FromQuery] ClientFilterDto filter)
        {
            var result = await _clientService.GetClientsAsync(filter);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("export")]
        public async Task<IActionResult> Export([FromQuery] string format, [FromQuery] ClientFilterDto filter)
        {
            var data = await _clientService.GetClientsAsync(filter);
            var bytes = _exportService.Export(format, data.Data);
            var fileName = $"clients_{DateTime.Now:yyyyMMdd}.{format}";
            var contentType = format switch
            {
                "csv" => "text/csv",
                "json" => "application/json",
                "pdf" => "application/pdf",
                _ => "application/octet-stream"
            };
            return File(bytes, contentType, fileName);
        }

    }
}


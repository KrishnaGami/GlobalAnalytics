using GlobalAnalytics.Lib.DTOs;
using GlobalAnalytics.Lib.Interfaces;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GlobalAnalytics.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(ClientController));
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
            try
            {
                _logger.Info("Fetching clients...");
                var result = await _clientService.GetClientsAsync(filter);
                _logger.Info($"Fetched {result.Data.Count} clients.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.Error("Error occurred in GetClients", ex);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [Authorize]
        [HttpGet("export")]
        public async Task<IActionResult> Export([FromQuery] string format, [FromQuery] ClientFilterDto filter)
        {
            try
            {
                _logger.Info($"Export request received. Format: {format}, Filter: Country={filter.Country}, Industry={filter.Industry}");

                var result = await _clientService.GetClientsAsync(filter);

                if (result == null || result.Data == null || !result.Data.Any())
                {
                    _logger.Warn("Export aborted: No client data found for the given filter.");
                    return NotFound("No data found to export.");
                }

                var bytes = _exportService.Export(format.ToLower(), result.Data);

                if (bytes == null || bytes.Length == 0)
                {
                    _logger.Warn($"Export failed: Unsupported or empty export for format '{format}'.");
                    return BadRequest("Unsupported or invalid export format.");
                }

                var contentType = format.ToLower() switch
                {
                    "csv" => "text/csv",
                    "json" => "application/json",
                    "pdf" => "application/pdf",
                    _ => "application/octet-stream"
                };

                var fileName = $"clients_{DateTime.Now:yyyyMMdd_HHmmss}.{format.ToLower()}";
                _logger.Info($"Export successful. {result.Data.Count} records exported as {format.ToUpper()}.");
                return File(bytes, contentType, fileName);
            }
            catch (Exception ex)
            {
                _logger.Error("Unhandled exception occurred during export.", ex);
                return StatusCode(500, "An error occurred while exporting client data.");
            }
        }
    }
}


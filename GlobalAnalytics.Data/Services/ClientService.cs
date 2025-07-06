using AutoMapper;
using GlobalAnalytics.Core.Filters;
using GlobalAnalytics.Core.Models;
using GlobalAnalytics.Lib.DTOs;
using GlobalAnalytics.Lib.Interfaces;
using log4net;

namespace GlobalAnalytics.Data.Services
{
    /// <summary>
    /// Handles business logic related to Client data, including filtering, paging, and mapping.
    /// </summary>
    public class ClientService : IClientService
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(ClientService));
        private readonly IClientRepository _clientRepo;
        private readonly IMapper _mapper;

        public ClientService(IClientRepository clientRepo, IMapper mapper)
        {
            _clientRepo = clientRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves filtered and paginated client data.
        /// </summary>
        public async Task<PagedResult<ClientDto>> GetClientsAsync(ClientFilterDto filterDto)
        {
            try
            {
                _logger.Info("Fetching client list with filter.");

                var filter = new ClientFilter
                {
                    Country = filterDto.Country,
                    Industry = filterDto.Industry,
                    PageNumber = filterDto.PageNumber,
                    PageSize = filterDto.PageSize,
                    SortBy = filterDto.SortBy,
                    SortDirection = filterDto.SortDirection
                };

                var result = await _clientRepo.GetClientsAsync(filter);

                _logger.Info($"Fetched {result.Data.Count} clients out of {result.TotalCount} total.");

                var dtoList = _mapper.Map<List<ClientDto>>(result.Data);

                return new PagedResult<ClientDto>
                {
                    Data = dtoList,
                    TotalCount = result.TotalCount
                };
            }
            catch (Exception ex)
            {
                _logger.Error("Error occurred while fetching clients.", ex);
                throw;
            }
        }
    }
}
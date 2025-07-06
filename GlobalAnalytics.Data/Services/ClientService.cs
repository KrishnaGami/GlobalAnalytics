using AutoMapper;
using GlobalAnalytics.Core.Filters;
using GlobalAnalytics.Core.Models;
using GlobalAnalytics.Lib.DTOs;
using GlobalAnalytics.Lib.Interfaces;
namespace GlobalAnalytics.Data.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepo;
        private readonly IMapper _mapper;

        public ClientService(IClientRepository clientRepo, IMapper mapper)
        {
            _clientRepo = clientRepo;
            _mapper = mapper;
        }

        public async Task<PagedResult<ClientDto>> GetClientsAsync(ClientFilterDto filterDto)
        {
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

            var dtoList = _mapper.Map<List<ClientDto>>(result.Data);

            return new PagedResult<ClientDto>
            {
                Data = dtoList,
                TotalCount = result.TotalCount
            };
        }
    }
}

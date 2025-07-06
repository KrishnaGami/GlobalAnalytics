using GlobalAnalytics.Core.Models;
using GlobalAnalytics.Lib.DTOs;

namespace GlobalAnalytics.Lib.Interfaces
{
    public interface IClientService
    {
        Task<PagedResult<ClientDto>> GetClientsAsync(ClientFilterDto filter);
    }
}

using GlobalAnalytics.Core.Entities;
using GlobalAnalytics.Core.Filters;
using GlobalAnalytics.Core.Models;

namespace GlobalAnalytics.Lib.Interfaces
{
    public interface IClientRepository
    {
        Task<PagedResult<Client>> GetClientsAsync(ClientFilter filter);
    }
}

using GlobalAnalytics.Core.Entities;
using GlobalAnalytics.Core.Filters;
using GlobalAnalytics.Core.Models;
using GlobalAnalytics.Data.Services;
using GlobalAnalytics.Lib.Interfaces;
using log4net;
using Microsoft.EntityFrameworkCore;

namespace GlobalAnalytics.Data.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly AppDbContext _context;
        private readonly ILog _logger = LogManager.GetLogger(typeof(ClientService));
        public ClientRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Client>> GetClientsAsync(ClientFilter filter)
        {
            var query = _context.Clients.AsQueryable();

            if (!string.IsNullOrEmpty(filter.Country))
                query = query.Where(c => c.Country == filter.Country);

            if (!string.IsNullOrEmpty(filter.Industry))
                query = query.Where(c => c.Industry == filter.Industry);

            if (!string.IsNullOrEmpty(filter.SortBy))
            {
                bool isDescending = filter.SortDirection?.ToLower() == "desc";
                query = isDescending
                    ? query.OrderByDescending(e => EF.Property<object>(e, filter.SortBy))
                    : query.OrderBy(e => EF.Property<object>(e, filter.SortBy));
            }

            int totalCount = await query.CountAsync();

            var data = await query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            return new PagedResult<Client>
            {
                Data = data,
                TotalCount = totalCount
            };
        }
    }

}
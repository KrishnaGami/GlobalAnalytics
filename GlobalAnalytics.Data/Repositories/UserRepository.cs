using GlobalAnalytics.Core.Entities;
using GlobalAnalytics.Lib.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalAnalytics.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task SaveRefreshToken(int userId, string token)
        {
            var refreshToken = new RefreshToken
            {
                Token = token,
                Expires = DateTime.UtcNow.AddDays(7),
                IsRevoked = false,
                IsUsed = false,
                UserId = userId
            };
            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetUserByRefreshToken(string refreshToken)
        {
            var token = await _context.RefreshTokens.Include(r => r.User)
                .FirstOrDefaultAsync(t => t.Token == refreshToken && !t.IsUsed && !t.IsRevoked && t.Expires > DateTime.UtcNow);
            return token?.User;
        }

        public async Task InvalidateRefreshToken(string token)
        {
            var existingToken = await _context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == token);
            if (existingToken != null)
            {
                existingToken.IsUsed = true;
                existingToken.IsRevoked = true;
                await _context.SaveChangesAsync();
            }
        }
    }
}

using GlobalAnalytics.Core.Entities;
using GlobalAnalytics.Lib.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalAnalytics.Lib.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetByUsernameAsync(string username);
        Task<User> GetUserByRefreshToken(string refreshToken);
        Task SaveRefreshToken(int userId, string refreshToken);
        Task InvalidateRefreshToken(string refreshToken);
    }
}

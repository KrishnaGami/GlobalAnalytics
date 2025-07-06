using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalAnalytics.Lib.Interfaces
{
    public interface IDependencyModule
    {
        void Register(IServiceCollection services, IConfiguration configuration);
    }
}

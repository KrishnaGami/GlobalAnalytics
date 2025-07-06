using AutoMapper;
using GlobalAnalytics.Core.Entities;
using GlobalAnalytics.Lib.DTOs;

namespace GlobalAnalytics.API.Mapping
{
    public class ClientMappingProfile: Profile
    {
        public ClientMappingProfile()
        {
            CreateMap<Client, ClientDto>();
        }
    }
}

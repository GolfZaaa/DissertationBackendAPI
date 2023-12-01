using AutoMapper;
using BackendAPI.DTOs.RoomsDto;
using BackendAPI.Models;

namespace BackendAPI.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Location, LocationRequest>();
            CreateMap<LocationRequest, Location>();

            CreateMap<Location, LocationResponse>();
            CreateMap<LocationResponse, Location>();
        }
    }
}

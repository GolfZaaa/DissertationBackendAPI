using AutoMapper;
using BackendAPI.DTOs.RoomsDto;
using BackendAPI.Models;

namespace BackendAPI.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Location, CreateLocationDto>();
            CreateMap<CreateLocationDto, Location>();

            CreateMap<Location, LocationResponse>();
            CreateMap<LocationResponse, Location>();
        }
    }
}

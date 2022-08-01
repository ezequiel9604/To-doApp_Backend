
using AutoMapper;
using Domain.DTOs;
using Domain.Entities;

namespace Infrastructure.Mapper;

public class AutoMapperProfileConfiguration : Profile
{

    public AutoMapperProfileConfiguration()
    {
        // createMap<destination, source>
        CreateMap<UserDTO, User>().ReverseMap();

        // createMap<source, destination>
        CreateMap<UserDTO, User>()
            .ForMember(dest => dest.PasswordSalt, opt => opt.Ignore())
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

        // createMap<destination, source>
        CreateMap<UTaskDTO, UTask>().ReverseMap()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User.Id))
            .ForMember(dest => dest.Hour, opt => opt.MapFrom(src => src.Date.Value.Hour))
            .ForMember(dest => dest.Minute, opt => opt.MapFrom(src => src.Date.Value.Minute))
            .ForMember(dest => dest.Day, opt => opt.MapFrom(src => src.Date.Value.Day))
            .ForMember(dest => dest.Month, opt => opt.MapFrom(src => src.Date.Value.Month))
            .ForMember(dest => dest.Year, opt => opt.MapFrom(src => src.Date.Value.Year))
            .ForMember(dest => dest.Frequency, opt => opt.MapFrom(src => src.Frequency.Name))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name));

        // createMap<source, destination>
        CreateMap<UTaskDTO, UTask>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Date, 
                opt => opt.MapFrom(src => new DateTime(src.Year, src.Month, src.Day, src.Hour, src.Minute, 0)));

        // createMap<destination, source>
        CreateMap<ChangeOfPasswordDTO, ChangeOfPassword>().ReverseMap();

    }

}


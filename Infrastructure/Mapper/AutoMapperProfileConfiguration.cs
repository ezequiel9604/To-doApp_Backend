
using AutoMapper;
using Domain.DTOs;
using Domain.Entities;

namespace Infrastructure.Mapper;

public class AutoMapperProfileConfiguration : Profile
{

    public AutoMapperProfileConfiguration()
    {
        // createMpa<destination, source>
        CreateMap<UserDTO, User>().ReverseMap();

        // createMpa<source, destination>
        CreateMap<UserDTO, User>()
            .ForMember(dest => dest.PasswordSalt, opt => opt.Ignore())
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

        // createMpa<destination, source>
        CreateMap<UTaskDTO, UTask>().ReverseMap()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User.Id))
            .ForMember(dest => dest.Hour, opt => opt.MapFrom(src => src.Hour.Value.Hour))
            .ForMember(dest => dest.Minute, opt => opt.MapFrom(src => src.Hour.Value.Minute))
            .ForMember(dest => dest.Frequency, opt => opt.MapFrom(src => src.Frequency.Name))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name));

        // createMpa<source, destination>
        CreateMap<UTaskDTO, UTask>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Hour.Value.Hour, opt => opt.MapFrom(src => src.Hour))
            .ForMember(dest => dest.Hour.Value.Minute, opt => opt.MapFrom(src => src.Minute));

        // createMpa<destination, source>
        CreateMap<ChangeOfPasswordDTO, ChangeOfPassword>().ReverseMap();

    }

}


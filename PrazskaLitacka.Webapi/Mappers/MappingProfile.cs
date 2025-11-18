using AutoMapper;
using PrazskaLitacka.Domain.Dto;
using PrazskaLitacka.Domain.Entities;

namespace PrazskaLitacka.Webapi.Mappers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<RegisterUserRequestDto, User>();
        CreateMap<User, RegisterUserResponseDto>();
    }
}

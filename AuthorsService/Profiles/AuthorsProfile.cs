using AutoMapper;
using AuthorsService.Models;
using AuthorsService.Dtos;

namespace AuthorsService.Profiles
{
    public class AuthorsProfile: Profile
    {
        public AuthorsProfile()
        {
            //source -> Target
            CreateMap<Author, AuthorReadDto>();
            CreateMap<AuthorCreateDto, Author>();
            CreateMap<AuthorReadDto, AuthorPublishedDto>();
            CreateMap<Author, AuthorPublishedDto>();
            CreateMap<Author, GrpcAuthorModel>()
                .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName));
        }
    }
}
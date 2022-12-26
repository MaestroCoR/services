using AuthorsService;
using AutoMapper;
using BooksService.Dtos;
using BooksService.Models;

namespace BooksService.Profiles
{
    public class BooksProfile: Profile
    {
        public BooksProfile()
        {
            //Source -> Target
            CreateMap<Author, AuthorReadDto>();
            CreateMap<BookCreateDto, Book>();
            CreateMap<Book, BookReadDto>();
            CreateMap<AuthorPublishedDto, Author>()
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));
            CreateMap<GrpcAuthorModel, Author>()
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.AuthorId))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Books, opt => opt.Ignore());
        }
    }
}
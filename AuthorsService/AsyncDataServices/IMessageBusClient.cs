using AuthorsService.Dtos;

namespace AuthorsService.AsyncDataServices
{
    public interface IMessageBusClient
    {
        void PublishNewAuthor(AuthorPublishedDto authorPublishedDto);
        void DeleteAuthor(AuthorPublishedDto authorPublishedDto);
        void UpdateAuthor(AuthorPublishedDto authorPublishedDto);
    }
}
using BooksService.Models;

namespace BooksService.SyncDataServices.Grpc
{
    public interface IAuthorDataClient
    {
        IEnumerable<Author> ReturnAllAuthors();
    }
}
using System.Threading.Tasks;
using AuthorsService.Dtos;
namespace AuthorsService.SyncDataServices.Http
{
    public interface IBookDataClient
    {
        Task SendAuthorToBook(AuthorReadDto auth);
    }
}
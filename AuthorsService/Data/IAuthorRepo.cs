using AuthorsService.Models;

namespace AuthorsService.Data
{
    public interface IAuthorRepo
    {
        bool SaveChanges();
        IEnumerable<Author> GetAllAuthors();
        Author GetAuthorById(int id);
        void CreateAuthor(Author auth);
        void DeleteAuthor(int id);
        void UpdateAuthor(int id, Author author);
    }
}
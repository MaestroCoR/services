using BooksService.Models;

namespace BooksService.Data
{
    public interface IBookRepo
    {
        bool SaveChanges();
        
        //Authors
        IEnumerable<Author> GetAllAuthors();
        void CreateAuthor(Author auth);
        bool AuthorExists(int authorId);
        bool ExternalAuthorExists(int externalAuthorId);
        void DeleteAuthor(Author auth);
        void UpdateAuthor(Author auth);
        
        // Books
        IEnumerable<Book> GetBooksForAuthor(int authorId);
        Book GetBook(int authorId, int bookId);
        bool BookExists(int authorId, int bookId);
        void CreateBook(int authorId, Book book);
        void DeleteBook(int authorId, int bookId);
        void UpdateBook(int authorId, int bookId, Book book);
        
    }
}
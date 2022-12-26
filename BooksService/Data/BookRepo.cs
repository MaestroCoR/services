using BooksService.Models;

namespace BooksService.Data
{
    public class BookRepo : IBookRepo
    {
        private readonly AppDbContext _context;

        public BookRepo(AppDbContext context)
        {
            _context = context;
        }
        public bool AuthorExists(int authorId)
        {
            return _context.Authors.Any(p => p.Id == authorId);
        }

        public void CreateAuthor(Author auth)
        {
            if(auth == null)
            {
                throw new ArgumentNullException(nameof(auth));
            }
            _context.Authors.Add(auth);     
        }
        public void DeleteAuthor(Author auth)
        {
           var entity = _context.Authors.FirstOrDefault(p => p.Id == auth.Id);
            _context.Entry(entity).CurrentValues.SetValues(auth);

            //_context.Authors.Update(auth);     
        }
        
        public void UpdateAuthor(Author auth)
        {
            var entity = _context.Authors.FirstOrDefault(p => p.Id == auth.Id);
            _context.Entry(entity).CurrentValues.SetValues(auth);
        }

        public void CreateBook(int authorId, Book book)
        {
            if (book == null)
            {
                throw new ArgumentNullException(nameof(book));
            }
            book.AuthorId =authorId;
            _context.Books.Add(book);
        }

        public bool ExternalAuthorExists(int externalAuthorId)
        {
            return _context.Authors.Any(p => p.ExternalId == externalAuthorId);
        }

        public IEnumerable<Author> GetAllAuthors()
        {
            return _context.Authors.ToList();
        }
        public bool BookExists(int authorId, int bookId)
        {
            return _context.Books.Where(c => c.AuthorId == authorId && c.Id == bookId).Any(p => p.Id == bookId);
        }
        public Book GetBook(int authorId, int bookId)
        {
            return _context.Books
            .Where(c => c.AuthorId == authorId && c.Id == bookId).FirstOrDefault();
        }

        public IEnumerable<Book> GetBooksForAuthor(int authorId)
        {
            return _context.Books
            .Where(c => c.AuthorId == authorId)
            .OrderBy(c => c.Author.LastName);
        }

        public void DeleteBook(int authorId, int bookId)
        {
            if(_context.Authors.Any(p => p.Id == authorId)){
                _context.Books.Remove(_context.Books.FirstOrDefault(p => p.Id == bookId));
            }
        }

        public void UpdateBook(int authorId, int bookId, Book book)
        {
            if(book == null)
            {
                throw new ArgumentNullException(nameof(book));
            }
            
            var entity = _context.Books.FirstOrDefault(p => p.Id == bookId);
            entity.Title = book.Title;
            entity.DateOfPublication = book.DateOfPublication;    
        }
        public bool SaveChanges()
        {
            return(_context.SaveChanges() >= 0);
        }
       
    }
}
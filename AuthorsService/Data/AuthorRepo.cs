using AuthorsService.Models;

namespace AuthorsService.Data
{
    public class AuthorRepo : IAuthorRepo
    {
        private readonly AppDbContext _context;

        public AuthorRepo(AppDbContext context)
        {
            _context = context;
        }
        public void CreateAuthor(Author auth)
        {
            if(auth == null)
            {
                throw new ArgumentNullException(nameof(auth));
            }

            _context.Authors.Add(auth);
        }


        public void DeleteAuthor(int id)
        {
            if(_context.Authors.Any(p => p.Id == id)){
                _context.Authors.Remove(_context.Authors.FirstOrDefault(p => p.Id == id));
            }
            
        }

        public IEnumerable<Author> GetAllAuthors()
        {
            return _context.Authors.ToList();
        }


        public void UpdateAuthor(int id, Author auth)
        {

            if(auth == null)
            {
                throw new ArgumentNullException(nameof(auth));
            }
            
            var entity = _context.Authors.FirstOrDefault(p => p.Id == id);
            entity.FirstName = auth.FirstName;
            entity.LastName = auth.LastName;
            entity.BirthDate = auth.BirthDate;
            entity.Country = auth.Country;      
           
        }
        public Author GetAuthorById(int id)
        {
            return _context.Authors.FirstOrDefault(p => p.Id == id);
        }

        public bool SaveChanges()
        {
            return(_context.SaveChanges() >= 0);
        }
    }
}
using AutoMapper;
using BooksService.Data;
using BooksService.Dtos;
using BooksService.Models;
using Microsoft.AspNetCore.Mvc;

namespace BooksService.Controllers
{
    [Route("api/c/authors/{authorId}/[controller]")]
    [ApiController]
    public class BooksController: ControllerBase
    {
        private readonly IBookRepo _repository;
        private readonly IMapper _mapper;

        public BooksController(IBookRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        [HttpGet]
        public ActionResult<IEnumerable<BookReadDto>> GetBooksOfAuthor(int authorId)
        {
            Console.WriteLine($"---> Hit GetBooksOfAuthor {authorId}");

            if (!_repository.AuthorExists(authorId))
            {
                return NotFound();
            }

            var books = _repository.GetBooksForAuthor(authorId);

            return Ok(_mapper.Map<IEnumerable<BookReadDto>>(books));
        }

        [HttpGet("{bookId}", Name = "GetBookOfAuthor")]
        public ActionResult<BookReadDto> GetBookOfAuthor(int authorId, int bookId)
        {
            Console.WriteLine($"---> Hit GetBookOfAuthor {authorId} / {bookId}");

            if (!_repository.AuthorExists(authorId))
            {
                return NotFound();
            }

            var book = _repository.GetBook(authorId, bookId);
            if (book == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<BookReadDto>(book));
        }

        [HttpPost]
        public ActionResult<BookReadDto> CreateBookOfAuthor(int authorId, BookCreateDto bookDto)
        {
            Console.WriteLine($"---> CreateBookOfAuthor {authorId}");

            if (!_repository.AuthorExists(authorId))
            {
                return NotFound();
            }

            var book = _mapper.Map<Book>(bookDto);

            _repository.CreateBook(authorId, book);
            _repository.SaveChanges();

            var bookReadDto = _mapper.Map<BookReadDto>(book);

            return CreatedAtRoute(nameof(GetBookOfAuthor),
               new {authorId = authorId, bookId = bookReadDto.Id}, bookReadDto );
        }
        [HttpDelete("{bookId}")]
        public ActionResult<BookReadDto> DeleteBookOfAuthor(int authorId, int bookId)
        {
            
            if (!_repository.BookExists(authorId, bookId))
            {
                return NotFound();
            }
            Console.WriteLine($"---> Delete Book {bookId} Of Author {authorId}");
            _repository.DeleteBook(authorId, bookId);
            _repository.SaveChanges();
            return Ok("Book successfully deleted");
        }
        [HttpPut("{bookId}")]
        public ActionResult<BookReadDto> UpdateBookOfAuthor(int authorId, int bookId, BookCreateDto bookCreateDto)
        {
            if (!_repository.BookExists(authorId, bookId))
            {
                return NotFound();
            }
            Console.WriteLine($"---> Update Book {bookId} Of Author {authorId}");
             var bookModel = _mapper.Map<Book>(bookCreateDto);
            _repository.UpdateBook(authorId, bookId, bookModel);
            _repository.SaveChanges();
            return Ok("Book successfully updated");
        }
    }
}
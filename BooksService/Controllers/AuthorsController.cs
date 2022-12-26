using AutoMapper;
using BooksService.Data;
using BooksService.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace BooksService.Controllers
{
    [Route("api/c/[controller]")]
    [ApiController]
    public class AuthorsController: ControllerBase
    {
        private readonly IBookRepo _repository;
        private readonly IMapper _mapper;

        public AuthorsController(IBookRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        [HttpGet]
        public ActionResult<IEnumerable<AuthorReadDto>> GetAuthors()
        {
            Console.WriteLine("---> Getting Authors from BooksService");
            var authorItems = _repository.GetAllAuthors();

            return Ok(_mapper.Map<IEnumerable<AuthorReadDto>>(authorItems));
        }
        [HttpPost]
        public  ActionResult TestInboundConnections()
        {
            Console.WriteLine("----> Inbound POST # Books Service");

            return Ok("Inbound test of from Author Controller");
        }

    }
}
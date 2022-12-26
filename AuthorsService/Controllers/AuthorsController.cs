using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using AuthorsService.Data;
using AuthorsService.Dtos;
using AuthorsService.Models;
using AuthorsService.SyncDataServices.Http;
using AuthorsService.AsyncDataServices;

namespace AuthorsService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController: ControllerBase
    {
        private readonly IAuthorRepo _repository;
        private readonly IMapper _mapper;
        private readonly IBookDataClient _bookDataClient;
        private readonly IMessageBusClient _messageBusClient;

        public AuthorsController(
            IAuthorRepo repository, 
            IMapper mapper,
            IBookDataClient bookDataClient,
            IMessageBusClient messageBusClient)
        {
            _repository = repository;
            _mapper = mapper;
            _bookDataClient = bookDataClient;
            _messageBusClient = messageBusClient;
        }
        [HttpGet]
        public ActionResult<IEnumerable<AuthorReadDto>> GetAuthors()
        {
            Console.WriteLine(" > Getting Authors.....");
            
            var authorItem = _repository.GetAllAuthors();
            return Ok(_mapper.Map<IEnumerable<AuthorReadDto>>(authorItem));
        }
        [HttpGet("{id}", Name ="GetAuthorById")]
        public ActionResult<AuthorReadDto> GetAuthorById(int id)
        {
            var authorItem = _repository.GetAuthorById(id);
            if (authorItem != null)
            {
                return Ok(_mapper.Map<AuthorReadDto>(authorItem));
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<ActionResult<AuthorReadDto>> CreateAuthor(AuthorCreateDto authorCreateDto)
        {
            var authorModel = _mapper.Map<Author>(authorCreateDto);
            _repository.CreateAuthor(authorModel);
            _repository.SaveChanges();

            var authorReadDto = _mapper.Map<AuthorReadDto>(authorModel);

            //Send Sync Message
            try
            {
                await _bookDataClient.SendAuthorToBook(authorReadDto);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"----> Could not send synchronously: {ex.Message}");
            }

            //Send Async Message
            try
            {
                var authorPublishedDto = _mapper.Map<AuthorPublishedDto>(authorReadDto);
                authorPublishedDto.Event = "Author_Published";
                _messageBusClient.PublishNewAuthor(authorPublishedDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"----> Could not send asynchronously: {ex.Message}");
            }
            return CreatedAtRoute(nameof(GetAuthorById), new {Id = authorReadDto.Id}, authorReadDto);
        }
        //Delete:
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var authorItem = _repository.GetAuthorById(id);
            if (authorItem != null)
            {
                _repository.DeleteAuthor(id);
                
                try
                {   authorItem.FirstName = "unknown";
                    authorItem.LastName = "unknown";
                    var authorPublishedDto = _mapper.Map<AuthorPublishedDto>(authorItem);
                    authorPublishedDto.Event = "Author_Deleted";
                    _messageBusClient.DeleteAuthor(authorPublishedDto);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"----> Could not send asynchronously: {ex.Message}");
                }
                
                
                _repository.SaveChanges();
                return Ok("Author successfully deleted");

            }
            return NotFound();
        
        }

         [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuthor(int id, AuthorCreateDto authorCreateDto)
        {
            var authorItem = _repository.GetAuthorById(id);
            if (authorItem != null)
            {
            var authorModel = _mapper.Map<Author>(authorCreateDto);
            _repository.UpdateAuthor(id, authorModel);
            _repository.SaveChanges();

           try
                {
                    var authorPublishedDto = _mapper.Map<AuthorPublishedDto>(authorItem);
                    authorPublishedDto.Event = "Author_Updated";
                    _messageBusClient.UpdateAuthor(authorPublishedDto);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"----> Could not send asynchronously: {ex.Message}");
                }
            return Ok("Author successfully updated");
            }
        return NotFound();
        }
    }

}
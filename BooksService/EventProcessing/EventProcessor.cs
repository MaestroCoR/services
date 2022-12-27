using System.Text.Json;
using AutoMapper;
using BooksService.Data;
using BooksService.Dtos;
using BooksService.Models;

namespace BooksService.EventProccessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }
        public void ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);

            switch (eventType)
            {
                case EventType.AuthorPublished:
                    addAuthor(message);
                    break;
                case EventType.AuthorDeleted:
                    deleteAuthor(message);
                    break;
                case EventType.AuthorUpdated:
                    updateAuthor(message);
                    break;
                default:
                    break;
            }
        }

        private EventType DetermineEvent(string notificationMessage)
        {
            Console.WriteLine("---> Determining Event");

            var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);

            switch(eventType.Event)
            {
                case "Author_Published":
                    Console.WriteLine("--->Author Published Event Detected");
                    return EventType.AuthorPublished;
                case "Author_Deleted":
                    Console.WriteLine("--->Author Deleted Event Detected");
                    return EventType.AuthorDeleted;
                case "Author_Updated":
                    Console.WriteLine("--->Author Updated Event Detected");
                    return EventType.AuthorUpdated;
                default:
                    Console.WriteLine("--->Could not determine the event type");
                    return EventType.Undetermined;
            }
        }
        private void addAuthor(string authorPublishedMessage)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IBookRepo>();

                var authorPublishedMDto = JsonSerializer.Deserialize<AuthorPublishedDto>(authorPublishedMessage);
                
                try
                {
                    var auth = _mapper.Map<Author>(authorPublishedMDto);
                    if (!repo.ExternalAuthorExists(auth.ExternalId))
                    {
                        repo.CreateAuthor(auth);
                        repo.SaveChanges();
                        Console.WriteLine("---> Author added");
                    }
                    else
                    {
                        Console.WriteLine("---> Author already exists...");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"---> Could not add Author to DB{ex.Message}");
                }
            }
        }
        private void deleteAuthor(string authorDeletedMessage)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IBookRepo>();

                var authorDeletedMDto = JsonSerializer.Deserialize<AuthorPublishedDto>(authorDeletedMessage);
                
                try
                {
                    var auth = _mapper.Map<Author>(authorDeletedMDto);
                    if (repo.ExternalAuthorExists(auth.ExternalId))
                    {
                        repo.DeleteAuthor(auth);
                        repo.SaveChanges();
                        Console.WriteLine("---> Author Deleted");
                    }
                    else
                    {
                        Console.WriteLine("---> Author not found...");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"---> Could not delete Author from DB{ex.Message}");
                }
            }
        }
        private void updateAuthor(string authorUpdatedMessage)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IBookRepo>();

                var authorUpdatedMDto = JsonSerializer.Deserialize<AuthorPublishedDto>(authorUpdatedMessage);
                
                try
                {
                    var auth = _mapper.Map<Author>(authorUpdatedMDto);
                    if (repo.ExternalAuthorExists(auth.ExternalId))
                    {
                        repo.UpdateAuthor(auth);
                        repo.SaveChanges();
                        Console.WriteLine("---> Author Updated");
                    }
                    else
                    {
                        Console.WriteLine("---> Author not found...");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"---> Could not delete Author from DB{ex.Message}");
                }
            }
        }
    }
    enum EventType
    {
        AuthorPublished,
        AuthorDeleted,
        AuthorUpdated,
        Undetermined
    }
}
namespace BooksService.EventProccessing
{
    public interface IEventProcessor
    {
        void ProcessEvent(string message);
        
    }
}
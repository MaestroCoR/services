namespace BooksService.Dtos
{
    public class BookReadDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime DateOfPublication {get; set; }
        public int AuthorId {get; set;}

    }
}
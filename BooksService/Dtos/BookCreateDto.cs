using System.ComponentModel.DataAnnotations;

namespace BooksService.Dtos
{
    public class BookCreateDto
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public DateTime DateOfPublication {get; set; }
       
    }
}
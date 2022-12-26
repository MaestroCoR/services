using System.ComponentModel.DataAnnotations;

namespace BooksService.Models
{
    public class Book
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public DateTime DateOfPublication {get; set; }
        [Required]
        public int AuthorId {get; set;}
        public Author Author {get; set; }
    }    
}
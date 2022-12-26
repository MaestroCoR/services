using System.ComponentModel.DataAnnotations;

namespace BooksService.Models
{
    public class Author
    {
        [Key]
        [Required]
        public int Id{ get; set; }
        [Required]
        public int ExternalId { get; set;}
        [Required]
        public string FirstName {get; set;}
        [Required]
        public string LastName {get; set;}
        
        public ICollection<Book> Books {get; set;} = new List<Book>();
    }
}
using System.ComponentModel.DataAnnotations;

namespace AuthorsService.Models
{
    public class Author
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public  DateTime BirthDate { get; set; }
    }

}
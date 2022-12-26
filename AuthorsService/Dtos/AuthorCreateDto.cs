using System.ComponentModel.DataAnnotations;

namespace AuthorsService.Dtos
{
    public class AuthorCreateDto
    {
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
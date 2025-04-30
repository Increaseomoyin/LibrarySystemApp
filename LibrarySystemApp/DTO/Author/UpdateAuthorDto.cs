using System.ComponentModel.DataAnnotations;

namespace LibrarySystemApp.DTO.Author
{
    public class UpdateAuthorDto
    { 
        public int Id { get; set; }
        [Required]
        public string? FirstName { get; set; }
        [Required]

        public string? LastName { get; set; }
        [Required]

        public string? Country { get; set; }

    }
}

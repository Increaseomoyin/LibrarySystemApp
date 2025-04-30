using System.ComponentModel.DataAnnotations;

namespace LibrarySystemApp.DTO.Author
{
    public class CreateAuthorDto
    {
        [Required]

        public string? FirstName { get; set; }
         [Required]

        public string? LastName { get; set; }
        [Required]

        public string? Country { get; set; }
        [Required]

        public string? AppUserId { get; set; }
    }
}

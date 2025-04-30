using System.ComponentModel.DataAnnotations;

namespace LibrarySystemApp.DTO.Borrower
{
    public class CreateBorrowerDto
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

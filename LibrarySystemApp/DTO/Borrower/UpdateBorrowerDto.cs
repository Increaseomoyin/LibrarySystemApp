using System.ComponentModel.DataAnnotations;

namespace LibrarySystemApp.DTO.Borrower
{
    public class UpdateBorrowerDto
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

using System.ComponentModel.DataAnnotations;

namespace LibrarySystemApp.DTO.Book
{
    public class CreateBookDto
    {
        [Required]
        public string? Title { get; set; }
    }
}

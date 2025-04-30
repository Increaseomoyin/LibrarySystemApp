using System.ComponentModel.DataAnnotations;

namespace LibrarySystemApp.DTO.Book
{
    public class UpdateBookDto
    {
        [Required]
        public int Id { get; set; }
        [Required]

        public string? Title { get; set; }
        
    }
}

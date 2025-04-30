using System.ComponentModel.DataAnnotations;

namespace LibrarySystemApp.DTO.Book
{
    public class DeleteBookDto
    {
        [Required]
        public int Id { get; set; }
    }
}

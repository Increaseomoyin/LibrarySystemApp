using System.ComponentModel.DataAnnotations;

namespace LibrarySystemApp.DTO.Category
{
    public class CategoryDto
    {
        [Required]
        public int Id { get; set; }
        public string? Name { get; set; }
    }
}

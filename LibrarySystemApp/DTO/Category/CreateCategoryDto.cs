using System.ComponentModel.DataAnnotations;

namespace LibrarySystemApp.DTO.Category
{
    public class CreateCategoryDto
    {
        [Required]
        public string? Name { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace LibrarySystemApp.DTO.Category
{
    public class DeleteCategoryDto
    {
        [Required]
        public int Id { get; set; }

    }
}

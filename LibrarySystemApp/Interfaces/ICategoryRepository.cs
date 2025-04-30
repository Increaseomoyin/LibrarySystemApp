using LibrarySystemApp.Models;

namespace LibrarySystemApp.Interfaces
{
    public interface ICategoryRepository
    {
        //GET METHODS
        Task<ICollection<Category>> GetCategories();
        Task<Category> GetCategoryById(int id);
        Task<bool> CategoryExists(int id);
        //CREATE METHODS
        Task<bool> CreateCategory(Category categoryCreate);
        Task<bool> Save();
        //UPDATE METHOD
        Task<bool> UpdateCategory(Category categoryUpdate);


    }
}

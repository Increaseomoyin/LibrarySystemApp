using LibrarySystemApp.Data;
using LibrarySystemApp.Interfaces;
using LibrarySystemApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystemApp.Repositories
{
    public class CategoryRepository :ICategoryRepository
    {
        private readonly DataContext _dataContext;

        public CategoryRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<bool> CategoryExists(int id)
        {
            return await _dataContext.Categories.AnyAsync(c => c.Id == id);
        }

        public async Task<bool> CreateCategory(Category categoryCreate)
        {
            await _dataContext.AddAsync(categoryCreate);
            return await Save();
        }

        public async Task<ICollection<Category>> GetCategories()
        {
            return await _dataContext.Categories.OrderBy(c=>c.Id).ToListAsync();
        }

        public async Task<Category> GetCategoryById(int id)
        {
            return await _dataContext.Categories.Where(c => c.Id == id).FirstOrDefaultAsync();
        }

        public async Task<bool> Save()
        {
            var saved = await _dataContext.SaveChangesAsync();
            return saved > 0 ?true: false;
        }

        public async Task<bool> UpdateCategory(Category categoryUpdate)
        {
             _dataContext.Categories.Update(categoryUpdate);
            return await Save();
        }
    }
}

using LibrarySystemApp.Data;
using LibrarySystemApp.DTO.Book;
using LibrarySystemApp.Interfaces;
using LibrarySystemApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystemApp.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly DataContext _dataContext;

        public BookRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<bool> BookExists(int id)
        {
            return await  _dataContext.Books.AnyAsync(b => b.Id == id);
        }

        public async Task<bool> CreateBook(List<int> categoryIds, Book createBookDto)
        {  
           var book = await _dataContext.Books.FirstOrDefaultAsync(b=>b.Title == createBookDto.Title);
            if(book != null)
                throw new Exception("A book with this title already exists.");
            await _dataContext.AddAsync(createBookDto);
            await _dataContext.SaveChangesAsync();

            foreach (int Id in categoryIds)
            {
                var category = await _dataContext.Categories.FirstOrDefaultAsync(c => c.Id == Id);
                if(category != null)
                {
                    var bookCategories = new BookCategory()
                    { 
                        BookId = createBookDto.Id,
                        CategoryId = category.Id
                    };
                    await _dataContext.AddAsync(bookCategories);
                }
            }
            return await Save();
        }

       

        public async Task<Book> GetBookById(int id)
        {
           var book= await _dataContext.Books.Where(b => b.Id == id).FirstOrDefaultAsync();
            return book;
        }

        public async Task<Book> GetBookByTitle(string title)
        {
            return await _dataContext.Books.Where(b => b.Title == title).FirstOrDefaultAsync();

        }

        public async Task<ICollection<Book>> GetBooks()
        {
           return await _dataContext.Books.ToListAsync();
        }

        public async Task<bool> Save()
        {
            var saved =await  _dataContext.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

        public async Task<bool> UpdateBook(List<int> categoryId, Book updateBook)
        {
            _dataContext.Books.Update(updateBook);
           await _dataContext.SaveChangesAsync();

            foreach(int id in categoryId)
            {
                if(id !=null)
                {
                    var bookCategories = await _dataContext.BookCategories.Where(bc => bc.BookId == updateBook.Id).ToListAsync();
                    _dataContext.BookCategories.RemoveRange(bookCategories);
                }
                var cat = await _dataContext.Categories.FirstOrDefaultAsync(c => c.Id == id);
                if(cat != null)
                {
                    var bookCategory = new BookCategory()
                    { 
                        BookId= updateBook.Id,
                        CategoryId=id
                    };
                   await _dataContext.BookCategories.AddAsync(bookCategory);
                }
            }
            return await Save();
        }
    }
}

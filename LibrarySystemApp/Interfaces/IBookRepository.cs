using LibrarySystemApp.DTO.Book;
using LibrarySystemApp.Models;

namespace LibrarySystemApp.Interfaces
{
    public interface IBookRepository
    {  
        //GET METHODS
       Task<ICollection<Book>> GetBooks();
       Task<Book> GetBookById (int id);
       Task<Book> GetBookByTitle (string title);
       Task<bool> BookExists (int id);

        //CREATE METHODS
        Task<bool> CreateBook (List<int> categoryIds, Book createBook);
        Task<bool> Save();

       // UPDATE METHODS
        Task<bool> UpdateBook(List<int> categoryId  ,Book updateBook);
        //DELETE METHODS
        //bool DeleteBook(int id);
    }
}

using LibrarySystemApp.Models;

namespace LibrarySystemApp.Interfaces
{
    public interface IAuthorRepository
    {
        //GET METHODS
        Task<ICollection<Author>> GetAuthors();
        Task<Author> GetAuthorById(int Id);
        Task<Author> GetAuthorByName(string FirstName, string LastName);
        Task<bool> AuthorExists(int Id);
        //CREATE METHODS
        Task<bool> CreateAuthor(List<int> bookId, Author authorCreate);
        Task<bool> Save();
        //UPDATE METHO
        Task<bool> UpdateAuthor(List<int> bookId, Author authorUpdate);
    }
}

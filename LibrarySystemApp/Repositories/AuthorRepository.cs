using LibrarySystemApp.Data;
using LibrarySystemApp.Interfaces;
using LibrarySystemApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystemApp.Repositories
{
    public class AuthorRepository :IAuthorRepository
    {
        private readonly DataContext _dataContext;
        private readonly UserManager<AppUser> _userManager;

        public AuthorRepository(DataContext dataContext, UserManager<AppUser> userManager)
        {
           _dataContext = dataContext;
            _userManager = userManager;
        }

        public async Task<bool> AuthorExists(int Id)
        {
            return await _dataContext.Authors.AnyAsync(a=>a.Id ==Id);
        }

        public async Task<bool> CreateAuthor(List<int> bookId, Author authorCreate)
        {

            var existingBorrower = await _dataContext.Borrowers
               .FirstOrDefaultAsync(b => b.AppUserId == authorCreate.AppUserId);
            var existingAuthors = await _dataContext.Authors
              .FirstOrDefaultAsync(a => a.AppUserId == authorCreate.AppUserId);
            if (existingBorrower != null || existingAuthors != null)
            {
                throw new Exception("This AppUser already has an account.");
            }
            var oldAuthor = await _dataContext.Authors.AnyAsync(a=>a.FirstName ==authorCreate.FirstName && a.LastName ==authorCreate.LastName);
            if (!oldAuthor)
            {
               var userAuthor = await _dataContext.AddAsync(authorCreate);
               await _dataContext.SaveChangesAsync();
               var appUser = await _dataContext.Users.FirstOrDefaultAsync(u => u.Id == authorCreate.AppUserId);
              await _userManager.AddToRoleAsync(appUser, "Author");
            }
            else
            {
                throw new Exception("Author Already Exist");
            }
            foreach(int id in bookId)
            {
                var book = await _dataContext.Books.AnyAsync(b => b.Id == id);
                if (book)
                {
                    var bookAuthors = new BookAuthor()
                    { 
                        BookId = id,
                        AuthorId = authorCreate.Id
                    };
                    await _dataContext.AddAsync(bookAuthors);

                }
            }
            return await Save();
        }

        public async Task<Author> GetAuthorById(int Id)
        {
            return await _dataContext.Authors.FirstOrDefaultAsync(a => a.Id == Id);
        }

        public async Task<Author> GetAuthorByName(string FirstName, string LastName)
        {
            return await _dataContext.Authors.FirstOrDefaultAsync(a => a.FirstName == FirstName && a.LastName == LastName);

        }

        public async Task<ICollection<Author>> GetAuthors()
        {
            return await _dataContext.Authors.OrderBy(a=>a.Id).ToListAsync();
        }

        public async Task<bool> Save()
        {
            var saved =await _dataContext.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

        public async Task<bool> UpdateAuthor(List<int> bookId, Author authorUpdate)
        {
            // 1. Fetch the existing Author from the database
            var existingAuthor = await _dataContext.Authors.FirstOrDefaultAsync(a => a.Id == authorUpdate.Id);

            if (existingAuthor == null)
            {
                throw new Exception("Author not found.");
            }

            // 2. Only update the fields you want (keep AppUserId untouched)
            existingAuthor.FirstName = authorUpdate.FirstName;
            existingAuthor.LastName = authorUpdate.LastName;
            existingAuthor.Country = authorUpdate.Country;

            await _dataContext.SaveChangesAsync();

            foreach (int id in bookId)
            {
                if(id !=null)
                {
                    var bookAuthors = await _dataContext.BookAuthors.Where(ba => ba.AuthorId == authorUpdate.Id).FirstOrDefaultAsync();
                    _dataContext.BookAuthors.RemoveRange(bookAuthors);
                }
                var book = await _dataContext.Books.AnyAsync(b => b.Id == id);
                if(book)
                {
                    var bookAuthor = new BookAuthor()
                    { 
                        BookId=id,
                        AuthorId=authorUpdate.Id
                    };
                    _dataContext.BookAuthors.Add(bookAuthor);
                }
            }
            return await Save();
        }
    }
}

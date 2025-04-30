using LibrarySystemApp.Data;
using LibrarySystemApp.Interfaces;
using LibrarySystemApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace LibrarySystemApp.Repositories
{
    public class BorrowerRepository : IBorrowerRepository
    {
        private readonly DataContext _dataContext;
        private readonly UserManager<AppUser> _userManager;

        public BorrowerRepository(DataContext dataContext, UserManager<AppUser> userManager)
        {
           _dataContext = dataContext;
            _userManager = userManager;
        }
        public async Task<bool> BorrowerExists(int Id)
        {
            return await _dataContext.Borrowers.AnyAsync(b => b.Id == Id);
        }

        public async Task<bool> CreateBorrower(List<int> bookId, Borrower borrowerCreate)
        {
            var existingBorrower = await _dataContext.Borrowers
                .FirstOrDefaultAsync(b => b.AppUserId == borrowerCreate.AppUserId);
            var existingAuthors = await _dataContext.Authors
              .FirstOrDefaultAsync(a => a.AppUserId == borrowerCreate.AppUserId);
            if (existingBorrower != null || existingAuthors !=null)
            {
                throw new Exception("This AppUser already has an account.");
            }
            var oldBorrower = await _dataContext.Borrowers.AnyAsync(b=>b.FirstName == borrowerCreate.FirstName && b.LastName ==borrowerCreate.LastName);
           
          
            if (oldBorrower)
                throw new Exception("Borrower Already Exist!");
            await _dataContext.Borrowers.AddAsync(borrowerCreate);
            await _dataContext.SaveChangesAsync();

            var user = await _dataContext.Users.FirstOrDefaultAsync(u => u.Id == borrowerCreate.AppUserId);
            if (user == null)
            {
                throw new Exception("AppUserId is not valid");
            }
            await _userManager.AddToRoleAsync(user, "Borrower");
            foreach(int id in bookId)
            {   
                var book = await _dataContext.Books.AnyAsync(b=>b.Id == id);
                if(book)
                {
                    var bookBorrower = new BookBorrower()
                    {
                        BookId = id,
                        BorrowerId = borrowerCreate.Id
                    };
                    await _dataContext.BookBorrowers.AddAsync(bookBorrower);

                }
            }
            return await Save();

        }

        public async Task<Borrower> GetBorrowerById(int Id)
        {
            return await _dataContext.Borrowers.FirstOrDefaultAsync(b => b.Id == Id);
        }

        public async Task<Borrower> GetBorrowerByName(string FirstName, string LastName)
        {
            return await _dataContext.Borrowers.FirstOrDefaultAsync(b => b.FirstName == FirstName && b.LastName == LastName);

        }

        public async Task<ICollection<Borrower>> GetBorrowers()
        {
            return await _dataContext.Borrowers.OrderBy(b => b.Id).ToListAsync();
        }

        public async Task<bool> Save()
        {
            var saved = await _dataContext.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

        public async Task<bool> UpdateBorrower(List<int> BookIds, Borrower borrowerUpdate)
        {
            var existingBorrower = await _dataContext.Borrowers.FirstOrDefaultAsync(b => b.Id == borrowerUpdate.Id);
            if (existingBorrower == null)
                throw new Exception("Borrower does not exist");
            //Make changes
            existingBorrower.FirstName = borrowerUpdate.FirstName;
            existingBorrower.LastName = borrowerUpdate.LastName;
            existingBorrower.Country = borrowerUpdate.Country;
            //Save changes 
            await _dataContext.SaveChangesAsync();

            foreach(int id in BookIds)
            {
                if(id!=null)
                {
                    var bookBorrowers = await _dataContext.BookBorrowers.Where(bb => bb.BorrowerId == borrowerUpdate.Id).ToListAsync();
                    _dataContext.BookBorrowers.RemoveRange(bookBorrowers);
                }
                var book = await _dataContext.Books.FirstOrDefaultAsync(b => b.Id == id);
                if(book !=null)
                {
                    var bookBorrower = new BookBorrower()
                    {
                        BookId = id,
                        BorrowerId = borrowerUpdate.Id
                    };
                    _dataContext.BookBorrowers.Add(bookBorrower);
                }
            }
            return await Save();
        }

    }
}

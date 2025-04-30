using LibrarySystemApp.Models;

namespace LibrarySystemApp.Interfaces
{
    public interface IBorrowerRepository
    {
        //GET METHODS
        Task<ICollection<Borrower>> GetBorrowers();
        Task<Borrower> GetBorrowerById(int Id);
        Task<Borrower> GetBorrowerByName(string FirstName, string LastName);
        Task<bool> BorrowerExists(int Id);
        //CREATE METHODS
        Task<bool> CreateBorrower(List<int> bookId, Borrower borrowerCreate);
        Task<bool> Save();
        //UPDAET METHOD
        Task<bool> UpdateBorrower(List<int> BookIds, Borrower borrowerUpdate);
    }
}


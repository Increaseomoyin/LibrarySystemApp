namespace LibrarySystemApp.Models
{
    public class Borrower
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Country { get; set; }
        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
        public ICollection<BookBorrower>? BookBorrowers { get; set; }
    }
}

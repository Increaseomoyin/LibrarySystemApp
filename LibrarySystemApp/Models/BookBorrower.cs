namespace LibrarySystemApp.Models
{
    public class BookBorrower
    {
        public int BookId { get; set; }
        public Book? Book { get; set; }
        public int BorrowerId { get; set; }
        public Borrower? Borrower { get; set; }

    }
}

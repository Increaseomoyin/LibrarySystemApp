namespace LibrarySystemApp.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public ICollection<BookAuthor>? BookAuthors { get; set; }
        public ICollection<BookCategory>? BookCategories { get; set; }
        public ICollection<BookBorrower>? BookBorrowers { get; set; }
    }
}

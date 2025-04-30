﻿namespace LibrarySystemApp.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Country { get; set; }
        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
        public ICollection<BookAuthor>? BookAuthors { get; set; }
    }
}

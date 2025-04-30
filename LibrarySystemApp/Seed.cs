using LibrarySystemApp.Data;
using LibrarySystemApp.Models;

namespace LibrarySystemApp
{
    public class Seed
    {
        private readonly DataContext _dataContext;

        public Seed(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void SeedDataContext()
        {
            if (!_dataContext.Authors.Any())
            {

                var books = new List<Book>()
                {
                    new Book(){Title ="The Great Gatsby" },
                    new Book(){Title ="1984" },
                    new Book(){Title ="To Kill a Mockingbird" },
                    new Book(){Title ="Moby-Dick " },
                    new Book(){Title ="Pride and Prejudice" },
                    new Book(){Title ="The Catcher in the Rye" },
                    new Book(){Title ="The Hobbit " },
                    new Book(){Title ="War and Peace" },
                    new Book(){Title ="The Odyssey" },
                    new Book(){Title ="Crime and Punishment" },
                    new Book(){Title ="Brave New World" },
                    new Book(){Title ="Frankenstein" },
                    new Book(){Title ="Jane Eyre" },
                    new Book(){Title ="The Picture of Dorian Gray" },
                    new Book(){Title ="The Lord of the Rings" },
                };
                _dataContext.AddRange(books);
                _dataContext.SaveChanges();

                var categories = new List<Category>()
                {
                    new Category(){Name = "Horror"},
                    new Category(){Name = "Fiction"},
                    new Category(){Name = "Mystery"},
                    new Category(){Name = "Fantasy"},
                    new Category(){Name = "Commedy"}
                };
                _dataContext.AddRange(categories);
                _dataContext.SaveChanges();

                var bookCategories = new List<BookCategory>()
                {
                     new BookCategory() { Book = books[0], Category = categories[0] },
                     new BookCategory() { Book = books[0], Category = categories[1] },
                     new BookCategory() { Book = books[0], Category = categories[2] },

                     new BookCategory() { Book = books[1], Category = categories[1] },
                     new BookCategory() { Book = books[1], Category = categories[3] },

                     new BookCategory() { Book = books[2], Category = categories[0] },
                     new BookCategory() { Book = books[2], Category = categories[4] },

                     new BookCategory() { Book = books[3], Category = categories[2] },
                     new BookCategory() { Book = books[3], Category = categories[3] },

                     new BookCategory() { Book = books[4], Category = categories[0] },

                     new BookCategory() { Book = books[5], Category = categories[1] },
                     new BookCategory() { Book = books[5], Category = categories[4] },

                     new BookCategory() { Book = books[6], Category = categories[3] },

                     new BookCategory() { Book = books[7], Category = categories[2] },
                     new BookCategory() { Book = books[7], Category = categories[4] },

                     new BookCategory() { Book = books[8], Category = categories[1] },
                     new BookCategory() { Book = books[8], Category = categories[0] },

                     new BookCategory() { Book = books[9], Category = categories[4] },

                     new BookCategory() { Book = books[10], Category = categories[1] },

                     new BookCategory() { Book = books[11], Category = categories[2] },
                     new BookCategory() { Book = books[11], Category = categories[3] },

                     new BookCategory() { Book = books[12], Category = categories[0] },
                     new BookCategory() { Book = books[12], Category = categories[4] },

                     new BookCategory() { Book = books[13], Category = categories[1] },
                     new BookCategory() { Book = books[13], Category = categories[2] },

                     new BookCategory() { Book = books[14], Category = categories[3] },
                     new BookCategory() { Book = books[14], Category = categories[4] }

                };
                _dataContext.AddRange(bookCategories);
                _dataContext.SaveChanges();

            }
        }
    }
}













           

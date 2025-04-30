using LibrarySystemApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystemApp.Data
{
    public class DataContext :IdentityDbContext<AppUser>
    {
        public DataContext(DbContextOptions<DataContext> options) :base(options)
        {
            
        }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Borrower> Borrowers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<BookCategory> BookCategories { get; set; }
        public DbSet<BookAuthor> BookAuthors { get; set; }
        public DbSet<BookBorrower> BookBorrowers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var roles = new List<IdentityRole>()
            { 
                new IdentityRole()
                {
                    Id = "1",
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                 new IdentityRole()
                {
                    Id = "2",
                    Name = "Author",
                    NormalizedName = "AUTHOR"
                },
                  new IdentityRole()
                {
                    Id = "3",
                    Name = "Borrower",
                    NormalizedName = "BORROWER"
                },
            };

            modelBuilder.Entity<IdentityRole>()
                .HasData(roles);


            //Creating the composite key for BookAuthor
            modelBuilder.Entity<BookAuthor>()
                .HasKey(ba => new { ba.AuthorId, ba.BookId });
            //Doing the Book Part
            modelBuilder.Entity<BookAuthor>()
                .HasOne(b => b.Book)
                .WithMany(ba => ba.BookAuthors)
                .HasForeignKey(b => b.BookId);
            //Doing the Author Part
            modelBuilder.Entity<BookAuthor>()
                .HasOne(a => a.Author)
                .WithMany(ba => ba.BookAuthors)
                .HasForeignKey(a => a.AuthorId);

            //Creating the composite key for BookCategory
            modelBuilder.Entity<BookCategory>()
                .HasKey(bc => new { bc.CategoryId, bc.BookId });
            //Doing the Book Part
            modelBuilder.Entity<BookCategory>()
                .HasOne(b => b.Book)
                .WithMany(bc => bc.BookCategories)
                .HasForeignKey(b => b.BookId);
            //Doing the Category Part
            modelBuilder.Entity<BookCategory>()
                .HasOne(c => c.Category)
                .WithMany(bc => bc.BookCategories)
                .HasForeignKey(c => c.CategoryId);

            //Creating the composite key for BookBorrower
            modelBuilder.Entity<BookBorrower>()
                .HasKey(bb => new { bb.BorrowerId, bb.BookId });
            //Doing the Book Part
            modelBuilder.Entity<BookBorrower>()
                .HasOne(b => b.Book)
                .WithMany(bb => bb.BookBorrowers)
                .HasForeignKey(b => b.BookId);
            //Doing the Borrower Part
            modelBuilder.Entity<BookBorrower>()
                .HasOne(b => b.Borrower)
                .WithMany(bb => bb.BookBorrowers)
                .HasForeignKey(b => b.BorrowerId);

            // Configure the relationship between Author and AppUser
            modelBuilder.Entity<Author>()
                .HasOne(a => a.AppUser)
                .WithMany() // Assuming AppUser has no navigation property to Author
                .HasForeignKey(a => a.AppUserId)
                .OnDelete(DeleteBehavior.Cascade); // Adjust the delete behavior as needed

            // Configure the relationship between Borrower and AppUser
            modelBuilder.Entity<Borrower>()
                .HasOne(b => b.AppUser)
                .WithMany() // Assuming AppUser has no navigation property to Borrower
                .HasForeignKey(b => b.AppUserId)
                .OnDelete(DeleteBehavior.Cascade); // Adjust the delete behavior as needed



        }






    }
}

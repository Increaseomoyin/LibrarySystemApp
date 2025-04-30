using AutoMapper;
using LibrarySystemApp.DTO.Author;
using LibrarySystemApp.DTO.Book;
using LibrarySystemApp.DTO.Borrower;
using LibrarySystemApp.DTO.Category;
using LibrarySystemApp.Models;

namespace LibrarySystemApp.Helper
{
    public class MappingProfile :Profile
    {
        public MappingProfile()
        {  
            //Book Mapping
            CreateMap<Book, BookDto>();
            CreateMap<BookDto, Book>();
            CreateMap<CreateBookDto, Book>();
            CreateMap<Book, CreateBookDto>();
            CreateMap<Book, UpdateBookDto>();
            CreateMap<UpdateBookDto, Book>();
            CreateMap<DeleteBookDto, Book>();
            CreateMap<Book, DeleteBookDto>();


            //Category Mapping
            CreateMap<Category, CategoryDto>();
            CreateMap<Category, CategoryDto>();
            CreateMap<CreateCategoryDto,Category>();
            CreateMap<Category, CreateCategoryDto>();
            CreateMap<Category, UpdateCategoryDto>();
            CreateMap<UpdateCategoryDto,Category>();
            CreateMap<DeleteCategoryDto,Category>();
            CreateMap<Category, DeleteCategoryDto>();


            //Author Mapping
            CreateMap<Author, AuthorDto>();
            CreateMap<AuthorDto, Author>();
            CreateMap<CreateAuthorDto, Author>();
            CreateMap<Author, CreateAuthorDto>();
            CreateMap<Author, UpdateAuthorDto>();
            CreateMap<UpdateAuthorDto, Author>();
            CreateMap<DeleteAuthorDto, Author>();
            CreateMap<Author, DeleteAuthorDto>();


            //Borrower Mapping
            CreateMap<Borrower, BorrowerDto>();
            CreateMap<BorrowerDto, Borrower>();
            CreateMap<CreateBorrowerDto, Borrower>();
            CreateMap<Borrower, CreateBorrowerDto>();
            CreateMap<Borrower, UpdateBorrowerDto>();
            CreateMap<UpdateBorrowerDto, Borrower>();
            CreateMap<DeleteBorrowerDto, Borrower>();
            CreateMap<Borrower, DeleteBorrowerDto>();


        }
    }
}

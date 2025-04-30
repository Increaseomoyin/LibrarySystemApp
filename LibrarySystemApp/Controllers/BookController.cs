using AutoMapper;
using LibrarySystemApp.DTO.Book;
using LibrarySystemApp.Interfaces;
using LibrarySystemApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace LibrarySystemApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController :ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMemoryCache _memoryCache;

        public BookController(IBookRepository bookRepository, IMapper mapper, ICategoryRepository categoryRepository, IMemoryCache memoryCache)
        {   
            //Another Comment
            _bookRepository = bookRepository;
            _mapper = mapper;
           _categoryRepository = categoryRepository;
            _memoryCache = memoryCache;
        }
        //GET REQUESTS
        //GET ALL BOOKS
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<BookDto>))]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetBooks()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var cacheKey = $"All-Books";
            if(!_memoryCache.TryGetValue(cacheKey, out List<BookDto> booksMap))
            {
                var books = await _bookRepository.GetBooks();
                booksMap = _mapper.Map<List<BookDto>>(books);

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(10))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));
                _memoryCache.Set(cacheKey, booksMap, cacheOptions);
            }
           
            return  Ok(booksMap);
        }
        //GET BOOK BY ID
        [HttpGet("{bookId:int}")]
        [ProducesResponseType(200, Type = typeof(BookDto))]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetBooksById([FromRoute] int bookId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (bookId == 0)
                return BadRequest(ModelState);
            var bookExists = await _bookRepository.BookExists(bookId);
            if (!bookExists)
                return NotFound();
            var cacheKey = $"Book-{bookId}";
            if (!_memoryCache.TryGetValue(cacheKey, out BookDto bookMap))
            {
                var book = await _bookRepository.GetBookById(bookId);
                bookMap = _mapper.Map<BookDto>(book);

                var cacheOptions = new MemoryCacheEntryOptions()
                   .SetAbsoluteExpiration(TimeSpan.FromMinutes(10))
                   .SetSlidingExpiration(TimeSpan.FromMinutes(5));
                _memoryCache.Set(cacheKey, bookMap, cacheOptions);
            }
            return Ok(bookMap);
        }
        //GET BOOK BY TITLE
        [HttpGet("{bookTitle}")]
        [ProducesResponseType(200, Type = typeof(BookDto))]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetBooksByTitle([FromRoute] string bookTitle)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (bookTitle==null)
                return BadRequest(ModelState);
         

            var book = await _bookRepository.GetBookByTitle(bookTitle);
            var bookMap = _mapper.Map<BookDto>(book);
            return Ok(bookMap);
        }
        //CREATE REQUEST
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateBook([FromQuery] List<int> categoryIds,[FromBody] CreateBookDto createBook)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (createBook == null)
                return BadRequest(ModelState);
            var bookMap = _mapper.Map<Book>(createBook);
           var done =  await _bookRepository.CreateBook(categoryIds, bookMap);
            if(!done)
            {
                ModelState.AddModelError("", "Something Occured");
                return StatusCode(500, ModelState);
            }
            return Ok("Created Book");           
        }
        //UPDATE REQUEST
        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateBook([FromQuery] int bookId, [FromQuery] List<int> categoryIds, [FromBody] UpdateBookDto bookUpdate)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (bookUpdate == null)
                return BadRequest(ModelState);
            if (bookUpdate.Id != bookId)
                return BadRequest(ModelState);
            if (!await _bookRepository.BookExists(bookId))
                return NotFound();
            var bookMap = _mapper.Map<Book>(bookUpdate);
            var set = await _bookRepository.UpdateBook(categoryIds, bookMap);
            if(!set)
            {
                ModelState.AddModelError("", "Something Bad Happened");
                return StatusCode(500, ModelState);

            }
            return NoContent();

        }
    }
}


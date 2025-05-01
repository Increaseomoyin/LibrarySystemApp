using AutoMapper;
using LibrarySystemApp.DTO.Author;
using LibrarySystemApp.Interfaces;
using LibrarySystemApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace LibrarySystemApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //JUST ANOTHER COMMENT
    public class AuthorController :ControllerBase
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;

        public AuthorController(IAuthorRepository authorRepository, IMapper mapper, IDistributedCache cache)
        {
           _authorRepository = authorRepository;
            _mapper = mapper;
            _cache = cache;
        }
        //GET REQUESTS
        //GET ALL AUTHORS
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<AuthorDto>))]
        public async Task<IActionResult> GetAuthors()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var cacheKey = $"All-Authors";
            List<AuthorDto> authorsMap;

            var authorsFromCache = await _cache.GetStringAsync(cacheKey);
            if (authorsFromCache != null)
            {
                authorsMap = JsonSerializer.Deserialize<List<AuthorDto>>(authorsFromCache);
            }
            else
            {
                var authors = await _authorRepository.GetAuthors();
                authorsMap = _mapper.Map<List<AuthorDto>>(authors);
                var serializedAuthors = JsonSerializer.Serialize(authorsMap);

                var cacheOptions = new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                    SlidingExpiration = TimeSpan.FromMinutes(5)
                };

                
                await _cache.SetStringAsync(cacheKey, serializedAuthors, cacheOptions);
               

            }
            return Ok(authorsMap);


        }
        //GET AUTHOR BY ID
        [HttpGet("{authorId}")]
        [ProducesResponseType(200, Type = typeof(AuthorDto))]
        public async Task<IActionResult> GetAuthorById([FromRoute] int authorId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if(authorId == 0)
                return BadRequest(ModelState);
            var cacheKey = $"Author-{authorId}";
            <Author
            if(!_memoryCache.TryGetValue(cacheKey, out AuthorDto authorMap))
            {
                var auth = await _authorRepository.AuthorExists(authorId);
                if (!auth)
                    return NotFound();
                var author = await _authorRepository.GetAuthorById(authorId);
                authorMap = _mapper.Map<AuthorDto>(author);

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(10))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));
                _memoryCache.Set(cacheKey, authorMap, cacheOptions);
            }
            
            return Ok(authorMap);
        }
        //GET AUTHOR BY Name
        [HttpGet("authorName")]
        [ProducesResponseType(200, Type = typeof(AuthorDto))]
        public async Task<IActionResult> GetAuthorById([FromQuery] string FirstName, [FromQuery] string LastName)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var author = await _authorRepository.GetAuthorByName(FirstName, LastName);
            if (author == null)
                return NotFound();
            var authorMap = _mapper.Map<AuthorDto>(author);
            return Ok(authorMap);
        }
        //CREATE REQUEST
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateAuthor([FromQuery] List<int> bookIds, CreateAuthorDto authorCreate)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (authorCreate == null)
                return BadRequest(ModelState);
            
            var authorMap = _mapper.Map<Author>(authorCreate);
            var author = await _authorRepository.CreateAuthor(bookIds, authorMap);
          
            return Ok($"{authorCreate.FirstName} {authorCreate.LastName} has been created!");
        }
        //UPDATE REQUEST
        [HttpPut]
        [Authorize(Roles ="Author")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateAuthor([FromQuery] int AuthorId,[FromQuery] List<int> bookIds, [FromBody] UpdateAuthorDto authorUpdate)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (authorUpdate == null)
                return BadRequest(ModelState);
            if(authorUpdate.Id != AuthorId)
                return BadRequest(ModelState);
            if (!await _authorRepository.AuthorExists(AuthorId))
                return NotFound();
            var authorMap = _mapper.Map<Author>(authorUpdate);
            var done = await _authorRepository.UpdateAuthor(bookIds, authorMap);
            if(!done)
            {
                ModelState.AddModelError("", "Something Happened!");
                return StatusCode(500, ModelState);
            }
            _memoryCache.Remove($"Author-{AuthorId}");
            return NoContent();
        }


    }
}

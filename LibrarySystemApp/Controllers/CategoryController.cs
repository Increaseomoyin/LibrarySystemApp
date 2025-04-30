using AutoMapper;
using LibrarySystemApp.DTO.Category;
using LibrarySystemApp.Interfaces;
using LibrarySystemApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystemApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController :ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }
        //GET REQUESTS
        //GET ALL CATEGORIES
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<CategoryDto>))]
        public async Task<IActionResult> GetCategories()
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var categories = await _categoryRepository.GetCategories();
            var categoriesMap = _mapper.Map<List<CategoryDto>>(categories);
            return Ok(categoriesMap);

        }
        //GET CATEGORY BY ID
        [HttpGet("{categoryId}")]
        [ProducesResponseType(200, Type = typeof(CategoryDto))]
        public async Task<IActionResult> GetCategoryById([FromRoute] int categoryId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if(categoryId ==0)
                return BadRequest(ModelState);
            var category = await _categoryRepository.GetCategoryById(categoryId);
            var categoryMap = _mapper.Map<CategoryDto>(category);
            return Ok(categoryMap);

        }
        //CREATE REQUESTS
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto categoryCreate)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (categoryCreate == null)
                return BadRequest(ModelState);
            var oldCategory = await _categoryRepository.GetCategories();
            var result = oldCategory.Where(c => c.Name == categoryCreate.Name).FirstOrDefault();
            if(result !=null)
            {
                ModelState.AddModelError("", "A Category with that Name already exist!");
                return StatusCode(422, ModelState);
            }
            var categoryMap = _mapper.Map<Category>(categoryCreate);

            var category = await _categoryRepository.CreateCategory(categoryMap);
            return Ok($"{categoryCreate.Name} is Created!");
        }
        //UPDATE REQUEST
        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateCategory([FromQuery] int CategoryId, [FromBody] UpdateCategoryDto categoryUpdate)
        {
            if(!ModelState.IsValid) 
                return BadRequest(ModelState);
            if (categoryUpdate == null)
                return BadRequest(ModelState);
            if(categoryUpdate.Id != CategoryId)
                return BadRequest(ModelState);
            if (!await _categoryRepository.CategoryExists(CategoryId))
                return NotFound();
            var categoryMap =_mapper.Map<Category>(categoryUpdate);
            var done = await _categoryRepository.UpdateCategory(categoryMap);
            if(!done)
            {
                ModelState.AddModelError("", "Something went wrong!");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}

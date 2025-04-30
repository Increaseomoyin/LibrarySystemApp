using AutoMapper;
using LibrarySystemApp.DTO.Author;
using LibrarySystemApp.DTO.Borrower;
using LibrarySystemApp.Interfaces;
using LibrarySystemApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystemApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowerController : ControllerBase
    {
        private readonly IBorrowerRepository _borrowerRepository;
        private readonly IMapper _mapper;

        public BorrowerController(IBorrowerRepository borrowerRepository, IMapper mapper)
        {
            _borrowerRepository = borrowerRepository;
            _mapper = mapper;
        } 
        //GET REQUESTS
        //GET ALL BORROWERS
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<BorrowerDto>))]
        public async Task<IActionResult> GetBorrowers()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var borrowers = await _borrowerRepository.GetBorrowers();
            var borrowersMap = _mapper.Map<List<BorrowerDto>>(borrowers);
            return Ok(borrowersMap);
        }
        //GET Borrower BY ID
        [HttpGet("{BorrowerId}")]
        [ProducesResponseType(200, Type = typeof(BorrowerDto))]
        public async Task<IActionResult> GetBorrowerById([FromRoute] int borrowerId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (borrowerId == 0)
                return BadRequest(ModelState);
            var auth = await _borrowerRepository.BorrowerExists(borrowerId);
            if (!auth)
                return NotFound();
            var borrower = await _borrowerRepository.GetBorrowerById(borrowerId);
            var borrowerMap = _mapper.Map<BorrowerDto>(borrower);
            return Ok(borrowerMap);
        }
        //GET AUTHOR BY Name
        [HttpGet("borrowerName")]
        [ProducesResponseType(200, Type = typeof(BorrowerDto))]
        public async Task<IActionResult> GetBorrowerById([FromQuery] string FirstName, [FromQuery] string LastName)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var borrower = await _borrowerRepository.GetBorrowerByName(FirstName, LastName);
            if (borrower == null)
                return NotFound();
            var BorrowerMap = _mapper.Map<BorrowerDto>(borrower);
            return Ok(BorrowerMap);
        }
        //CREATE REQUEST
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult > CreateBorrower([FromQuery] List<int> id, [FromBody] CreateBorrowerDto borrowerCreate)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            if (borrowerCreate == null)
                return BadRequest(ModelState);
            var borrowerMap = _mapper.Map<Borrower>(borrowerCreate);
            var done = await _borrowerRepository.CreateBorrower(id, borrowerMap);
            if(!done)
            {
                ModelState.AddModelError("", "Something Went Wrong!");
                return StatusCode(500, ModelState);
            }
            return Ok($"{borrowerCreate.FirstName} {borrowerCreate.LastName} has been created!");

        }
        ///UPDATE REQUEST
        [HttpPut]
        [Authorize(Roles = "Borrower")]

        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateBorrower([FromQuery] int BorrowerId, [FromQuery] List<int> bookIds, [FromBody] UpdateBorrowerDto borrowerUpdate)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (borrowerUpdate == null)
                return BadRequest(ModelState);
            if (borrowerUpdate.Id != BorrowerId)
                return BadRequest(ModelState);
            if (!await _borrowerRepository.BorrowerExists(BorrowerId))
                return NotFound();
            var borrowerMap = _mapper.Map<Borrower>(borrowerUpdate);
            var done = await _borrowerRepository.UpdateBorrower(bookIds, borrowerMap);
            return NoContent();
        }
    }
    
}

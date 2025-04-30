using LibrarySystemApp.DTO.Account;
using LibrarySystemApp.Interfaces;
using LibrarySystemApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;

namespace LibrarySystemApp.Controllers.Account
{
    [Route("api/[controller]")]
    [ApiController] 
    public class AccountController :ControllerBase
    {   
        //TESTING 1 ADDITION
        //COMMENTS JUST TO TEST OUT THE GIT
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILogger<AppUser> _logger;

        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager, ILogger<AppUser> logger)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _logger = logger;
        }//Login
        [HttpPost("Login")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> LoginUser([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (loginDto == null)
                return BadRequest(ModelState);
            var user = await _userManager.FindByNameAsync(loginDto.UserName);
            if(user == null)
                return NotFound();
            var signedin = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if(signedin.Succeeded)
            {
                var toks = await _tokenService.CreateToken(user);
                var display = new DisplayDto()
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    AppUserId = user.Id,
                    Token = toks
                };
                return Ok(display);
            }
            return BadRequest(ModelState);

        }

        //Register
        [HttpPost("Register")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterDto registerDto)
        {   
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (registerDto == null)
                return BadRequest(ModelState);

            var userExists = await _userManager.FindByNameAsync(registerDto.UserName);
            if(userExists==null)
            {
                var appUser = new AppUser()
                {
                    UserName = registerDto.UserName,
                    Email = registerDto.Email,
                };

                var createdUser = await _userManager.CreateAsync(appUser , registerDto.Password);
                if(!createdUser.Succeeded)
                {   _logger.LogInformation($"{appUser.UserName} has just been registered at {DateTime.Now}");
                    return StatusCode(500, createdUser.Errors);
                }
            }
            else
            {
                ModelState.AddModelError("", "User Already Exist!");
                return StatusCode(422, ModelState);
            }
            return Ok($"Account for {registerDto.UserName} created!");

        }
    }
}

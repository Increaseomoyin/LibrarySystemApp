using LibrarySystemApp.Models;

namespace LibrarySystemApp.Interfaces
{
    public interface ITokenService
    {
        public Task<string> CreateToken(AppUser appUser);
    }
}

using library.Models;
using library.ViewModels.Account;
namespace library.Services.Interfaces
{
    public interface IAuthService
    {
        public Task<AuthResult> SignInAsync(string username, string password);
    }
}

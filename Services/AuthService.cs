using library.Services.Interfaces;
using library.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using library.Models;
namespace library.Services
{
    public class AuthService:IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher<User> _hasher;
        public AuthService(AppDbContext context, IPasswordHasher<User> hasher)
        {
            _context = context;
            _hasher = hasher;
        }
        public async  Task<AuthResult> SignInAsync(string username, string password)
        {
            username = username.Trim();
            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserName == username);
            if (user == null) return AuthResult.Fail("The username or password is incorrect");

            var hash = _hasher.VerifyHashedPassword(user, user.PasswordHash, password);
            if (hash==PasswordVerificationResult.Failed)
               return AuthResult.Fail("The username or password is incorrect");

            if (user.Stat != "Active")
                return AuthResult.Fail("The user is not active");
            
            return AuthResult.Success(userid: user.UserId, roleid: user.RoleId.Value,
                    username: user.UserName);
        }
        
    }
}

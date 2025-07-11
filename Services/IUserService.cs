using S186Statements.Web.Models;

namespace S186Statements.Web.Services
{
    public interface IUserService
    {
        Task<User> EnsureUserExistsAsync(string email, string firstName, string lastName, string entraId);
        Task<User?> GetUserByEmailAsync(string email);
    }
}
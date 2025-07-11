using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using S186Statements.Web.Services;

namespace S186Statements.Web.Controllers
{
    [Authorize]
    public class SettingsController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger<SettingsController> _logger;

        public SettingsController(IUserService userService, ILogger<SettingsController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                // Get the current user's email from claims
                var email = User.FindFirst("preferred_username")?.Value
                    ?? User.FindFirst("email")?.Value
                    ?? User.FindFirst("upn")?.Value;

                if (string.IsNullOrEmpty(email))
                {
                    _logger.LogWarning("Email claim not found for user");
                    return Forbid();
                }

                // Get the user from the database to check their role
                var user = await _userService.GetUserByEmailAsync(email);

                if (user == null)
                {
                    _logger.LogWarning("User not found in database: {Email}", email);
                    return Forbid();
                }

                // Check if user is an administrator
                if (user.IsAdministrator != true)
                {
                    _logger.LogWarning("User {Email} attempted to access settings without administrator role", email);
                    return Forbid();
                }

                _logger.LogInformation("Administrator {Email} accessed settings page", email);
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking administrator access for settings");
                return Forbid();
            }
        }
    }
}
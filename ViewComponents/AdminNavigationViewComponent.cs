using Microsoft.AspNetCore.Mvc;
using S186Statements.Web.Services;
using System.Security.Claims;

namespace S186Statements.Web.ViewComponents
{
    public class AdminNavigationViewComponent : ViewComponent
    {
        private readonly IUserService _userService;
        private readonly ILogger<AdminNavigationViewComponent> _logger;

        public AdminNavigationViewComponent(IUserService userService, ILogger<AdminNavigationViewComponent> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                // Check if user is authenticated
                if (!User.Identity?.IsAuthenticated == true)
                {
                    return View(false);
                }

                // Get the current user's email from claims
                var claimsPrincipal = User as ClaimsPrincipal;
                var email = claimsPrincipal?.FindFirst("preferred_username")?.Value
                    ?? claimsPrincipal?.FindFirst("email")?.Value
                    ?? claimsPrincipal?.FindFirst("upn")?.Value;

                if (string.IsNullOrEmpty(email))
                {
                    _logger.LogWarning("Email claim not found for user in AdminNavigationViewComponent");
                    return View(false);
                }

                // Get the user from the database to check their role
                var user = await _userService.GetUserByEmailAsync(email);

                if (user == null)
                {
                    _logger.LogWarning("User not found in database in AdminNavigationViewComponent: {Email}", email);
                    return View(false);
                }

                // Check if user is an administrator
                var isAdministrator = user.IsAdministrator == true;

                if (isAdministrator)
                {
                    _logger.LogDebug("User {Email} has administrator access", email);
                }

                return View(isAdministrator);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking administrator access in AdminNavigationViewComponent");
                return View(false);
            }
        }
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using S186Statements.Web.Models;
using S186Statements.Web.Services;
using System.Diagnostics;
using System.Security.Claims;

namespace S186Statements.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IStrapiApiService _strapiApiService;
        private readonly IUserService _userService;

        public HomeController(ILogger<HomeController> logger, IStrapiApiService strapiApiService, IUserService userService)
        {
            _logger = logger;
            _strapiApiService = strapiApiService;
            _userService = userService;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Index action called. User authenticated: {IsAuthenticated}", User.Identity?.IsAuthenticated);

            // Check if user is authenticated
            if (User.Identity?.IsAuthenticated == true)
            {
                try
                {
                    // Log all claims for debugging
                    _logger.LogInformation("User claims count: {ClaimsCount}", User.Claims.Count());
                    foreach (var claim in User.Claims)
                    {
                        _logger.LogInformation("Claim: {Type} = {Value}", claim.Type, claim.Value);
                    }

                    // Extract claims using the same pattern as the working codebase
                    var emailClaim = User.FindFirst("preferred_username")?.Value
                        ?? User.FindFirst("email")?.Value
                        ?? User.FindFirst("upn")?.Value
                        ?? User.FindFirst(ClaimTypes.Email)?.Value;

                    var nameClaim = User.FindFirst("name")?.Value
                        ?? User.FindFirst("display_name")?.Value;

                    var objectIdentifierClaim = User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value
                        ?? User.FindFirst("oid")?.Value
                        ?? User.FindFirst("sub")?.Value
                        ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                    _logger.LogInformation("Extracted claims - Email: {Email}, Name: {Name}, ObjectId: {ObjectId}",
                        emailClaim, nameClaim, objectIdentifierClaim);

                    if (string.IsNullOrEmpty(emailClaim))
                    {
                        _logger.LogWarning("Email claim not found in user identity. Available claims: {Claims}",
                            string.Join(", ", User.Claims.Select(c => $"{c.Type}={c.Value}")));
                        return View();
                    }

                    // Parse the name claim (format: "JONES, Andy" or "Andy JONES" or "Andy")
                    string? firstName = null;
                    string? lastName = null;

                    if (!string.IsNullOrEmpty(nameClaim))
                    {
                        if (nameClaim.Contains(','))
                        {
                            // Format: "JONES, Andy"
                            var parts = nameClaim.Split(',');
                            if (parts.Length >= 2)
                            {
                                lastName = parts[0].Trim();
                                firstName = parts[1].Trim();
                            }
                        }
                        else if (nameClaim.Contains(' '))
                        {
                            // Format: "Andy JONES" or "Andy Jones"
                            var parts = nameClaim.Split(' ');
                            if (parts.Length >= 2)
                            {
                                firstName = parts[0].Trim();
                                lastName = parts[1].Trim();
                            }
                            else
                            {
                                firstName = nameClaim.Trim();
                            }
                        }
                        else
                        {
                            // Single name, treat as first name
                            firstName = nameClaim.Trim();
                        }
                    }

                    // If we still don't have a name, try to extract from email
                    if (string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(emailClaim))
                    {
                        var emailParts = emailClaim.Split('@')[0];
                        if (emailParts.Contains('.'))
                        {
                            var nameParts = emailParts.Split('.');
                            if (nameParts.Length >= 2)
                            {
                                firstName = char.ToUpper(nameParts[0][0]) + nameParts[0].Substring(1);
                                lastName = char.ToUpper(nameParts[1][0]) + nameParts[1].Substring(1);
                            }
                            else
                            {
                                firstName = char.ToUpper(emailParts[0]) + emailParts.Substring(1);
                            }
                        }
                        else
                        {
                            firstName = char.ToUpper(emailParts[0]) + emailParts.Substring(1);
                        }
                    }

                    _logger.LogInformation("Parsed name - FirstName: {FirstName}, LastName: {LastName}", firstName, lastName);

                    // Ensure user exists or update existing user
                    _logger.LogInformation("Calling EnsureUserExistsAsync for email: {Email}", emailClaim);
                    var user = await _userService.EnsureUserExistsAsync(
                        emailClaim,
                        firstName ?? "Unknown",
                        lastName ?? "User",
                        objectIdentifierClaim ?? emailClaim // Use email as fallback for EntraId
                    );
                    _logger.LogInformation("User ensured successfully. User ID: {UserId}", user?.Id);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error ensuring user exists in database");
                    // Don't throw the exception - we don't want to break the page load
                }
            }
            else
            {
                _logger.LogInformation("User is not authenticated");
            }

            return View();
        }

        [Authorize]
        public async Task<IActionResult> Dashboard()
        {
            try
            {
                var services = await _strapiApiService.GetServicesAsync();
                var issues = await _strapiApiService.GetIssuesAsync();

                var dashboardViewModel = new DashboardViewModel
                {
                    TotalServices = services.Count(),
                    TotalIssues = issues.Count(),
                    OpenIssues = issues.Count(i => i.State == IssueState.Open),
                    ClosedIssues = issues.Count(i => i.State == IssueState.Closed),
                    RecentServices = services.Take(5).ToList(),
                    RecentIssues = issues.Take(5).ToList()
                };

                return View(dashboardViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading dashboard data");
                ModelState.AddModelError("", "An error occurred while loading dashboard data.");
                return View(new DashboardViewModel());
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize]
        public async Task<IActionResult> TestStrapi()
        {
            try
            {
                // Test if we can connect to Strapi
                var services = await _strapiApiService.GetServicesAsync();
                return Json(new
                {
                    success = true,
                    message = "Strapi connection successful",
                    servicesCount = services.Count()
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Strapi connection failed",
                    error = ex.Message
                });
            }
        }

        [Authorize]
        public IActionResult DebugClaims()
        {
            var claims = User.Claims.Select(c => new { Type = c.Type, Value = c.Value }).ToList();
            var userInfo = new
            {
                IsAuthenticated = User.Identity?.IsAuthenticated,
                AuthenticationType = User.Identity?.AuthenticationType,
                Name = User.Identity?.Name,
                Claims = claims
            };

            return Json(userInfo);
        }

        [Authorize]
        public async Task<IActionResult> TestStrapiConnection()
        {
            try
            {
                // Test basic connectivity to Strapi
                using var httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri("http://localhost:1337");

                var response = await httpClient.GetAsync("/");
                var content = await response.Content.ReadAsStringAsync();

                return Json(new
                {
                    success = response.IsSuccessStatusCode,
                    statusCode = response.StatusCode,
                    content = content.Substring(0, Math.Min(200, content.Length)) + "..."
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    error = ex.Message,
                    type = ex.GetType().Name
                });
            }
        }
    }

    public class DashboardViewModel
    {
        public int TotalServices { get; set; }
        public int TotalIssues { get; set; }
        public int OpenIssues { get; set; }
        public int ClosedIssues { get; set; }
        public List<Service> RecentServices { get; set; } = new List<Service>();
        public List<Issue> RecentIssues { get; set; } = new List<Issue>();
    }
}

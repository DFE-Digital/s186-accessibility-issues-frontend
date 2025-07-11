using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using S186Statements.Web.Models;
using S186Statements.Web.Services;
using System.Diagnostics;

namespace S186Statements.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IStrapiApiService _strapiApiService;

        public HomeController(ILogger<HomeController> logger, IStrapiApiService strapiApiService)
        {
            _logger = logger;
            _strapiApiService = strapiApiService;
        }

        public IActionResult Index()
        {
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

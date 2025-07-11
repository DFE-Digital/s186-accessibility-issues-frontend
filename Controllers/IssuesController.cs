using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using S186Statements.Web.Models;
using S186Statements.Web.Services;

namespace S186Statements.Web.Controllers
{
    [Authorize]
    public class IssuesController : Controller
    {
        private readonly IStrapiApiService _strapiApiService;

        public IssuesController(IStrapiApiService strapiApiService)
        {
            _strapiApiService = strapiApiService;
        }

        // GET: Issues
        public async Task<IActionResult> Index()
        {
            try
            {
                var issues = await _strapiApiService.GetIssuesAsync();
                return View(issues);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while retrieving issues.");
                return View(new List<Issue>());
            }
        }

        // GET: Issues/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var issue = await _strapiApiService.GetIssueAsync(id);
                if (issue == null)
                {
                    return NotFound();
                }

                return View(issue);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while retrieving the issue.");
                return View();
            }
        }

        // GET: Issues/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Issues/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,State,Source,DateIdentified,PlanToFix,PlanToFixDate,ReasonForNotFixing,HowAdded,ServiceId")] Issue issue)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _strapiApiService.CreateIssueAsync(issue);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while creating the issue.");
                }
            }
            return View(issue);
        }

        // GET: Issues/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var issue = await _strapiApiService.GetIssueAsync(id);
                if (issue == null)
                {
                    return NotFound();
                }
                return View(issue);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while retrieving the issue.");
                return View();
            }
        }

        // POST: Issues/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Title,Description,State,Source,DateIdentified,PlanToFix,PlanToFixDate,ReasonForNotFixing,HowAdded,ServiceId")] Issue issue)
        {
            if (id != issue.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _strapiApiService.UpdateIssueAsync(id, issue);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while updating the issue.");
                }
            }
            return View(issue);
        }

        // GET: Issues/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var issue = await _strapiApiService.GetIssueAsync(id);
                if (issue == null)
                {
                    return NotFound();
                }

                return View(issue);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while retrieving the issue.");
                return View();
            }
        }

        // POST: Issues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _strapiApiService.DeleteIssueAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while deleting the issue.");
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
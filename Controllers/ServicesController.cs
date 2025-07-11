using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using S186Statements.Web.Models;
using S186Statements.Web.Services;

namespace S186Statements.Web.Controllers
{
    [Authorize]
    public class ServicesController : Controller
    {
        private readonly IStrapiApiService _strapiApiService;

        public ServicesController(IStrapiApiService strapiApiService)
        {
            _strapiApiService = strapiApiService;
        }

        // GET: Services
        public async Task<IActionResult> Index()
        {
            try
            {
                var services = await _strapiApiService.GetServicesAsync();
                return View(services);
            }
            catch (Exception ex)
            {
                // Log the exception
                ModelState.AddModelError("", "An error occurred while retrieving services.");
                return View(new List<Service>());
            }
        }

        // GET: Services/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var service = await _strapiApiService.GetServiceAsync(id);
                if (service == null)
                {
                    return NotFound();
                }

                return View(service);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while retrieving the service.");
                return View();
            }
        }

        // GET: Services/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Services/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ServiceId,Name,FipsId")] Service service)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _strapiApiService.CreateServiceAsync(service);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while creating the service.");
                }
            }
            return View(service);
        }

        // GET: Services/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var service = await _strapiApiService.GetServiceAsync(id);
                if (service == null)
                {
                    return NotFound();
                }
                return View(service);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while retrieving the service.");
                return View();
            }
        }

        // POST: Services/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ServiceId,Name,FipsId")] Service service)
        {
            if (id != service.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _strapiApiService.UpdateServiceAsync(id, service);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while updating the service.");
                }
            }
            return View(service);
        }

        // GET: Services/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var service = await _strapiApiService.GetServiceAsync(id);
                if (service == null)
                {
                    return NotFound();
                }

                return View(service);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while retrieving the service.");
                return View();
            }
        }

        // POST: Services/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _strapiApiService.DeleteServiceAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while deleting the service.");
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
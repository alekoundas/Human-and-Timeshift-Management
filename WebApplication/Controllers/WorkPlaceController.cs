using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataAccess;
using DataAccess.Models.Entity;
using Bussiness;
using DataAccess.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication.Controllers
{
    public class WorkPlaceController : Controller
    {
        private readonly BaseDbContext _context;
        private BaseDatawork _baseDataWork;
        public WorkPlaceController(BaseDbContext BaseDbContext, SecurityDbContext SecurityDbContext)
        {
            _context = BaseDbContext;
            _baseDataWork = new BaseDatawork(BaseDbContext);
        }

        // GET: WorkPlace
        [Authorize(Roles = "WorkPlace_View")]
        public IActionResult Index()
        {
            ViewData["Title"] = "Σύνολο πόστων";
            return View();
        }

        // GET: WorkPlace/Details/5
        [Authorize(Roles = "WorkPlace_View")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var workPlace = await _context.WorkPlaces
                .FirstOrDefaultAsync(m => m.Id == id);

            if (workPlace == null)
                return NotFound();

            ViewData["Title"] = "Προβολή πόστου";
            ViewData["EmployeeDataTable"] = "Σύνολο υπαλλήλων";

            return View(workPlace);
        }

        // GET: WorkPlace/Create
        [Authorize(Roles = "WorkPlace_Create")]
        public IActionResult Create()
        {
            ViewData["Title"] = "Προσθήκη πόστου";
            return View();
        }

        // POST: WorkPlace/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WorkPlaceCreateViewModel workPlace)
        {
            if (ModelState.IsValid)
            {
                _baseDataWork.WorkPlaces.Add(
                    WorkPlaceCreateViewModel.CreateFrom(workPlace));
                await _baseDataWork.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(workPlace);
        }

        // GET: WorkPlace/Edit/5
        [Authorize(Roles = "WorkPlace_Edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var workPlace = await _context.WorkPlaces
                .Include(x=>x.Customer)
                .FirstOrDefaultAsync(z=>z.Id==id);

            if (workPlace == null)
                return NotFound();

            ViewData["Title"] = "Επεξεργασία πόστου";
            ViewData["EmployeeDataTable"] = "Σύνολο υπαλλήλων";
            return View(workPlace);
        }

        // POST: WorkPlace/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "WorkPlace_Edit")]
        public async Task<IActionResult> Edit(int id, WorkPlace workPlace)
        {
            if (id != workPlace.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(workPlace);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkPlaceExists(workPlace.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(workPlace);
        }


        private bool WorkPlaceExists(int id)
        {
            return _context.WorkPlaces.Any(e => e.Id == id);
        }
    }
}

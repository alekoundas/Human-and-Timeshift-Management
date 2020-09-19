using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataAccess;
using DataAccess.Models.Entity;
using Microsoft.AspNetCore.Authorization;
using Bussiness;
using DataAccess.ViewModels;
using System.Globalization;

namespace WebApplication.Controllers
{
    public class TimeShiftController : Controller
    {
        private readonly BaseDbContext _context;
        private BaseDatawork _baseDataWork;
        public TimeShiftController(BaseDbContext BaseDbContext, SecurityDbContext SecurityDbContext)
        {
            _context = BaseDbContext;
            _baseDataWork = new BaseDatawork(BaseDbContext);
        }

        // GET: TimeShifts
        [Authorize(Roles = "TimeShift_View")]
        public IActionResult Index()
        {
            ViewData["Title"] = "Σύνολο χρονοδιαγραμμάτων ανα πόστο ";

            return View();
        }

        // GET: TimeShifts/Details/5
        [Authorize(Roles = "TimeShift_View")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var timeShift = await _context.TimeShifts
                .Include(t => t.WorkPlace)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (timeShift == null)
                return NotFound();

            ViewData["Title"] = "Προβολή χρονοδιαγράμματος ";
            ViewData["WorkPlaceDataTable"] = "Σύνολο υπαλλήλων πόστου";

            return View(timeShift);
        }

        // GET: TimeShifts/Create
        [Authorize(Roles = "TimeShift_Create")]
        public IActionResult Create()
        {
            ViewData["Title"] = "Προσθήκη χρονοδιαγράμματος ";
            return View();
        }

        // POST: TimeShifts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TimeShiftCreateViewModel timeShift)
        {
            if (ModelState.IsValid)
            {
                var timeShiftExists = _baseDataWork.TimeShifts
                    .Where(x => x.WorkPlaceId == timeShift.WorkPlaceId)
                    .Where(y => y.Year == timeShift.Year)
                    .Where(y => y.Month == timeShift.Month).Any();
                if (!timeShiftExists)
                {

                    var newTimeShift = TimeShiftCreateViewModel.CreateFrom(timeShift);
                    newTimeShift.Title = timeShift.Year + " " +
                        CultureInfo.CreateSpecificCulture("el-GR").DateTimeFormat.GetMonthName(timeShift.Month);

                    if (timeShift.Title?.Length > 0)
                        newTimeShift.Title = newTimeShift.Title +
                            " (" +
                            timeShift.Title +
                            ")";

                    _context.Add(newTimeShift);
                    var status = await _context.SaveChangesAsync();
                    if (status > 0)
                        TempData["StatusMessage"] = "Το χρονοδιάγραμμα δημιουργήθηκε με επιτυχία.";
                    else
                        TempData["StatusMessage"] = "Ωχ! Το χρονοδιάγραμμα Δεν δημιουργήθηκε.";
                    return RedirectToAction(nameof(Index));
                }
                TempData["StatusMessage"] = "Ωχ! Το χρονοδιάγραμμα φαίνεται να υπάρχει ήδη.";
            }
            return View(timeShift);
        }

        // GET: TimeShifts/Edit/5
        [Authorize(Roles = "TimeShift_Edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var timeShift = await _context.TimeShifts.Include(x => x.WorkPlace)
                .FirstOrDefaultAsync(z => z.Id == id);

            if (timeShift == null)
                return NotFound();

            ViewData["Title"] = "Προβολή χρονοδιαγράμματος ";
            ViewData["WorkPlaceDataTable"] = "Σύνολο υπαλλήλων πόστου";

            return View(timeShift);
        }

        // POST: TimeShifts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TimeShift timeShift)
        {
            if (id != timeShift.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(timeShift);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TimeShiftExists(timeShift.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["WorkPlaceId"] = new SelectList(_context.WorkPlaces, "Id", "Id", timeShift.WorkPlaceId);
            return View(timeShift);
        }



        private bool TimeShiftExists(int id)
        {
            return _context.TimeShifts.Any(e => e.Id == id);
        }
    }
}

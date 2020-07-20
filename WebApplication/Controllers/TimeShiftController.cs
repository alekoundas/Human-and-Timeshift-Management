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

            return View(timeShift);
        }

        // GET: TimeShifts/Create
        [Authorize(Roles = "TimeShift_Create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: TimeShifts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TimeShiftCreateViewModel timeShift)
        {
            if (ModelState.IsValid)
            {
                _context.Add(TimeShiftCreateViewModel.CreateFrom(timeShift));
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(timeShift);
        }

        // GET: TimeShifts/Edit/5
        [Authorize(Roles = "TimeShift_Edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var timeShift = await _context.TimeShifts.Include(x=>x.WorkPlace)
                .FirstOrDefaultAsync(z=>z.Id==id);
            if (timeShift == null)
            {
                return NotFound();
            }
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

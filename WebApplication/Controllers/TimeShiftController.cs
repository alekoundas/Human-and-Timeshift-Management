using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataAccess;
using DataAccess.Models.Entity.WorkTimeShift;

namespace WebApplication.Controllers
{
    public class TimeShiftController : Controller
    {
        private readonly BaseDbContext _context;

        public TimeShiftController(BaseDbContext context)
        {
            _context = context;
        }

        // GET: TimeShifts
        public async Task<IActionResult> Index()
        {
            var baseDbContext = _context.TimeShifts.Include(t => t.WorkPlace);
            return View(await baseDbContext.ToListAsync());
        }

        // GET: TimeShifts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timeShift = await _context.TimeShifts
                .Include(t => t.WorkPlace)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (timeShift == null)
            {
                return NotFound();
            }

            return View(timeShift);
        }

        // GET: TimeShifts/Create
        public IActionResult Create()
        {
            ViewData["WorkPlaceId"] = new SelectList(_context.WorkPlaces, "Id", "Id");
            return View();
        }

        // POST: TimeShifts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,StartOn,EndOn,WorkPlaceId,Id,CreatedOn")] TimeShift timeShift)
        {
            if (ModelState.IsValid)
            {
                _context.Add(timeShift);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["WorkPlaceId"] = new SelectList(_context.WorkPlaces, "Id", "Id", timeShift.WorkPlaceId);
            return View(timeShift);
        }

        // GET: TimeShifts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timeShift = await _context.TimeShifts.FindAsync(id);
            if (timeShift == null)
            {
                return NotFound();
            }
            ViewData["WorkPlaceId"] = new SelectList(_context.WorkPlaces, "Id", "Id", timeShift.WorkPlaceId);
            return View(timeShift);
        }

        // POST: TimeShifts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Title,StartOn,EndOn,WorkPlaceId,Id,CreatedOn")] TimeShift timeShift)
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

        // GET: TimeShifts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timeShift = await _context.TimeShifts
                .Include(t => t.WorkPlace)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (timeShift == null)
            {
                return NotFound();
            }

            return View(timeShift);
        }

        // POST: TimeShifts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var timeShift = await _context.TimeShifts.FindAsync(id);
            _context.TimeShifts.Remove(timeShift);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TimeShiftExists(int id)
        {
            return _context.TimeShifts.Any(e => e.Id == id);
        }
    }
}

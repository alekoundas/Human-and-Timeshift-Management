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
using DataAccess.ViewModels.RealWorkHours;

namespace WebApplication.Controllers
{
    public class RealWorkHourController : Controller
    {
        private readonly BaseDbContext _context;
        private BaseDatawork _baseDataWork;
        public RealWorkHourController(BaseDbContext BaseDbContext, SecurityDbContext SecurityDbContext)
        {
            _context = BaseDbContext;
            _baseDataWork = new BaseDatawork(BaseDbContext);
        }


        // GET: RealWorkHours
        public async Task<IActionResult> Index()
        {
            var baseDbContext = _context.RealWorkHours.Include(r => r.Employee).Include(r => r.TimeShift);
            return View(await baseDbContext.ToListAsync());
        }

        // GET: RealWorkHours/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var realWorkHour = await _context.RealWorkHours
                .Include(r => r.Employee)
                .Include(r => r.TimeShift)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (realWorkHour == null)
            {
                return NotFound();
            }

            return View(realWorkHour);
        }

        // GET: RealWorkHours/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RealWorkHours/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RealWorkHourCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                viewModel.Employees.ForEach(x =>
                {
                    var realWorkHour = RealWorkHourCreateViewModel
                        .CreateFrom(viewModel);

                    realWorkHour.EmployeeId = x.Id;
                    _baseDataWork.RealWorkHours.Add(realWorkHour);
                });
                await _baseDataWork.CompleteAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // GET: RealWorkHours/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var realWorkHour = await _context.RealWorkHours.FindAsync(id);
            if (realWorkHour == null)
                return NotFound();
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Id", realWorkHour.EmployeeId);
            ViewData["TimeShiftId"] = new SelectList(_context.TimeShifts, "Id", "Id", realWorkHour.TimeShiftId);
            return View(realWorkHour);
        }

        // POST: RealWorkHours/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StartOn,EndOn,TimeShiftId,EmployeeId,Id,CreatedOn")] RealWorkHour realWorkHour)
        {
            if (id != realWorkHour.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(realWorkHour);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RealWorkHourExists(realWorkHour.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Id", realWorkHour.EmployeeId);
            ViewData["TimeShiftId"] = new SelectList(_context.TimeShifts, "Id", "Id", realWorkHour.TimeShiftId);
            return View(realWorkHour);
        }

        // GET: RealWorkHours/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var realWorkHour = await _context.RealWorkHours
                .Include(r => r.Employee)
                .Include(r => r.TimeShift)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (realWorkHour == null)
            {
                return NotFound();
            }

            return View(realWorkHour);
        }

        // POST: RealWorkHours/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var realWorkHour = await _context.RealWorkHours.FindAsync(id);
            _context.RealWorkHours.Remove(realWorkHour);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RealWorkHourExists(int id)
        {
            return _context.RealWorkHours.Any(e => e.Id == id);
        }
    }
}

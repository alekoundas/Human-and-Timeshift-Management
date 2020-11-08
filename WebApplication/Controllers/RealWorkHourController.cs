﻿using Bussiness;
using DataAccess;
using DataAccess.Models.Entity;
using DataAccess.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

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
        [Authorize(Roles = "RealWorkHour_View")]
        public async Task<IActionResult> Index()
        {
            var baseDbContext = _context.RealWorkHours.Include(r => r.Employee).Include(r => r.TimeShift);
            ViewData["Title"] = "Σύνολο πραγματικών βαρδιών";
            ViewData["Filter"] = "Φίλτρα αναζήτησης";

            return View(await baseDbContext.ToListAsync());
        }

        // GET: RealWorkHours/Details/5
        [Authorize(Roles = "RealWorkHour_View")]

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
            ViewData["Title"] = "Σύνολο πραγματικών βαρδιών";


            return View(realWorkHour);
        }

        // GET: RealWorkHours/Create
        [Authorize(Roles = "RealWorkHour_Create")]

        public IActionResult Create()
        {
            ViewData["Title"] = "Προσθήκη πραγματικής βαρδιας";

            return View();
        }

        // POST: RealWorkHours/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RealWorkHourCreate viewModel)
        {
            var changeCount = 0;
            if (ModelState.IsValid)
            {
                viewModel.Employees.ForEach(id =>
                {
                    changeCount++;
                    var realWorkHour = RealWorkHourCreate
                        .CreateFrom(viewModel);

                    realWorkHour.EmployeeId = id;
                    _baseDataWork.RealWorkHours.Add(realWorkHour);
                });
                var state = await _baseDataWork.SaveChangesAsync();
                if (state > 0)
                    TempData["StatusMessage"] = "Aποθηκεύτηκαν με επιτυχία " +
                        changeCount +
                        " νέες πραγματικές βάρδιες";
                else
                    TempData["StatusMessage"] = "Ωχ! Οι αλλαγές ΔΕΝ αποθηκεύτηκαν.";

            }
            else
                TempData["StatusMessage"] = "Ωχ! Φαίνεται πως δεν συμπληρώθηκαν τα απαραίτητα παιδία.";

            return View();
        }

        // GET: RealWorkHours/Edit/5
        [Authorize(Roles = "RealWorkHour_Edit")]

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
                return NotFound();

            var realWorkHour = await _context.RealWorkHours
                .Include(r => r.Employee)
                .Include(r => r.TimeShift)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (realWorkHour == null)
                return NotFound();

            return View(realWorkHour);
        }


        // POST: RealWorkHours/CurrentDay
        [HttpGet, ActionName("CurrentDay")]
        public IActionResult CurrentDay()
        {
            ViewData["Title"] = "Σύνολο πραγματικών βαρδιών ημέρας";
            return View();
        }

        private bool RealWorkHourExists(int id)
        {
            return _context.RealWorkHours.Any(e => e.Id == id);
        }
    }
}

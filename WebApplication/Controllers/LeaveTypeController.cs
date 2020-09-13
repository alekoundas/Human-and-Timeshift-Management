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
using Microsoft.AspNetCore.Authorization;
using DataAccess.ViewModels.LeaveTypes;

namespace WebApplication.Controllers
{
    public class LeaveTypeController : Controller
    {
        private readonly BaseDbContext _context;
        private BaseDatawork _baseDataWork;
        public LeaveTypeController(BaseDbContext BaseDbContext, SecurityDbContext SecurityDbContext)
        {
            _context = BaseDbContext;
            _baseDataWork = new BaseDatawork(BaseDbContext);
        }

        // GET: LeaveType
        [Authorize(Roles = "LeaveType_View")]
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Σύνολο είδους αδειών";
            return View(await _context.LeaveTypes.ToListAsync());
        }

        // GET: LeaveType/Details/5
        [Authorize(Roles = "LeaveType_View")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var leaveType = await _context.LeaveTypes
                .FirstOrDefaultAsync(m => m.Id == id);

            if (leaveType == null)
                return NotFound();

            ViewData["Title"] = "Προβολή είδους άδειας";
            return View(leaveType);
        }

        // GET: LeaveType/Create
        [Authorize(Roles = "LeaveType_Create")]
        public IActionResult Create()
        {
            ViewData["Title"] = "Προσθήκη είδους άδειας";
            return View();
        }

        // POST: LeaveType/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateLeaveType leaveType)
        {
            if (ModelState.IsValid)
            {
                _baseDataWork.LeaveTypes.Add(
                    CreateLeaveType.CreateFrom(leaveType));

                await _baseDataWork.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(leaveType);
        }

        // GET: LeaveType/Edit/5
        [Authorize(Roles = "LeaveType_Edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var leaveType = await _context.LeaveTypes.FindAsync(id);
            if (leaveType == null)
                return NotFound();

            ViewData["Title"] = "Επεξεργασία είδους άδειας";

            return View(leaveType);
        }

        // POST: LeaveType/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LeaveType leaveType)
        {
            if (id != leaveType.Id)
                return NotFound();

            if (ModelState.IsValid)
            {

                try
                {
                    _context.Update(leaveType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeaveTypeExists(leaveType.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(leaveType);
        }

        private bool LeaveTypeExists(int id)
        {
            return _context.LeaveTypes.Any(e => e.Id == id);
        }
    }
}

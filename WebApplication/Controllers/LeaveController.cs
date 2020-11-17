using Bussiness;
using DataAccess;
using DataAccess.Models.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Controllers
{
    public class LeaveController : MasterController
    {
        private readonly BaseDbContext _context;
        private BaseDatawork _baseDataWork;
        public LeaveController(BaseDbContext BaseDbContext, SecurityDbContext SecurityDbContext)
        {
            _context = BaseDbContext;
            _baseDataWork = new BaseDatawork(BaseDbContext);
        }

        // GET: Leave
        [Authorize(Roles = "Leave_View")]
        public async Task<IActionResult> Index()
        {
            var baseDbContext = _context.Leaves.Include(l => l.Employee);
            ViewData["Title"] = "Σύνολο αδειών";

            return View(await baseDbContext.ToListAsync());
        }
        [Authorize(Roles = "Leave_View")]

        // GET: Leave/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var leave = await _context.Leaves
                .Include(x => x.Employee)
                .Include(x => x.LeaveType)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (leave == null)
                return NotFound();

            ViewData["Title"] = "Προβολή άδειας";

            return View(leave);
        }

        // GET: Leave/Create
        [Authorize(Roles = "Leave_Create")]
        public IActionResult Create()
        {
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Id");
            ViewData["Title"] = "Προσθήκη άδειας";

            return View();
        }

        // POST: Leave/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Leave leave)
        {
            if (ModelState.IsValid)
            {
                _context.Add(leave);

                var status = await _baseDataWork.SaveChangesAsync();
                if (status > 0)
                    TempData["StatusMessage"] = "Η άδεια " +
                    " δημιουργήθηκε με επιτυχία";
                else
                    TempData["StatusMessage"] = "Ωχ! Δεν έγινε προσθήκη νέων εγγραφών.";

                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Id", leave.EmployeeId);
            return View(leave);
        }

        // GET: Leave/Edit/5
        [Authorize(Roles = "Leave_Edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var leave = await _context.Leaves
                .Include(x => x.Employee)
                .Include(x => x.LeaveType)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (leave == null)
                return NotFound();

            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Id", leave.EmployeeId);
            ViewData["Title"] = "Επεξεργασία άδειας";

            return View(leave);
        }

        // POST: Leave/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Leave leave)
        {
            if (id != leave.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(leave);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeaveExists(leave.Id))
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
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Id", leave.EmployeeId);
            return View(leave);
        }




        private bool LeaveExists(int id)
        {
            return _context.Leaves.Any(e => e.Id == id);
        }
    }
}

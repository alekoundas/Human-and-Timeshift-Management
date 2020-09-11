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
    public class EmployeeController : Controller
    {
        private readonly BaseDbContext _context;
        private BaseDatawork _baseDataWork;
        public EmployeeController(BaseDbContext BaseDbContext, SecurityDbContext SecurityDbContext)
        {
            _context = BaseDbContext;
            _baseDataWork = new BaseDatawork(BaseDbContext);
        }

        // GET: Employee
        [Authorize(Roles ="Employee_View")]
        public IActionResult Index()
        {
            ViewData["Title"] = "Σύνολο υπαλλήλων";
            return View();
        }

        // GET: Employee/Details/5
        [Authorize(Roles = "Employee_View")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var employee = await _context.Employees
                .Include(x => x.Company)
                .Include(y => y.Contacts)
                .Include(y => y.Specialization)
                .FirstOrDefaultAsync(z => z.Id == id);

            if (employee == null)
                return NotFound();

            ViewData["Title"] = "Προβολή υπαλλήλου";
            ViewData["WorkPlaceDataTable"] = "Συμβατά πόστα προς εργασία";
            ViewData["Contacts"] = "Eπαφές υπαλλήλου";

            return View(employee);
        }

        // GET: Employee/Create
        [Authorize(Roles = "Employee_Create")]
        public IActionResult Create()
        {
            ViewData["Title"] = "Προσθήκη υπαλλήλου";
            return View();
        }

        // POST: Employee/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeCreateViewModel employee)
        {
            var contacts = new List<Contact>();
            if (ModelState.IsValid)
            {
                _baseDataWork.Employees.Add(
                    EmployeeCreateViewModel.CreateFrom(employee));
                await _baseDataWork.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Id", employee.CompanyId);
            return View(employee);
        }

        // GET: Employee/Edit/5
        [Authorize(Roles = "Employee_Edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(x => x.Company)
                .Include(x => x.Specialization)
                .Include(y => y.Contacts)
                .FirstOrDefaultAsync(z => z.Id == id);
                
            if (employee == null)
            {
                return NotFound();
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Id", employee.CompanyId);
            ViewData["Title"] = "Επεξεργασία υπαλλήλου";
            ViewData["WorkPlaceDataTable"] = "Συμβατά πόστα προς εργασία";
            ViewData["Contacts"] = "Eπαφές υπαλλήλου";

            return View(employee);
        }

        // POST: Employee/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Employee employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.Id))
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
            return View(employee);
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}

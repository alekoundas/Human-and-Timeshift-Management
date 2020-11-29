using Bussiness.Service;
using DataAccess;
using DataAccess.Models.Entity;
using DataAccess.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApplication.Controllers
{
    public class EmployeeController : Controller
    {
        private const string XlsxContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        private readonly BaseDbContext _context;
        private BaseDatawork _baseDataWork;
        public EmployeeController(BaseDbContext BaseDbContext, SecurityDbContext SecurityDbContext)
        {
            _context = BaseDbContext;
            _baseDataWork = new BaseDatawork(BaseDbContext);
        }

        // GET: Employee
        [Authorize(Roles = "Employee_View")]
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
        public async Task<IActionResult> Create(EmployeeCreate employee)
        {
            var contacts = new List<Contact>();
            if (ModelState.IsValid)
            {
                _baseDataWork.Employees.Add(
                    EmployeeCreate.CreateFrom(employee));

                var status = await _baseDataWork.SaveChangesAsync();
                if (status > 0)
                    TempData["StatusMessage"] = "Ο Υπάλληλος " +
                        employee.FirstName +
                        " - " +
                        employee.LastName +
                    " δημιουργήθηκε με επιτυχία";
                else
                    TempData["StatusMessage"] = "Ωχ! Δεν έγινε προσθήκη νέων εγγραφών.";

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
                return NotFound();

            var employee = await _context.Employees
                .Include(x => x.Company)
                .Include(x => x.Specialization)
                .Include(y => y.Contacts)
                .FirstOrDefaultAsync(z => z.Id == id);

            if (employee == null)
                return NotFound();

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
                return NotFound();

            if (ModelState.IsValid)
                try
                {
                    _context.Update(employee);
                    var zzz = await _baseDataWork.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<ActionResult> DownloadExcelTemplate()
        {
            var excelColumns = new List<string>(new string[] {
                "FirstName",
                "LastName",
                "VatNumber",
                "SocialSecurityNumber",
                "ErpCode",
                "Address",
                "HireDate",
                "ContractStartOn",
                "ContractEndOn",
                "SpecializationId",
                "ContractId",
                "IsActive",
                "CompanyId"
            });


            var excelPackage = (await (new ExcelService<Employee>(_context)
             .CreateNewExcel("Employees"))
             .AddSheetAsync(excelColumns))
             .CompleteExcel(out var errors);

            if (errors.Count == 0)
                using (var package = excelPackage)
                    return File(package.GetAsByteArray(), XlsxContentType, "Employees.xlsx");
            else
                TempData["StatusMessage"] = "Ωχ! " + string.Join("", errors);

            return View();
        }

        [HttpGet]
        public async Task<ActionResult> DownloadExcelWithData()
        {
            var excelColumns = new List<string>(new string[] {
                "FirstName",
                "LastName",
                "VatNumber",
                "SocialSecurityNumber",
                "ErpCode",
                "Email",
                "Address",
                "IsActive",
                "SpecializationId",
                "ContractId",
                "IsActive",
                "CompanyId"
            });


            var excelPackage = (await (new ExcelService<Employee>(_context)
                .CreateNewExcel("Employees"))
                .AddSheetAsync(excelColumns, "Employees"))
                .CompleteExcel(out var errors);

            if (errors.Count == 0)
                using (var package = excelPackage)
                    return File(package.GetAsByteArray(), XlsxContentType, "Employees.xlsx");
            else
                TempData["StatusMessage"] = "Ωχ! " + string.Join("", errors);

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Import(IFormFile ImportExcel)
        {
            if (ImportExcel == null)
                TempData["StatusMessage"] = "Ωχ! Φαίνεται πως δεν δόθηκε αρχείο Excel.";
            else
            {
                var employees = (await (new ExcelService<Employee>(_context)
                    .ExtractDataFromExcel(ImportExcel)))
                    .ValidateExtractedData()
                    .RetrieveExtractedData(out var errors);

                if (errors.Count == 0)
                {
                    _baseDataWork.Employees.AddRange(employees);
                    var status = await _baseDataWork.SaveChangesAsync();
                    if (status > 0)
                        TempData["StatusMessage"] = employees.Count +
                            " εγγραφές προστέθηκαν με επιτυχία";
                    else
                        TempData["StatusMessage"] = "Ωχ! Δεν έγινε προσθήκη νέων εγγραφών.";
                }
                else
                    TempData["StatusMessage"] = "Ωχ! " + string.Join("", errors);
            }

            return View("Index");
        }
    }
}

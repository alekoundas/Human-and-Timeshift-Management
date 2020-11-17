using Bussiness;
using Bussiness.Service;
using DataAccess;
using DataAccess.Models.Entity;
using DataAccess.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                    var zzz = await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.Id))
                        return NotFound();
                    else
                        throw;
                }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<ActionResult> DownloadExcelTemplate()
        {
            var errors = new List<string>();
            var excelColumns = new List<string>(new string[] {
                "FirstName",
                "LastName",
                "Afm",
                "SocialSecurityNumber",
                "ErpCode",
                "Email",
                "Address",
                "SpecializationId",
                "CompanyId" });


            var excelPackage = (await (new ExcelService(_context)
             .CreateNewExcel("Employees"))
             .AddSheetAsync<Employee>(excelColumns))
             .CompleteExcel(out errors);

            if (errors.Count == 0)
            {
                byte[] reportBytes;
                using (var package = excelPackage)
                {
                    reportBytes = package.GetAsByteArray();
                    return File(reportBytes, XlsxContentType, "Employees.xlsx");
                }
            }
            else
            {
                TempData["StatusMessage"] = "Ωχ! Φαίνεται πως εχουν πρόβλημα οι κολόνες: " +
                    string.Join("", errors);
                return View();
            }
        }

        [HttpGet]
        public async Task<ActionResult> DownloadExcelWithData()
        {
            var errors = new List<string>();
            var excelColumns = new List<string>(new string[] {
                "FirstName",
                "LastName",
                "Afm",
                "SocialSecurityNumber",
                "ErpCode",
                "Email",
                "Address",
                "SpecializationId",
                "CompanyId" });

            var employee = await _baseDataWork.Employees.GetAllAsync();

            var excelPackage = (await (new ExcelService(_context)
                .CreateNewExcel("Employees"))
                .AddSheetAsync<Employee>(excelColumns, employee))
                .CompleteExcel(out errors);

            if (errors.Count == 0)
            {
                byte[] reportBytes;
                using (var package = excelPackage)
                {
                    reportBytes = package.GetAsByteArray();
                    return File(reportBytes, XlsxContentType, "Employees.xlsx");
                }
            }
            else
            {
                TempData["StatusMessage"] = "Ωχ! Φαίνεται πως εχουν πρόβλημα οι κολόνες: " +
                    string.Join("", errors);
                return View();
            }
        }
        [HttpPost]
        public async Task<ActionResult> Import(IFormFile ImportExcel)
        {
            if (ImportExcel == null)
                TempData["StatusMessage"] = "Ωχ! Φαίνεται πως δεν δόθηκε αρχείο Excel.";
            else
                using (MemoryStream stream = new MemoryStream())
                {
                    var employees = new List<Employee>();
                    var employee = new Employee();
                    await ImportExcel.CopyToAsync(stream);
                    using (ExcelPackage excelPackage = new ExcelPackage(stream))
                    {

                        foreach (ExcelWorksheet worksheet in excelPackage.Workbook.Worksheets)
                            for (int i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
                            {
                                for (int j = worksheet.Dimension.Start.Column; j <= worksheet.Dimension.End.Column; j++)
                                    if (worksheet.Cells[1, j].Value.ToString().Contains("Id"))//Filter integers
                                        employee
                                          .GetType()
                                      .GetProperty(worksheet.Cells[1, j].Value.ToString())
                                      .SetValue(employee, Int32.Parse(worksheet.Cells[i, j].Value?.ToString()), null);
                                    else
                                        employee
                                            .GetType()
                                        .GetProperty(worksheet.Cells[1, j].Value.ToString())
                                        .SetValue(employee, worksheet.Cells[i, j].Value?.ToString(), null);

                                employee.CreatedOn = DateTime.Now;
                                employees.Add(employee);
                                employee = new Employee();
                            }
                    }
                    _baseDataWork.Employees.AddRange(employees);
                    var status = await _baseDataWork.SaveChangesAsync();
                    if (status > 0)
                        TempData["StatusMessage"] = employees.Count +
                        " εγγραφές προστέθηκαν με επιτυχία";
                    else
                        TempData["StatusMessage"] = "Ωχ! Δεν έγινε προσθήκη νέων εγγραφών.";
                }


            return View("Index");
        }
        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}

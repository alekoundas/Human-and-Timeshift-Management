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
using DataAccess.ViewModels;
using Microsoft.AspNetCore.Authorization;
using WebApplication.Utilities;
using Microsoft.AspNetCore.Http;
using System.IO;
using OfficeOpenXml;

namespace WebApplication.Controllers
{
    public class CompanyController : Controller
    {
        private const string XlsxContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        private readonly BaseDbContext _context;
        private BaseDatawork _baseDataWork;
        public CompanyController(BaseDbContext BaseDbContext, SecurityDbContext SecurityDbContext)
        {
            _context = BaseDbContext;
            _baseDataWork = new BaseDatawork(BaseDbContext);
        }

        // GET: Companies
        [Authorize(Roles = "Company_View")]
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Σύνολο εταιριών";
            return View(await _context.Companies.ToListAsync());
        }

        // GET: Companies/Details/5

        [Authorize(Roles = "Company_View")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var company = await _context.Companies
                .FirstOrDefaultAsync(m => m.Id == id);

            if (company == null)
                return NotFound();

            ViewData["Title"] = "Προβολη εταιρίας ";
            ViewData["EmployeeDataTable"] = "Σύνολο  υπαλλήλων";

            return View(company);
        }

        // GET: Companies/Create
        [Authorize(Roles = "Company_Create")]
        public IActionResult Create()
        {
            ViewData["Title"] = "Προσθήκη νέας εταιρίας ";
            return View();
        }

        // POST: Companies/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CompanyCreateViewModel company)
        {

            if (ModelState.IsValid)
            {
                _baseDataWork.Companies.Add(
                    CompanyCreateViewModel.CreateFrom(company));
                await _baseDataWork.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }

        // GET: Companies/Edit/5
        [Authorize(Roles = "Company_Edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var company = await _context.Companies.FindAsync(id);
            if (company == null)
                return NotFound();

            ViewData["Title"] = "Επεξεργασία εταιρίας ";
            ViewData["EmployeeDataTable"] = "Σύνολο  υπαλλήλων";
            return View(company);
        }

        // POST: Companies/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  Company company)
        {
            if (id != company.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(company);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyExists(company.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }

        public async Task<ActionResult> DownloadExcelTemplate()
        {
            var excelColumns = new List<string>(new string[] { "Title","Afm", "Description" });

            var excelPackage = new ExcelHelper(_context)
                .CreateNewExcel("Companies")
                .AddSheet<Company>(excelColumns)
                .CompleteExcel();


            byte[] reportBytes;
            using (var package = excelPackage)
            {
                reportBytes = package.GetAsByteArray();
            }

            return File(reportBytes, XlsxContentType, "Companies.xlsx");
        }

        public async Task<ActionResult> DownloadExcelWithData()
        {
            var excelColumns = new List<string>(new string[] { "Title", "Afm", "Description" });
            var companies = await _baseDataWork.Companies.GetAllAsync();

            var excelPackage = new ExcelHelper(_context)
                .CreateNewExcel("Companies")
                .AddSheet(excelColumns, companies)
                .CompleteExcel();


            byte[] reportBytes;
            using (var package = excelPackage)
            {
                reportBytes = package.GetAsByteArray();
            }

            return File(reportBytes, XlsxContentType, "Companies.xlsx");
        }
        public async Task<ActionResult> Import(IFormFile ImportExcel)
        {
            if (ImportExcel == null)
                TempData["StatusMessage"] = "Ωχ! Φαίνεται πως δεν δόθηκε αρχείο Excel.";
            else
                using (MemoryStream stream = new MemoryStream())
                {
                    var companies = new List<Company>();
                    var company = new Company();
                    await ImportExcel.CopyToAsync(stream);
                    using (ExcelPackage excelPackage = new ExcelPackage(stream))
                    {

                        foreach (ExcelWorksheet worksheet in excelPackage.Workbook.Worksheets)
                            for (int i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
                            {
                                for (int j = worksheet.Dimension.Start.Column; j <= worksheet.Dimension.End.Column; j++)
                                    company
                                        .GetType()
                                        .GetProperty(worksheet.Cells[1, j].Value.ToString())
                                        .SetValue(company, worksheet.Cells[i, j].Value?.ToString(), null);

                                company.CreatedOn = DateTime.Now;
                                companies.Add(company);
                                company = new Company();
                            }
                    }
                    _baseDataWork.Companies.AddRange(companies);
                    var status = await _baseDataWork.SaveChangesAsync();
                    if (status > 0)
                        TempData["StatusMessage"] = companies.Count +
                        " εγγραφές προστέθηκαν με επιτυχία";
                    else
                        TempData["StatusMessage"] = "Ωχ! Δεν έγινε προσθήκη νέων εγγραφών.";
                }


            return View("Index");
        }




        private bool CompanyExists(int id)
        {
            return _context.Companies.Any(e => e.Id == id);
        }
    }
}

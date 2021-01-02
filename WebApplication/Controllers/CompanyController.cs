using Bussiness.Service;
using DataAccess;
using DataAccess.Models.Entity;
using DataAccess.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApplication.Controllers
{
    public class CompanyController : MasterController
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
        public async Task<IActionResult> Create(CompanyCreate company)
        {
            if (ModelState.IsValid)
            {
                _baseDataWork.Companies.Add(
                    CompanyCreate.CreateFrom(company));

                var status = await _baseDataWork.SaveChangesAsync();
                if (status > 0)
                    TempData["StatusMessage"] = "H εταιρία " +
                        company.Title +
                    " δημιουργήθηκε με επιτυχία";
                else
                    TempData["StatusMessage"] = "Ωχ! Δεν έγινε προσθήκη νέων εγγραφών.";

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
            return View(CompanyEdit.CreateFrom(company));
        }

        // POST: Companies/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CompanyEdit company)
        {
            if (id != company.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(CompanyEdit.CreateFrom(company));
                    await _baseDataWork.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                }
                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }

        public async Task<ActionResult> DownloadExcelTemplate()
        {
            var excelColumns = new List<string>(new string[] {
                "Title",
                "VatNumber",
                "Description",
                 "IsActive"
            });

            var excelPackage = (await (new ExcelService<Company>(_context)
               .CreateNewExcel("Companies"))
               .AddSheetAsync(excelColumns))
               .CompleteExcel(out var errors);

            if (errors.Count == 0)
                using (var package = excelPackage)
                    return File(package.GetAsByteArray(), XlsxContentType, "Companies.xlsx");
            else
                TempData["StatusMessage"] = "Ωχ! " + string.Join("", errors);

            return View();
        }

        public async Task<ActionResult> DownloadExcelWithData()
        {
            var excelColumns = new List<string>(new string[] {
                "Title",
                "VatNumber",
                "Description",
                 "IsActive"
            });


            var excelPackage = (await (new ExcelService<Company>(_context)
             .CreateNewExcel("Companies"))
             .AddSheetAsync(excelColumns, "Companies"))
             .CompleteExcel(out var errors);

            if (errors.Count == 0)
                using (var package = excelPackage)
                    return File(package.GetAsByteArray(), XlsxContentType, "Companies.xlsx");
            else
                TempData["StatusMessage"] = "Ωχ! " + string.Join("", errors);

            return View();
        }

        public async Task<ActionResult> Import(IFormFile ImportExcel)
        {
            if (ImportExcel == null)
                TempData["StatusMessage"] = "Ωχ! Φαίνεται πως δεν δόθηκε αρχείο Excel.";
            else
            {
                var companies = (await (new ExcelService<Company>(_context)
                    .ExtractDataFromExcel(ImportExcel)))
                    .ValidateExtractedData()
                    .RetrieveExtractedData(out var errors);

                if (errors.Count == 0)
                {
                    _baseDataWork.Companies.AddRange(companies);
                    var status = await _baseDataWork.SaveChangesAsync();

                    if (status > 0)
                        TempData["StatusMessage"] = companies.Count +
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

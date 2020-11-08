using Bussiness;
using DataAccess;
using DataAccess.Models.Entity;
using DataAccess.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Utilities;

namespace WebApplication.Controllers
{
    public class WorkPlaceHourRestrictionController : Controller
    {
        private const string XlsxContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        private readonly BaseDbContext _context;
        private BaseDatawork _baseDataWork;
        public WorkPlaceHourRestrictionController(BaseDbContext BaseDbContext, SecurityDbContext SecurityDbContext)
        {
            _context = BaseDbContext;
            _baseDataWork = new BaseDatawork(BaseDbContext);
        }

        // GET: WorkPlaceHourRestriction
        [Authorize(Roles = "WorkPlaceHourRestriction_View")]
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Σύνολο περιορσμών μέγιστης εισαγωγής π.βαρδιών";
            return View();
        }

        // GET: WorkPlaceHourRestriction/Details/5

        [Authorize(Roles = "WorkPlaceHourRestriction_View")]
        public async Task<IActionResult> Details(int? id)
        {
            var includes = new List<Func<IQueryable<WorkPlaceHourRestriction>, IIncludableQueryable<WorkPlaceHourRestriction, object>>>();

            includes.Add(x => x.Include(y => y.HourRestrictions));
            includes.Add(x => x.Include(y => y.WorkPlace));

            var workPlaceHourRestriction = await _baseDataWork
                .WorkPlaceHourRestrictions
                .FirstAsync(x => x.Id == id, includes);

            if (workPlaceHourRestriction == null)
                return NotFound();

            ViewData["Title"] = "Προβολη περιορσμών μήνα ";
            ViewData["HourRestrictionDataTable"] = "Σύνολο περιορσμών μέγιστης εισαγωγής π.βαρδιών ανα μέρα";

            return View(WorkPlaceHourRestrictionEdit
                .CreateFrom(workPlaceHourRestriction));
        }

        // GET: WorkPlaceHourRestriction/Create
        [Authorize(Roles = "WorkPlaceHourRestriction_Create")]
        public IActionResult Create()
        {
            ViewData["Title"] = "Προσθήκη νέου περιορισμού ";
            return View();
        }

        // POST: WorkPlaceHourRestriction/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WorkPlaceHourRestrictionCreate workPlaceHourRestriction)
        {

            if (ModelState.IsValid)
            {
                var workPlaceRestriction = WorkPlaceHourRestrictionCreate
                    .CreateFrom(workPlaceHourRestriction);

                _baseDataWork.WorkPlaceHourRestrictions.Add(workPlaceRestriction);
                await _baseDataWork.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Title"] = "Προσθήκη νέου περιορισμού ";

            return View();
        }

        // GET: WorkPlaceHourRestriction/Edit/5
        [Authorize(Roles = "WorkPlaceHourRestriction_Edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();
            var includes = new List<Func<IQueryable<WorkPlaceHourRestriction>, IIncludableQueryable<WorkPlaceHourRestriction, object>>>();

            includes.Add(x => x.Include(y => y.HourRestrictions));
            includes.Add(x => x.Include(y => y.WorkPlace));

            var workPlaceHourRestriction = await _baseDataWork
                .WorkPlaceHourRestrictions
                .FirstAsync(x => x.Id == id, includes);

            if (workPlaceHourRestriction == null)
                return NotFound();

            ViewData["Title"] = "Επεξεργασία περιορισμού ";
            ViewData["HourRestrictionDataTable"] = "Σύνολο περιορσμών μέγιστης εισαγωγής π.βαρδιών ανα μέρα";

            return View(WorkPlaceHourRestrictionEdit
                .CreateFrom(workPlaceHourRestriction));
        }

        // POST: WorkPlaceHourRestriction/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, WorkPlaceHourRestrictionEdit workPlaceHourRestriction)
        {
            if (id != workPlaceHourRestriction.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(WorkPlaceHourRestrictionEdit
                        .CreateFrom(workPlaceHourRestriction));

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyExists(workPlaceHourRestriction.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(workPlaceHourRestriction);
        }

        public async Task<ActionResult> DownloadExcelTemplate()
        {
            var errors = new List<string>();
            var excelColumns = new List<string>(new string[] {
                "Title",
                "Afm",
                "Description" });

            var excelPackage = (await (new ExcelHelper(_context)
               .CreateNewExcel("Companies"))
               .AddSheetAsync<Company>(excelColumns))
               .CompleteExcel(out errors);

            if (errors.Count == 0)
            {
                byte[] reportBytes;
                using (var package = excelPackage)
                {
                    reportBytes = package.GetAsByteArray();
                    return File(reportBytes, XlsxContentType, "Companies.xlsx");
                }
            }
            else
            {
                TempData["StatusMessage"] = "Ωχ! Φαίνεται πως εχουν πρόβλημα οι κολόνες: " +
                    string.Join("", errors);
                return View();
            }
        }

        public async Task<ActionResult> DownloadExcelWithData()
        {
            var errors = new List<string>();
            var companies = await _baseDataWork.Companies.GetAllAsync();
            var excelColumns = new List<string>(new string[] {
                "Title",
                "Afm",
                "Description" });


            var excelPackage = (await (new ExcelHelper(_context)
             .CreateNewExcel("Companies"))
             .AddSheetAsync<Company>(excelColumns, companies))
             .CompleteExcel(out errors);

            if (errors.Count == 0)
            {
                byte[] reportBytes;
                using (var package = excelPackage)
                {
                    reportBytes = package.GetAsByteArray();
                    return File(reportBytes, XlsxContentType, "Companies.xlsx");
                }
            }
            else
            {
                TempData["StatusMessage"] = "Ωχ! Φαίνεται πως εχουν πρόβλημα οι κολόνες: " +
                    string.Join("", errors);
                return View();
            }
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

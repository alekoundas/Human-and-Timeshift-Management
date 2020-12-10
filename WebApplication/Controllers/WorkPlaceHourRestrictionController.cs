using Bussiness.Service;
using DataAccess;
using DataAccess.Models.Entity;
using DataAccess.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Controllers
{
    public class WorkPlaceHourRestrictionController : MasterController
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
        public IActionResult Index()
        {
            ViewData["Title"] = "Σύνολο περιορισμών μέγιστης εισαγωγής π.βαρδιών";
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

                var workPlaceRestrictionExists = _baseDataWork.WorkPlaceHourRestrictions
                    .Where(x => x.Year == workPlaceRestriction.Year)
                    .Where(x => x.Month == workPlaceRestriction.Month)
                    .Where(x => x.WorkPlaceId == workPlaceRestriction.WorkPlaceId)
                    .Any();

                if (!workPlaceRestrictionExists)
                {
                    _baseDataWork.WorkPlaceHourRestrictions.Add(workPlaceRestriction);
                    var status = await _baseDataWork.SaveChangesAsync();

                    if (status > 0)
                        TempData["StatusMessage"] = "O περιορισμός δημιουργήθηκε με επιτυχία.";
                    else
                        TempData["StatusMessage"] = "Ωχ! O περιορισμός Δεν δημιουργήθηκε.";
                    return RedirectToAction(nameof(Index));
                }
                TempData["StatusMessage"] = "Ωχ! O περιορισμός του επιλεγμένου μήνα φαίνεται να υπάρχει ήδη για αυτο το πόστο.";
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

                    var status = await _baseDataWork.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                }
                return RedirectToAction(nameof(Index));
            }

            workPlaceHourRestriction.WorkPlace = await _baseDataWork.WorkPlaces
                .FirstOrDefaultAsync(x => x.Id == workPlaceHourRestriction.WorkPlaceId);

            ViewData["Title"] = "Επεξεργασία περιορισμού ";
            ViewData["HourRestrictionDataTable"] = "Σύνολο περιορσμών μέγιστης εισαγωγής π.βαρδιών ανα μέρα";

            return View(workPlaceHourRestriction);
        }

        public async Task<ActionResult> DownloadExcelTemplate()
        {
            var errors = new List<string>();
            var excelColumns = new List<string>(new string[] {
                "Month",
                "Year",
                "WorkPlaceId"
            });

            var excelPackage = (await (new ExcelService<WorkPlaceHourRestriction>(_context)
               .CreateNewExcel("WorkPlaceHourRestrictions"))
               .AddSheetAsync(excelColumns))
               .CompleteExcel(out errors);

            if (errors.Count == 0)
            {
                byte[] reportBytes;
                using (var package = excelPackage)
                {
                    reportBytes = package.GetAsByteArray();
                    return File(reportBytes, XlsxContentType, "WorkPlaceHourRestrictions.xlsx");
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
            var workPlaceHourRestrictions = await _baseDataWork.WorkPlaceHourRestrictions.GetAllAsync();
            var excelColumns = new List<string>(new string[] {
              "Month",
                "Year",
                "WorkPlaceId"
            });


            var excelPackage = (await (new ExcelService<WorkPlaceHourRestriction>(_context)
             .CreateNewExcel("WorkPlaceHourRestrictions"))
             .AddSheetAsync(excelColumns, "WorkPlaceHourRestrictions"))
             .CompleteExcel(out errors);

            if (errors.Count == 0)
            {
                byte[] reportBytes;
                using (var package = excelPackage)
                {
                    reportBytes = package.GetAsByteArray();
                    return File(reportBytes, XlsxContentType, "WorkPlaceHourRestrictions.xlsx");
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
            {
                var workPlaceHourRestrictions = (await (new ExcelService<WorkPlaceHourRestriction>(_context)
                       .ExtractDataFromExcel(ImportExcel)))
                       .ValidateExtractedData()
                       .RetrieveExtractedData(out var errors);

                if (errors.Count == 0)
                {
                    _baseDataWork.WorkPlaceHourRestrictions.AddRange(workPlaceHourRestrictions);
                    var status = await _baseDataWork.SaveChangesAsync();

                    if (status > 0)
                        TempData["StatusMessage"] = workPlaceHourRestrictions.Count +
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

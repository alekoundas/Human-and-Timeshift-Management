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
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Controllers
{
    public class TimeShiftController : MasterController
    {
        private const string XlsxContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        private readonly BaseDbContext _context;
        private BaseDatawork _baseDataWork;
        public TimeShiftController(BaseDbContext BaseDbContext, SecurityDbContext SecurityDbContext)
        {
            _context = BaseDbContext;
            _baseDataWork = new BaseDatawork(BaseDbContext);
        }

        // GET: TimeShifts
        [Authorize(Roles = "TimeShift_View")]
        public IActionResult Index()
        {
            ViewData["Title"] = "Σύνολο χρονοδιαγραμμάτων ανα πόστο ";
            return View();
        }

        // GET: TimeShifts/Details/5
        [Authorize(Roles = "TimeShift_View")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var timeShift = await _context.TimeShifts
                .Include(t => t.WorkPlace)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (timeShift == null)
                return NotFound();

            ViewData["Title"] = "Προβολή χρονοδιαγράμματος ";
            ViewData["WorkPlaceDataTable"] = "Σύνολο υπαλλήλων πόστου";

            return View(timeShift);
        }

        // GET: TimeShifts/Create
        [Authorize(Roles = "TimeShift_Create")]
        public IActionResult Create()
        {
            ViewData["Title"] = "Προσθήκη χρονοδιαγράμματος ";
            return View();
        }

        // POST: TimeShifts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TimeShiftCreate timeShift)
        {
            if (ModelState.IsValid)
            {
                var timeShiftExists = _baseDataWork.TimeShifts
                    .Where(x => x.WorkPlaceId == timeShift.WorkPlaceId)
                    .Where(y => y.Year == timeShift.Year)
                    .Where(y => y.Month == timeShift.Month)
                    .Any();

                if (!timeShiftExists)
                {

                    var newTimeShift = TimeShiftCreate.CreateFrom(timeShift);
                    newTimeShift.Title = timeShift.Year + " " +
                        CultureInfo.CreateSpecificCulture("el-GR").DateTimeFormat.GetMonthName(timeShift.Month);

                    if (timeShift.Title?.Length > 0)
                        newTimeShift.Title = newTimeShift.Title +
                            " (" +
                            timeShift.Title +
                            ")";

                    _context.Add(newTimeShift);
                    var status = await _baseDataWork.SaveChangesAsync();
                    if (status > 0)
                        TempData["StatusMessage"] = "Το χρονοδιάγραμμα δημιουργήθηκε με επιτυχία.";
                    else
                        TempData["StatusMessage"] = "Ωχ! Το χρονοδιάγραμμα Δεν δημιουργήθηκε.";
                    return RedirectToAction(nameof(Index));
                }
                TempData["StatusMessage"] = "Ωχ! Το χρονοδιάγραμμα φαίνεται να υπάρχει ήδη.";
            }
            return View(timeShift);
        }

        // GET: TimeShifts/Edit/5
        [Authorize(Roles = "TimeShift_Edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var timeShift = await _context.TimeShifts.Include(x => x.WorkPlace)
                .FirstOrDefaultAsync(z => z.Id == id);

            if (timeShift == null)
                return NotFound();

            ViewData["Title"] = "Προβολή χρονοδιαγράμματος ";
            ViewData["WorkPlaceDataTable"] = "Σύνολο υπαλλήλων πόστου";

            return View(timeShift);
        }

        // POST: TimeShifts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TimeShift timeShift)
        {
            if (id != timeShift.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(timeShift);
                    await _baseDataWork.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["WorkPlaceId"] = new SelectList(_context.WorkPlaces, "Id", "Id", timeShift.WorkPlaceId);
            return View(timeShift);
        }


        // GET: TimeShifts/Amendment
        [Authorize(Roles = "TimeShiftAmendment_View")]
        public async Task<IActionResult> Amendment()
        {
            ViewData["Title"] = "Προβολή χρονοδιαγράμματος προς τροποποίηση";
            ViewData["DataTable"] = "Σύνολο υπαλλήλων πόστου";

            return View();
        }

        // GET: TimeShifts/AmendmentApprove
        [Authorize(Roles = "TimeShiftAmendmentApprove_View")]
        public async Task<IActionResult> AmendmentApprove()
        {
            ViewData["Title"] = "Προβολή τροποποιήσεων";
            ViewData["DataTable"] = "Σύνολο υπαλλήλων πόστου";

            return View();
        }

       




        [HttpGet]
        public async Task<ActionResult> DownloadExcelTemplate()
        {
            var excelColumns = new List<string>(new string[] {
                "Title",
                "Month",
                "Year",
                "WorkPlaceId",
                "IsActive"});


            var excelPackage = (await (new ExcelService<TimeShift>(_context)
             .CreateNewExcel("Employees"))
             .AddSheetAsync(excelColumns))
             .CompleteExcel(out var errors);

            if (errors.Count == 0)
                using (var package = excelPackage)
                    return File(package.GetAsByteArray(), XlsxContentType, "TimeShifts.xlsx");
            else
                TempData["StatusMessage"] = "Ωχ! " + string.Join("", errors);

            return View();
        }

        [HttpGet]
        public async Task<ActionResult> DownloadExcelWithData()
        {
            var excelColumns = new List<string>(new string[] {
                "Title",
                "Month",
                "Year",
                "WorkPlaceId",
                "IsActive"});


            var excelPackage = (await (new ExcelService<TimeShift>(_context)
                .CreateNewExcel("TimeShifts"))
                .AddSheetAsync(excelColumns, "TimeShifts"))
                .CompleteExcel(out var errors);

            if (errors.Count == 0)
                using (var package = excelPackage)
                    return File(package.GetAsByteArray(), XlsxContentType, "TimeShifts.xlsx");
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
                var timeShifts = (await (new ExcelService<TimeShift>(_context)
                    .ExtractDataFromExcel(ImportExcel)))
                    .ValidateExtractedData()
                    .RetrieveExtractedData(out var errors);

                if (errors.Count == 0)
                {
                    _baseDataWork.TimeShifts.AddRange(timeShifts);
                    var status = await _baseDataWork.SaveChangesAsync();
                    if (status > 0)
                        TempData["StatusMessage"] = timeShifts.Count +
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

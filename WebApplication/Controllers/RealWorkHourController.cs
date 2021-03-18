using Bussiness.Service;
using DataAccess;
using DataAccess.Models.Entity;
using DataAccess.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Controllers
{
    public class RealWorkHourController : MasterController
    {
        private const string XlsxContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        private readonly BaseDbContext _context;
        private BaseDatawork _baseDataWork;
        public RealWorkHourController(BaseDbContext BaseDbContext, SecurityDbContext SecurityDbContext)
        {
            _context = BaseDbContext;
            _baseDataWork = new BaseDatawork(BaseDbContext);
        }


        // GET: RealWorkHours
        [Authorize(Roles = "RealWorkHour_View")]
        public async Task<IActionResult> Index()
        {
            var baseDbContext = _context.RealWorkHours.Include(r => r.Employee).Include(r => r.TimeShift);
            ViewData["Title"] = "Σύνολο πραγματικών βαρδιών";
            ViewData["Filter"] = "Φίλτρα αναζήτησης";

            return View(await baseDbContext.ToListAsync());
        }

        // GET: RealWorkHours/Details/5
        [Authorize(Roles = "RealWorkHour_View")]

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var realWorkHour = await _context.RealWorkHours
                .Include(r => r.Employee)
                .Include(r => r.TimeShift)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (realWorkHour == null)
            {
                return NotFound();
            }
            ViewData["Title"] = "Σύνολο πραγματικών βαρδιών";


            return View(realWorkHour);
        }

        // GET: RealWorkHours/Create
        [Authorize(Roles = "RealWorkHour_Create")]

        public IActionResult Create()
        {
            ViewData["Title"] = "Προσθήκη πραγματικής βαρδιας";

            return View();
        }

        // POST: RealWorkHours/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RealWorkHourCreate viewModel)
        {
            //var timeshift = _baseDataWork.TimeShifts.Where(x => x.Id == viewModel.TimeShiftId).FirstOrDefault();
            var changeCount = 0;
            if (ModelState.IsValid)
            {
                viewModel.Employees.ForEach(id =>
                {
                    changeCount++;
                    var realWorkHour = RealWorkHourCreate
                        .CreateFrom(viewModel);

                    realWorkHour.EmployeeId = id;
                    _baseDataWork.RealWorkHours.Add(realWorkHour);
                });
                var state = await _baseDataWork.SaveChangesAsync();
                if (state > 0)
                    TempData["StatusMessage"] = "Aποθηκεύτηκαν με επιτυχία " +
                        changeCount +
                        " νέες πραγματικές βάρδιες";
                else
                    TempData["StatusMessage"] = "Ωχ! Οι αλλαγές ΔΕΝ αποθηκεύτηκαν.";

            }
            else
                TempData["StatusMessage"] = "Ωχ! Φαίνεται πως δεν συμπληρώθηκαν τα απαραίτητα παιδία.";

            return View();
        }

        // GET: RealWorkHours/Edit/5
        [Authorize(Roles = "RealWorkHour_Edit")]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var realWorkHour = await _context.RealWorkHours.FindAsync(id);
            if (realWorkHour == null)
                return NotFound();
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Id", realWorkHour.EmployeeId);
            ViewData["TimeShiftId"] = new SelectList(_context.TimeShifts, "Id", "Id", realWorkHour.TimeShiftId);
            return View(realWorkHour);
        }

        // POST: RealWorkHours/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StartOn,EndOn,TimeShiftId,EmployeeId,Id,CreatedOn")] RealWorkHour realWorkHour)
        {
            if (id != realWorkHour.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(realWorkHour);
                    await _baseDataWork.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RealWorkHourExists(realWorkHour.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Id", realWorkHour.EmployeeId);
            ViewData["TimeShiftId"] = new SelectList(_context.TimeShifts, "Id", "Id", realWorkHour.TimeShiftId);
            return View(realWorkHour);
        }

        private bool RealWorkHourExists(int id)
        {
            return _context.RealWorkHours.Any(e => e.Id == id);
        }

        public async Task<ActionResult> DownloadExcelTemplate(int? id)
        {
            var timeshift = await _baseDataWork.TimeShifts.FirstOrDefaultAsync(x => x.Id == id);
            var daysInMonth = DateTime.DaysInMonth(timeshift.Year, timeshift.Month);
            var excelColumns = new List<string>(new string[] {
                "EmployeeId",
                "Comments",
                "TimeShiftId"
            });
            for (int i = 0; i < daysInMonth; i++)
            {
                excelColumns.Add("Day_" + i);
            }

            var excelPackage = (await (new ExcelService<RealWorkHour>(_context)
                .AddLookupFilter(x => x.EmployeeWorkPlaces.Any(y => y.WorkPlace.TimeShifts.Any(z => z.Id == id)))
               .CreateNewExcel("RealWorkHours"))
               .AddSheetAsync(excelColumns))
               .CompleteExcel(out var errors);

            if (errors.Count == 0)
                using (var package = excelPackage)
                    return File(package.GetAsByteArray(), XlsxContentType, "RealWorkHours.xlsx");
            else
                TempData["StatusMessage"] = "Ωχ! " + string.Join("", errors);

            return View();
        }

        public async Task<ActionResult> DownloadExcelWithData(int? id)
        {
            var timeshift = await _baseDataWork.TimeShifts.FirstOrDefaultAsync(x => x.Id == id);
            var daysInMonth = DateTime.DaysInMonth(timeshift.Year, timeshift.Month);
            var excelColumns = new List<string>(new string[] {
                "EmployeeId",
                "Comments",
                "TimeShiftId"
            });
            for (int i = 0; i < daysInMonth; i++)
            {
                excelColumns.Add("Day_" + i);
            }

            var excelPackage = (await (new ExcelService<RealWorkHour>(_context)
                .AddLookupFilter(x => x.EmployeeWorkPlaces.Any(y => y.WorkPlace.TimeShifts.Any(z => z.Id == id)))
             .CreateNewExcel("RealWorkHours"))
             .AddSheetAsync(excelColumns, "RealWorkHours"))
             .CompleteExcel(out var errors);

            if (errors.Count == 0)
                using (var package = excelPackage)
                    return File(package.GetAsByteArray(), XlsxContentType, "RealWorkHours.xlsx");
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
                var realWorkHours = (await (new ExcelService<RealWorkHour>(_context)
                    .ExtractDataFromExcel(ImportExcel)))
                    .ValidateExtractedData()
                    .RetrieveExtractedData(out var errors);

                if (errors.Count == 0)
                {

                    _baseDataWork.RealWorkHours.RemoveRange(_baseDataWork.RealWorkHours
                        .Where(x => x.TimeShiftId == realWorkHours[0].TimeShiftId).ToList());

                    var status_delete = await _baseDataWork.SaveChangesAsync();

                    _baseDataWork.RealWorkHours.AddRange(realWorkHours);
                    var status = await _baseDataWork.SaveChangesAsync();

                    if (status > 0)
                        TempData["StatusMessage"] = realWorkHours.Count +
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

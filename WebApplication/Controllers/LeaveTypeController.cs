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
    public class LeaveTypeController : MasterController
    {
        private const string XlsxContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        private readonly BaseDbContext _context;
        private BaseDatawork _baseDataWork;
        public LeaveTypeController(BaseDbContext BaseDbContext, SecurityDbContext SecurityDbContext)
        {
            _context = BaseDbContext;
            _baseDataWork = new BaseDatawork(BaseDbContext);
        }

        // GET: LeaveType
        [Authorize(Roles = "LeaveType_View")]
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Σύνολο είδους αδειών";
            return View(await _context.LeaveTypes.ToListAsync());
        }

        // GET: LeaveType/Details/5
        [Authorize(Roles = "LeaveType_View")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var leaveType = await _context.LeaveTypes
                .FirstOrDefaultAsync(m => m.Id == id);

            if (leaveType == null)
                return NotFound();

            ViewData["Title"] = "Προβολή είδους άδειας";
            return View(leaveType);
        }

        // GET: LeaveType/Create
        [Authorize(Roles = "LeaveType_Create")]
        public IActionResult Create()
        {
            ViewData["Title"] = "Προσθήκη είδους άδειας";
            return View();
        }

        // POST: LeaveType/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LeaveTypeCreate leaveType)
        {
            if (ModelState.IsValid)
            {
                _baseDataWork.LeaveTypes.Add(
                    LeaveTypeCreate.CreateFrom(leaveType));

                var status = await _baseDataWork.SaveChangesAsync();
                if (status > 0)
                    TempData["StatusMessage"] = "Το είδος άδειας " +
                        leaveType.Name +
                    " δημιουργήθηκε με επιτυχία";
                else
                    TempData["StatusMessage"] = "Ωχ! Δεν έγινε προσθήκη νέων εγγραφών.";

                return RedirectToAction(nameof(Index));
            }
            return View(leaveType);
        }

        // GET: LeaveType/Edit/5
        [Authorize(Roles = "LeaveType_Edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var leaveType = await _context.LeaveTypes.FindAsync(id);
            if (leaveType == null)
                return NotFound();

            ViewData["Title"] = "Επεξεργασία είδους άδειας";

            return View(leaveType);
        }

        // POST: LeaveType/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LeaveType leaveType)
        {
            if (id != leaveType.Id)
                return NotFound();

            if (ModelState.IsValid)
            {

                try
                {
                    _context.Update(leaveType);
                    await _baseDataWork.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                }
                return RedirectToAction(nameof(Index));
            }
            return View(leaveType);
        }

        [HttpGet]
        public async Task<ActionResult> DownloadExcelTemplate()
        {
            var excelColumns = new List<string>(new string[] {
                "Name",
                "IsActive",
                "Description" });

            var excelPackage = (await (new ExcelService<LeaveType>(_context)
            .CreateNewExcel("LeaveTypes"))
            .AddSheetAsync(excelColumns))
            .CompleteExcel(out var errors);


            if (errors.Count == 0)
                using (var package = excelPackage)
                    return File(package.GetAsByteArray(), XlsxContentType, "LeaveTypes.xlsx");
            else
                TempData["StatusMessage"] = "Ωχ! " + string.Join("", errors);

            return View();
        }

        [HttpGet]
        public async Task<ActionResult> DownloadExcelWithData()
        {
            var excelColumns = new List<string>(new string[] {
               "Name",
               "IsActive",
                "Description" });

            var excelPackage = (await (new ExcelService<LeaveType>(_context)
           .CreateNewExcel("LeaveTypes"))
           .AddSheetAsync(excelColumns, "LeaveTypes"))
           .CompleteExcel(out var errors);

            if (errors.Count == 0)
                using (var package = excelPackage)
                    return File(package.GetAsByteArray(), XlsxContentType, "LeaveTypes.xlsx");
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
                var leaveTypes = (await (new ExcelService<LeaveType>(_context)
                    .ExtractDataFromExcel(ImportExcel)))
                    .ValidateExtractedData()
                    .RetrieveExtractedData(out var errors);

                if (errors.Count == 0)
                {
                    _baseDataWork.LeaveTypes.AddRange(leaveTypes);
                    var status = await _baseDataWork.SaveChangesAsync();
                    if (status > 0)
                        TempData["StatusMessage"] = leaveTypes.Count +
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

using Bussiness;
using Bussiness.Service;
using DataAccess;
using DataAccess.Models.Security;
using DataAccess.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApplication.Controllers.Security
{
    public class LogTypeController : MasterController
    {
        private const string XlsxContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        private readonly BaseDbContext _context;
        private readonly SecurityDataWork _securityDatawork;
        private BaseDatawork _baseDataWork;
        public LogTypeController(BaseDbContext BaseDbContext, SecurityDbContext securityDbContext)
        {
            _context = BaseDbContext;
            _baseDataWork = new BaseDatawork(BaseDbContext);
            _securityDatawork = new SecurityDataWork(securityDbContext);
        }

        // GET: LogType
        [Authorize(Roles = "LogType_View")]
        public IActionResult Index()
        {
            ViewData["Title"] = "Σύνολο είδους Log";
            return View();
        }

        // GET: LogType/Details/5
        [Authorize(Roles = "LogType_View")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var specialization = await _securityDatawork.LogTypes
                .FirstOrDefaultAsync(x => x.Id == id);

            if (specialization == null)
                return NotFound();

            ViewData["Title"] = "Προβολή είδους Log";
            return View(specialization);
        }

        // GET: LogType/Create
        [Authorize(Roles = "LogType_Create")]
        public IActionResult Create()
        {
            ViewData["Title"] = "Προσθήκη είδους Log";

            return View();
        }

        // POST: LogType/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LogTypeCreate specialization)
        {
            if (ModelState.IsValid)
            {
                _securityDatawork.LogTypes.Add(
                    LogTypeCreate.CreateFrom(specialization));

                var status = await _securityDatawork.SaveChangesAsync();
                if (status > 0)
                    TempData["StatusMessage"] = "Το είδος Log" +
                        specialization.Title +
                    " δημιουργήθηκε με επιτυχία";
                else
                    TempData["StatusMessage"] = "Ωχ! Δεν έγινε προσθήκη νέων εγγραφών.";

                return RedirectToAction(nameof(Index));
            }
            return View(specialization);
        }

        // GET: LogType/Edit/5
        [Authorize(Roles = "LogType_Edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var specialization = await _securityDatawork.LogTypes
                .FirstOrDefaultAsync(x => x.Id == id);
            if (specialization == null)
                return NotFound();

            ViewData["Title"] = "Επεξεργασία είδους Log";
            return View(specialization);
        }

        // POST: LogType/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LogType specialization)
        {
            if (id != specialization.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _securityDatawork.Update(specialization);
                    await _securityDatawork.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                }
                return RedirectToAction(nameof(Index));
            }
            return View(specialization);
        }

        //[HttpGet]
        //public async Task<ActionResult> DownloadExcelTemplate()
        //{
        //    var errors = new List<string>();
        //    var excelColumns = new List<string>(new string[] {
        //        "Title",
        //        "Title_GR",
        //        "IsActive"  });

        //    var excelPackage = (await (new ExcelService<LogType>(_context)
        //         .CreateNewExcel("LogTypes"))
        //         .AddSheetAsync(excelColumns))
        //         .CompleteExcel(out errors);


        //    if (errors.Count == 0)
        //        using (var package = excelPackage)
        //            return File(package.GetAsByteArray(), XlsxContentType, "LogTypes.xlsx");
        //    else
        //        TempData["StatusMessage"] = "Ωχ! " + string.Join("", errors);

        //    return View();
        //}

        //[HttpGet]
        //public async Task<ActionResult> DownloadExcelWithData()
        //{
        //    var excelColumns = new List<string>(new string[] {
        //       "Title",
        //        "Title_GR",
        //        "IsActive"  });

        //    var excelPackage = (await (new ExcelService<LogType>(_context)
        //        .CreateNewExcel("LogTypes"))
        //        .AddSheetAsync(excelColumns, "LogTypes"))
        //        .CompleteExcel(out var errors);

        //    if (errors.Count == 0)
        //        using (var package = excelPackage)
        //            return File(package.GetAsByteArray(), XlsxContentType, "LogTypes.xlsx");
        //    else
        //        TempData["StatusMessage"] = "Ωχ! " + string.Join("", errors);

        //    return View();
        //}

        //[HttpPost]
        //public async Task<ActionResult> Import(IFormFile ImportExcel)
        //{
        //    if (ImportExcel == null)
        //        TempData["StatusMessage"] = "Ωχ! Φαίνεται πως δεν δόθηκε αρχείο Excel.";
        //    else
        //    {
        //        var specializations = (await (new ExcelService<LogType>(_context)
        //            .ExtractDataFromExcel(ImportExcel)))
        //            .ValidateExtractedData()
        //            .RetrieveExtractedData(out var errors);
        //        if (errors.Count == 0)
        //        {
        //            _baseDataWork.LogTypes.AddRange(specializations);
        //            var status = await _baseDataWork.SaveChangesAsync();
        //            if (status > 0)
        //                TempData["StatusMessage"] = specializations.Count +
        //                " εγγραφές προστέθηκαν με επιτυχία";
        //            else
        //                TempData["StatusMessage"] = "Ωχ! Δεν έγινε προσθήκη νέων εγγραφών.";
        //        }
        //        else
        //            TempData["StatusMessage"] = "Ωχ! " + string.Join("", errors);
        //    }
        //    return View("Index");
        //}
    }
}

using Bussiness;
using DataAccess;
using DataAccess.Models.Security;
using DataAccess.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace WebApplication.Controllers.Security
{
    public class LogEntityController : MasterController
    {
        private const string XlsxContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        private readonly BaseDbContext _context;
        private readonly BaseDatawork _baseDataWork;
        private readonly SecurityDataWork _securityDatawork;

        public LogEntityController(BaseDbContext BaseDbContext, SecurityDbContext securityDbContext)
        {
            _context = BaseDbContext;
            _securityDatawork = new SecurityDataWork(securityDbContext);
            _baseDataWork = new BaseDatawork(BaseDbContext);
        }

        // GET: LogEntity
        [Authorize(Roles = "LogEntity_View")]
        public IActionResult Index()
        {
            ViewData["Title"] = "Σύνολο Log οντοτήτων";
            return View();
        }

        // GET: LogEntity/Details/5
        [Authorize(Roles = "LogEntity_View")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var specialization = await _securityDatawork.LogEntities

                .FirstOrDefaultAsync(x => x.Id == id);
            if (specialization == null)
                return NotFound();

            ViewData["Title"] = "Προβολή Log οντότητας";
            return View(specialization);
        }

        // GET: LogEntity/Create
        [Authorize(Roles = "LogEntity_Create")]
        public IActionResult Create()
        {
            ViewData["Title"] = "Προσθήκη Log οντότητα";

            return View();
        }

        // POST: LogEntity/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LogEntityCreate specialization)
        {
            if (ModelState.IsValid)
            {
                _securityDatawork.LogEntities.Add(
                    LogEntityCreate.CreateFrom(specialization));

                var status = await _securityDatawork.SaveChangesAsync();
                if (status > 0)
                    TempData["StatusMessage"] = "Η Log οντότητα" +
                        specialization.Title +
                    " δημιουργήθηκε με επιτυχία";
                else
                    TempData["StatusMessage"] = "Ωχ! Δεν έγινε προσθήκη νέων εγγραφών.";

                return RedirectToAction(nameof(Index));
            }
            return View(specialization);
        }

        // GET: LogEntity/Edit/5
        [Authorize(Roles = "LogEntity_Edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var specialization = await _securityDatawork.LogEntities
                .FirstOrDefaultAsync(x => x.Id == id);
            if (specialization == null)
                return NotFound();

            ViewData["Title"] = "Επεξεργασία Log οντότητας";
            return View(specialization);
        }

        // POST: LogEntity/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LogEntity specialization)
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

        //    var excelPackage = (await (new ExcelService<LogEntity>(_context)
        //         .CreateNewExcel("LogEntities"))
        //         .AddSheetAsync(excelColumns))
        //         .CompleteExcel(out errors);


        //    if (errors.Count == 0)
        //        using (var package = excelPackage)
        //            return File(package.GetAsByteArray(), XlsxContentType, "LogEntities.xlsx");
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

        //    var excelPackage = (await (new ExcelService<LogEntity>(_context)
        //        .CreateNewExcel("LogEntities"))
        //        .AddSheetAsync(excelColumns, "LogEntities"))
        //        .CompleteExcel(out var errors);

        //    if (errors.Count == 0)
        //        using (var package = excelPackage)
        //            return File(package.GetAsByteArray(), XlsxContentType, "LogEntities.xlsx");
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
        //        var specializations = (await (new ExcelService<LogEntity>(_context)
        //            .ExtractDataFromExcel(ImportExcel)))
        //            .ValidateExtractedData()
        //            .RetrieveExtractedData(out var errors);
        //        if (errors.Count == 0)
        //        {
        //            _baseDataWork.LogEntities.AddRange(specializations);
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

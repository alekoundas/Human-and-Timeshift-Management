using Bussiness;
using Bussiness.Service;
using DataAccess;
using DataAccess.Models.Entity;
using DataAccess.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Controllers
{
    public class WorkPlaceController : MasterController
    {
        private const string XlsxContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        private readonly BaseDbContext _context;
        private BaseDatawork _baseDataWork;
        private SecurityDataWork _securityDataWork;
        public WorkPlaceController(
            BaseDbContext BaseDbContext, 
            SecurityDbContext SecurityDbContext)
        {
            _context = BaseDbContext;
            _baseDataWork = new BaseDatawork(BaseDbContext);
            _securityDataWork = new SecurityDataWork(SecurityDbContext);
        }

        // GET: WorkPlace
        [Authorize(Roles = "WorkPlace_View")]
        public IActionResult Index()
        {
            ViewData["Title"] = "Σύνολο πόστων";
            return View();
        }

        // GET: WorkPlace/Details/5
        [Authorize(Roles = "WorkPlace_View")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var workPlace = await _context.WorkPlaces
                .Include(x => x.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (workPlace == null)
                return NotFound();

            ViewData["Title"] = "Προβολή πόστου";
            ViewData["EmployeeDataTable"] = "Σύνολο υπαλλήλων";
            ViewData["UserIds"] = _securityDataWork.ApplicationUsers
                .Query
                .Where(x => x.UserRoles.Any(y => y.Role.WorkPlaceId == id.ToString()))
                .Select(x => x.Id)
                .ToList();

            return View(workPlace);
        }

        // GET: WorkPlace/Create
        [Authorize(Roles = "WorkPlace_Create")]
        public IActionResult Create()
        {
            ViewData["Title"] = "Προσθήκη πόστου";
            return View();
        }

        // POST: WorkPlace/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WorkPlaceCreate workPlace)
        {
            if (ModelState.IsValid)
            {
                _baseDataWork.WorkPlaces.Add(
                    WorkPlaceCreate.CreateFrom(workPlace));

                var status = await _baseDataWork.SaveChangesAsync();
                if (status > 0)
                    TempData["StatusMessage"] = "Το πόστο " +
                        workPlace.Title +
                    " δημιουργήθηκε με επιτυχία";
                else
                    TempData["StatusMessage"] = "Ωχ! Δεν έγινε προσθήκη νέων εγγραφών.";

                return RedirectToAction(nameof(Index));
            }
            return View(workPlace);
        }

        // GET: WorkPlace/Edit/5
        [Authorize(Roles = "WorkPlace_Edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var workPlace = await _context.WorkPlaces
                .Include(x => x.Customer)
                .FirstOrDefaultAsync(z => z.Id == id);

            if (workPlace == null)
                return NotFound();

            ViewData["Title"] = "Επεξεργασία πόστου";
            ViewData["EmployeeDataTable"] = "Σύνολο υπαλλήλων";
            ViewData["UserIds"] = _securityDataWork.ApplicationUsers
                .Query
                .Where(x=>x.UserRoles.Any(y=>y.Role.WorkPlaceId== id.ToString()))
                .Select(x=>x.Id)
                .ToList();
            return View(WorkPlaceEdit.CreateFrom(workPlace));
        }

        // POST: WorkPlace/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "WorkPlace_Edit")]
        public async Task<IActionResult> Edit(int id, WorkPlaceEdit workPlace)
        {
            if (id != workPlace.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(WorkPlaceEdit.CreateFrom(workPlace));
                    await _baseDataWork.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {

                }
                return RedirectToAction(nameof(Index));
            }
            return View(workPlace);
        }

        [HttpGet]
        public async Task<ActionResult> DownloadExcelTemplate()
        {
            var excelColumns = new List<string>(new string[] {
                "Title",
                "Description",
                "IsActive",
                "CustomerId" });

            var excelPackage = (await (new ExcelService<WorkPlace>(_context)
                .CreateNewExcel("WorkPlaces"))
                .AddSheetAsync(excelColumns))
                .CompleteExcel(out var errors);


            if (errors.Count == 0)
                using (var package = excelPackage)
                    return File(package.GetAsByteArray(), XlsxContentType, "WorkPlaces.xlsx");
            else
                TempData["StatusMessage"] = "Ωχ! " + string.Join("", errors);

            return View();
        }

        [HttpGet]
        public async Task<ActionResult> DownloadExcelWithData()
        {
            var excelColumns = new List<string>(new string[] {
                "Title",
                "Description",
                "IsActive",
                "CustomerId"
            });

            var excelPackage = (await (new ExcelService<WorkPlace>(_context)
               .CreateNewExcel("WorkPlaces"))
               .AddSheetAsync(excelColumns, "WorkPlaces"))
               .CompleteExcel(out var errors);

            if (errors.Count == 0)
                using (var package = excelPackage)
                    return File(package.GetAsByteArray(), XlsxContentType, "WorkPlaces.xlsx");
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
                var workPlaces = (await (new ExcelService<WorkPlace>(_context)
                       .ExtractDataFromExcel(ImportExcel)))
                       .ValidateExtractedData()
                       .RetrieveExtractedData(out var errors);

                if (errors.Count == 0)
                {
                    _baseDataWork.WorkPlaces.AddRange(workPlaces);
                    var status = await _baseDataWork.SaveChangesAsync();

                    if (status > 0)
                        TempData["StatusMessage"] = workPlaces.Count +
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

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
    public class WorkPlaceController : Controller
    {
        private const string XlsxContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        private readonly BaseDbContext _context;
        private BaseDatawork _baseDataWork;
        public WorkPlaceController(BaseDbContext BaseDbContext, SecurityDbContext SecurityDbContext)
        {
            _context = BaseDbContext;
            _baseDataWork = new BaseDatawork(BaseDbContext);
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
                .FirstOrDefaultAsync(m => m.Id == id);

            if (workPlace == null)
                return NotFound();

            ViewData["Title"] = "Προβολή πόστου";
            ViewData["EmployeeDataTable"] = "Σύνολο υπαλλήλων";

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
        public async Task<IActionResult> Create(WorkPlaceCreateViewModel workPlace)
        {
            if (ModelState.IsValid)
            {
                _baseDataWork.WorkPlaces.Add(
                    WorkPlaceCreateViewModel.CreateFrom(workPlace));
                await _baseDataWork.SaveChangesAsync();
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
                .Include(x=>x.Customer)
                .FirstOrDefaultAsync(z=>z.Id==id);

            if (workPlace == null)
                return NotFound();

            ViewData["Title"] = "Επεξεργασία πόστου";
            ViewData["EmployeeDataTable"] = "Σύνολο υπαλλήλων";
            return View(workPlace);
        }

        // POST: WorkPlace/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "WorkPlace_Edit")]
        public async Task<IActionResult> Edit(int id, WorkPlace workPlace)
        {
            if (id != workPlace.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(workPlace);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkPlaceExists(workPlace.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(workPlace);
        }

        [HttpGet]
        public async Task<ActionResult> DownloadExcelTemplate()
        {
            var errors = new List<string>();
            var excelColumns = new List<string>(new string[] {
                "Title",
                "Description",
                "CustomerId" });

            var excelPackage = (await(new ExcelHelper(_context)
                .CreateNewExcel("WorkPlaces"))
                .AddSheetAsync<WorkPlace>(excelColumns))
                .CompleteExcel(out errors);

            
            if (errors.Count == 0)
            {
                byte[] reportBytes;
                using (var package = excelPackage)
                {
                    reportBytes = package.GetAsByteArray();
                    return File(reportBytes, XlsxContentType, "WorkPlaces.xlsx");
                }
            }
            else
            {
                TempData["StatusMessage"] = "Ωχ! Φαίνεται πως εχουν πρόβλημα οι κολόνες: " +
                    string.Join("", errors);
                return View();
            }
        }
        [HttpGet]
        public async Task<ActionResult> DownloadExcelWithData()
        {
            var errors = new List<string>();
            var excelColumns = new List<string>(new string[] {
                "Title",
                "Description",
                "CustomerId" });

            var workPlace = await _baseDataWork.WorkPlaces.GetAllAsync();

            var excelPackage = (await (new ExcelHelper(_context)
               .CreateNewExcel("WorkPlaces"))
               .AddSheetAsync<WorkPlace>(excelColumns, workPlace))
               .CompleteExcel(out errors);


            if (errors.Count == 0)
            {
                byte[] reportBytes;
                using (var package = excelPackage)
                {
                    reportBytes = package.GetAsByteArray();
                    return File(reportBytes, XlsxContentType, "WorkPlaces.xlsx");
                }
            }
            else
            {
                TempData["StatusMessage"] = "Ωχ! Φαίνεται πως εχουν πρόβλημα οι κολόνες: " +
                    string.Join("", errors);
                return View();
            }
        }
        [HttpPost]
        public async Task<ActionResult> Import(IFormFile ImportExcel)
        {
            if (ImportExcel == null)
                TempData["StatusMessage"] = "Ωχ! Φαίνεται πως δεν δόθηκε αρχείο Excel.";
            else
                using (MemoryStream stream = new MemoryStream())
                {
                    var workPlaces = new List<WorkPlace>();
                    var workPlace = new WorkPlace();
                    await ImportExcel.CopyToAsync(stream);
                    using (ExcelPackage excelPackage = new ExcelPackage(stream))
                    {

                        foreach (ExcelWorksheet worksheet in excelPackage.Workbook.Worksheets)
                            for (int i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
                            {
                                for (int j = worksheet.Dimension.Start.Column; j <= worksheet.Dimension.End.Column; j++)
                                    if (worksheet.Cells[1, j].Value.ToString().Contains("Id"))//Filter integers
                                        workPlace
                                          .GetType()
                                      .GetProperty(worksheet.Cells[1, j].Value.ToString())
                                      .SetValue(workPlace, Int32.Parse(worksheet.Cells[i, j].Value?.ToString()), null);
                                    else
                                        workPlace
                                            .GetType()
                                        .GetProperty(worksheet.Cells[1, j].Value.ToString())
                                        .SetValue(workPlace, worksheet.Cells[i, j].Value?.ToString(), null);

                                workPlace.CreatedOn = DateTime.Now;
                                workPlaces.Add(workPlace);
                                workPlace = new WorkPlace();
                            }
                    }
                    _baseDataWork.WorkPlaces.AddRange(workPlaces);
                    var status = await _baseDataWork.SaveChangesAsync();
                    if (status > 0)
                        TempData["StatusMessage"] = workPlaces.Count +
                        " εγγραφές προστέθηκαν με επιτυχία";
                    else
                        TempData["StatusMessage"] = "Ωχ! Δεν έγινε προσθήκη νέων εγγραφών.";
                }


            return View("Index");
        }


        private bool WorkPlaceExists(int id)
        {
            return _context.WorkPlaces.Any(e => e.Id == id);
        }
    }
}

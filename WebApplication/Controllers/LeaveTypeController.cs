using Bussiness;
using DataAccess;
using DataAccess.Models.Entity;
using DataAccess.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Utilities;

namespace WebApplication.Controllers
{
    public class LeaveTypeController : Controller
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
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeaveTypeExists(leaveType.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(leaveType);
        }

        [HttpGet]
        public async Task<ActionResult> DownloadExcelTemplate()
        {
            var errors = new List<string>();
            var excelColumns = new List<string>(new string[] {
                "Name",
                "Description" });

            var excelPackage = (await (new ExcelHelper(_context)
            .CreateNewExcel("LeaveTypes"))
            .AddSheetAsync<LeaveType>(excelColumns))
            .CompleteExcel(out errors);


            if (errors.Count == 0)
            {
                byte[] reportBytes;
                using (var package = excelPackage)
                {
                    reportBytes = package.GetAsByteArray();
                    return File(reportBytes, XlsxContentType, "LeaveTypes.xlsx");
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
               "Name",
                "Description" });

            var leaveTypes = await _baseDataWork.LeaveTypes.GetAllAsync();


            var excelPackage = (await (new ExcelHelper(_context)
           .CreateNewExcel("LeaveTypes"))
           .AddSheetAsync<LeaveType>(excelColumns, leaveTypes))
           .CompleteExcel(out errors);

            if (errors.Count == 0)
            {
                byte[] reportBytes;
                using (var package = excelPackage)
                {
                    reportBytes = package.GetAsByteArray();
                    return File(reportBytes, XlsxContentType, "LeaveTypes.xlsx");
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
                    var leaveTypes = new List<LeaveType>();
                    var leaveType = new LeaveType();
                    await ImportExcel.CopyToAsync(stream);
                    using (ExcelPackage excelPackage = new ExcelPackage(stream))
                    {

                        foreach (ExcelWorksheet worksheet in excelPackage.Workbook.Worksheets)
                        {
                            for (int i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
                            {
                                for (int j = worksheet.Dimension.Start.Column; j <= worksheet.Dimension.End.Column; j++)
                                    if (worksheet.Cells[1, j].Value.ToString().Contains("Id"))//Filter integers
                                        leaveType
                                          .GetType()
                                      .GetProperty(worksheet.Cells[1, j].Value.ToString())
                                      .SetValue(leaveType, Int32.Parse(worksheet.Cells[i, j].Value?.ToString()), null);
                                    else
                                        leaveType
                                            .GetType()
                                        .GetProperty(worksheet.Cells[1, j].Value.ToString())
                                        .SetValue(leaveType, worksheet.Cells[i, j].Value?.ToString(), null);

                                leaveType.CreatedOn = DateTime.Now;
                                leaveTypes.Add(leaveType);
                                leaveType = new LeaveType();
                            }
                        }
                    }
                    _baseDataWork.LeaveTypes.AddRange(leaveTypes);
                    var status = await _baseDataWork.SaveChangesAsync();
                    if (status > 0)
                        TempData["StatusMessage"] = leaveTypes.Count +
                        " εγγραφές προστέθηκαν με επιτυχία";
                    else
                        TempData["StatusMessage"] = "Ωχ! Δεν έγινε προσθήκη νέων εγγραφών.";
                }


            return View("Index");
        }
        private bool LeaveTypeExists(int id)
        {
            return _context.LeaveTypes.Any(e => e.Id == id);
        }
    }
}

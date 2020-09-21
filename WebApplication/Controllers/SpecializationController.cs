using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataAccess;
using DataAccess.Models.Entity;
using Microsoft.AspNetCore.Authorization;
using DataAccess.ViewModels;
using Bussiness;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using WebApplication.Utilities;
using OfficeOpenXml;

namespace WebApplication.Controllers
{
    public class SpecializationController : Controller
    {
        private const string XlsxContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        private readonly BaseDbContext _context;
        private BaseDatawork _baseDataWork;
        private HostingEnvironment _hostingEnvironment;
        public SpecializationController(BaseDbContext BaseDbContext, SecurityDbContext SecurityDbContext)
        {
            _context = BaseDbContext;
            _baseDataWork = new BaseDatawork(BaseDbContext);
            _hostingEnvironment = new HostingEnvironment();
        }

        // GET: Specializations
        [Authorize(Roles = "Specialization_View")]
        public IActionResult Index()
        {
            ViewData["Title"] = "Σύνολο ειδικοτήτων";
            return View();
        }

        // GET: Specializations/Details/5
        [Authorize(Roles = "Specialization_View")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var specialization = await _baseDataWork.Specializations.FindAsync((int)id);
            if (specialization == null)
                return NotFound();

            ViewData["Title"] = "Προβολή ειδικότητας";
            return View(specialization);
        }

        // GET: Specializations/Create
        [Authorize(Roles = "Specialization_Create")]
        public IActionResult Create()
        {
            ViewData["Title"] = "Προσθήκη ειδικότητας";

            return View();
        }

        // POST: Specializations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SpecializationCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                _baseDataWork.Specializations.Add(
                    SpecializationCreateViewModel.CreateFrom(viewModel));
                await _baseDataWork.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }

        // GET: Specializations/Edit/5
        [Authorize(Roles = "Specialization_Edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var specialization = await _baseDataWork.Specializations.FindAsync((int)id);
            if (specialization == null)
                return NotFound();

            ViewData["Title"] = "Επεξεργασία ειδικότητας";
            return View(specialization);
        }

        // POST: Specializations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Specialization specialization)
        {
            if (id != specialization.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _baseDataWork.Update(specialization);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SpecializationExists(specialization.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(specialization);
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
                 .AddSheetAsync<Specialization>(excelColumns))
                 .CompleteExcel(out errors);


            if (errors.Count == 0)
            {
                byte[] reportBytes;
                using (var package = excelPackage)
                {
                    reportBytes = package.GetAsByteArray();
                    return File(reportBytes, XlsxContentType, "Specializations.xlsx");
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
            var specialization = await _baseDataWork.Specializations.GetAllAsync();
            var excelColumns = new List<string>(new string[] { 
                "Name",
                "Description" });

            var excelPackage = (await (new ExcelHelper(_context)
                .CreateNewExcel("LeaveTypes"))
                .AddSheetAsync<Specialization>(excelColumns, specialization))
                .CompleteExcel(out errors);

            if (errors.Count == 0)
            {
                byte[] reportBytes;
                using (var package = excelPackage)
                {
                    reportBytes = package.GetAsByteArray();
                    return File(reportBytes, XlsxContentType, "Specializations.xlsx");
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
                    var specializations = new List<Specialization>();
                    var specialization = new Specialization();
                    await ImportExcel.CopyToAsync(stream);
                    using (ExcelPackage excelPackage = new ExcelPackage(stream))
                    {

                        foreach (ExcelWorksheet worksheet in excelPackage.Workbook.Worksheets)
                            for (int i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
                            {
                                for (int j = worksheet.Dimension.Start.Column; j <= worksheet.Dimension.End.Column; j++)
                                    specialization
                                        .GetType()
                                        .GetProperty(worksheet.Cells[1, j].Value.ToString())
                                        .SetValue(specialization, worksheet.Cells[i, j].Value?.ToString(), null);

                                specialization.CreatedOn = DateTime.Now;
                                specializations.Add(specialization);
                                specialization = new Specialization();
                            }
                    }
                    _baseDataWork.Specializations.AddRange(specializations);
                    var status = await _baseDataWork.SaveChangesAsync();
                    if (status > 0)
                        TempData["StatusMessage"] = specializations.Count +
                        " εγγραφές προστέθηκαν με επιτυχία";
                    else
                        TempData["StatusMessage"] = "Ωχ! Δεν έγινε προσθήκη νέων εγγραφών.";
                }


            return View("Index");
        }
        private bool SpecializationExists(int id)
        {
            return _context.Specializations.Any(e => e.Id == id);
        }
    }
}


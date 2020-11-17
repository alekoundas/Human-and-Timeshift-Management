using Bussiness;
using Bussiness.Service;
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

namespace WebApplication.Controllers
{
    public class CustomerController : MasterController
    {
        private const string XlsxContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        private readonly BaseDbContext _context;
        private BaseDatawork _baseDataWork;
        public CustomerController(BaseDbContext BaseDbContext, SecurityDbContext SecurityDbContext)
        {
            _context = BaseDbContext;
            _baseDataWork = new BaseDatawork(BaseDbContext);
        }

        // GET: Customers
        [Authorize(Roles = "Customer_View")]
        public IActionResult Index()
        {
            ViewData["Title"] = "Σύνολο πελατών";
            return View();
        }

        // GET: Customers/Details/5
        [Authorize(Roles = "Customer_View")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var customer = await _context.Customers.Include(x => x.Company)
                .FirstOrDefaultAsync(z => z.Id == id);

            if (customer == null)
                return NotFound();

            ViewData["Title"] = "Προβολή πελάτη ";
            ViewData["WorkPlaceDataTable"] = "Σύνολο πόστων";

            return View(customer);
        }

        // GET: Customers/Create
        [Authorize(Roles = "Customer_Create")]
        public IActionResult Create()
        {
            ViewData["Title"] = "Προσθήκη πελάτη ";
            return View();
        }

        // POST: Customers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CustomerCreate customer)
        {
            if (ModelState.IsValid)
            {
                _baseDataWork.Customers.Add(CustomerCreate.CreateFrom(customer));

                var status = await _baseDataWork.SaveChangesAsync();
                if (status > 0)
                    TempData["StatusMessage"] = "Ο πελάτης " +
                        customer.ΙdentifyingΝame +
                    " δημιουργήθηκε με επιτυχία";
                else
                    TempData["StatusMessage"] = "Ωχ! Δεν έγινε προσθήκη νέων εγγραφών.";

                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customers/Edit/5
        [Authorize(Roles = "Customer_Edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var customer = await _context.Customers.Include(x => x.Company)
                .FirstOrDefaultAsync(z => z.Id == id);

            if (customer == null)
                return NotFound();

            ViewData["Title"] = "Επεξεργασία πελάτη ";
            ViewData["WorkPlaceDataTable"] = "Σύνολο πόστων";

            return View(customer);
        }

        // POST: Customers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Customer customer)
        {
            if (id != customer.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        [HttpGet]
        public async Task<ActionResult> DownloadExcelTemplate()
        {
            var errors = new List<string>();
            var excelColumns = new List<string>(new string[] {
                "ΙdentifyingΝame",
                "AFM",
                "Profession",
                "Address",
                "PostalCode",
                "DOY",
                "Description",
                "CompanyId" });

            var excelPackage = (await (new ExcelService(_context)
               .CreateNewExcel("Customers"))
               .AddSheetAsync<Customer>(excelColumns))
               .CompleteExcel(out errors);


            if (errors.Count == 0)
            {
                byte[] reportBytes;
                using (var package = excelPackage)
                {
                    reportBytes = package.GetAsByteArray();
                    return File(reportBytes, XlsxContentType, "Customers.xlsx");
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
                "ΙdentifyingΝame",
                "AFM",
                "Profession",
                "Address",
                "PostalCode",
                "DOY",
                "Description",
                "CompanyId" });

            var customer = await _baseDataWork.Customers.GetAllAsync();

            var excelPackage = (await (new ExcelService(_context)
           .CreateNewExcel("Customers"))
           .AddSheetAsync<Customer>(excelColumns, customer))
           .CompleteExcel(out errors);

            if (errors.Count == 0)
            {
                byte[] reportBytes;
                using (var package = excelPackage)
                {
                    reportBytes = package.GetAsByteArray();
                    return File(reportBytes, XlsxContentType, "Customers.xlsx");
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
                    var customers = new List<Customer>();
                    var customer = new Customer();
                    await ImportExcel.CopyToAsync(stream);
                    using (ExcelPackage excelPackage = new ExcelPackage(stream))
                    {

                        foreach (ExcelWorksheet worksheet in excelPackage.Workbook.Worksheets)
                            for (int i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
                            {
                                for (int j = worksheet.Dimension.Start.Column; j <= worksheet.Dimension.End.Column; j++)
                                    if (worksheet.Cells[1, j].Value.ToString().Contains("Id"))//Filter integers
                                        customer
                                          .GetType()
                                      .GetProperty(worksheet.Cells[1, j].Value.ToString())
                                      .SetValue(customer, Int32.Parse(worksheet.Cells[i, j].Value?.ToString()), null);
                                    else
                                        customer
                                            .GetType()
                                        .GetProperty(worksheet.Cells[1, j].Value.ToString())
                                        .SetValue(customer, worksheet.Cells[i, j].Value?.ToString(), null);

                                customer.CreatedOn = DateTime.Now;
                                customers.Add(customer);
                                customer = new Customer();
                            }
                    }
                    _baseDataWork.Customers.AddRange(customers);
                    var status = await _baseDataWork.SaveChangesAsync();
                    if (status > 0)
                        TempData["StatusMessage"] = customers.Count +
                        " εγγραφές προστέθηκαν με επιτυχία";
                    else
                        TempData["StatusMessage"] = "Ωχ! Δεν έγινε προσθήκη νέων εγγραφών.";
                }


            return View("Index");
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}

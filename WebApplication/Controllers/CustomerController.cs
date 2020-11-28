using Bussiness.Service;
using DataAccess;
using DataAccess.Models.Audit;
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
                _context.EnsureAutoHistory(() => new AuditAutoHistory { });

                var status = await _baseDataWork.SaveChangesAsync();
                if (status > 0)
                    TempData["StatusMessage"] = "Ο πελάτης " +
                        customer.IdentifyingName +
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
                    _context.EnsureAutoHistory(() => new AuditAutoHistory { });
                    await _baseDataWork.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                }
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        [HttpGet]
        public async Task<ActionResult> DownloadExcelTemplate()
        {
            var excelColumns = new List<string>(new string[] {
                "IdentifyingName",
                "VatNumber",
                "Profession",
                "Address",
                "PostalCode",
                "DOY",
                "Description",
                "IsActive",
                "CompanyId"
            });

            var excelPackage = (await (new ExcelService<Customer>(_context)
               .CreateNewExcel("Customers"))
               .AddSheetAsync(excelColumns))
               .CompleteExcel(out var errors);


            if (errors.Count == 0)
                using (var package = excelPackage)
                    return File(package.GetAsByteArray(), XlsxContentType, "Customers.xlsx");
            else
                TempData["StatusMessage"] = "Ωχ! " + string.Join("", errors);

            return View();
        }

        [HttpGet]
        public async Task<ActionResult> DownloadExcelWithData()
        {
            var excelColumns = new List<string>(new string[] {
                "IdentifyingName",
                "VatNumber",
                "Profession",
                "Address",
                "PostalCode",
                "DOY",
                "Description",
                "IsActive",
                "CompanyId"
            });

            var excelPackage = (await (new ExcelService<Customer>(_context)
           .CreateNewExcel("Customers"))
           .AddSheetAsync(excelColumns, "Customers"))
           .CompleteExcel(out var errors);

            if (errors.Count == 0)
                using (var package = excelPackage)
                    return File(package.GetAsByteArray(), XlsxContentType, "Customers.xlsx");
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
                var customers = (await (new ExcelService<Customer>(_context)
                    .ExtractDataFromExcel(ImportExcel)))
                    .ValidateExtractedData()
                    .RetrieveExtractedData(out var errors);
                if (errors.Count == 0)
                {

                    _baseDataWork.Customers.AddRange(customers);
                    var status = await _baseDataWork.SaveChangesAsync();
                    if (status > 0)
                        TempData["StatusMessage"] = customers.Count +
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

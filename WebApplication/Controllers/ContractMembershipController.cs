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
    public class ContractMembershipController : MasterController
    {
        private const string XlsxContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        private readonly BaseDbContext _context;
        private BaseDatawork _baseDataWork;

        public ContractMembershipController(BaseDbContext BaseDbContext, SecurityDbContext SecurityDbContext)
        {
            _context = BaseDbContext;
            _baseDataWork = new BaseDatawork(BaseDbContext);
        }

        // GET: ContractMemberships
        [Authorize(Roles = "ContractMembership_View")]
        public ActionResult Index()
        {
            ViewData["Title"] = "Σύνολο ιδιοτήτων σύμβασης";
            return View();
        }

        // GET: ContractMemberships/Details/5
        [Authorize(Roles = "ContractMembership_View")]
        public async Task<ActionResult> Details(int id)
        {
            if (id == null)
                return NotFound();

            var ContractMembership = await _baseDataWork.ContractMemberships
                .FirstOrDefaultAsync(m => m.Id == id);

            if (ContractMembership == null)
                return NotFound();

            ViewData["Title"] = "Προβολη ιδιότητας σύμβασης ";

            return View(ContractMembership);
        }

        // GET: ContractMemberships/Create
        [Authorize(Roles = "ContractMembership_Create")]
        public ActionResult Create()
        {
            ViewData["Title"] = "Προσθήκη νέας ιδιότητας σύμβασης ";
            return View();
        }

        // POST: ContractMemberships/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ContractMembershipCreate ContractMembership)
        {
            if (ModelState.IsValid)
            {
                _baseDataWork.ContractMemberships.Add(
                    ContractMembershipCreate.CreateFrom(ContractMembership));

                var status = await _baseDataWork.SaveChangesAsync();
                if (status > 0)
                    TempData["StatusMessage"] = "Η ιδιότητα σύμβασης " +
                        ContractMembership.Name +
                    " δημιουργήθηκε με επιτυχία";
                else
                    TempData["StatusMessage"] = "Ωχ! Δεν έγινε προσθήκη νέων εγγραφών.";

                return RedirectToAction(nameof(Index));
            }
            return View(ContractMembership);
        }

        // GET: ContractMemberships/Edit/5
        [Authorize(Roles = "Company_Edit")]
        public async Task<ActionResult> Edit(int id)
        {
            if (id == null)
                return NotFound();

            var company = await _context.ContractMemberships.FindAsync(id);
            if (company == null)
                return NotFound();

            ViewData["Title"] = "Επεξεργασία ιδιότητας σύμβασης ";
            return View(company);
        }

        // POST: ContractMemberships/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, ContractMembership ContractMembership)
        {
            if (id != ContractMembership.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ContractMembership);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                }
                return RedirectToAction(nameof(Index));
            }
            return View(ContractMembership);
        }

        public async Task<ActionResult> DownloadExcelTemplate()
        {
            var excelColumns = new List<string>(new string[] {
                "Name",
                "Description",
                "IsActive"
            });

            var excelPackage = (await (new ExcelService<ContractMembership>(_context)
               .CreateNewExcel("ContractMemberships"))
               .AddSheetAsync(excelColumns))
               .CompleteExcel(out var errors);

            if (errors.Count == 0)
                using (var package = excelPackage)
                    return File(package.GetAsByteArray(), XlsxContentType, "ContractMemberships.xlsx");
            else
                TempData["StatusMessage"] = "Ωχ! " + string.Join("", errors);

            return View();
        }

        public async Task<ActionResult> DownloadExcelWithData()
        {
            var excelColumns = new List<string>(new string[] {
                "Name",
                "Description",
                "IsActive"
            });


            var excelPackage = (await (new ExcelService<ContractMembership>(_context)
             .CreateNewExcel("ContractMemberships"))
             .AddSheetAsync(excelColumns, "ContractMemberships"))
             .CompleteExcel(out var errors);

            if (errors.Count == 0)
                using (var package = excelPackage)
                    return File(package.GetAsByteArray(), XlsxContentType, "ContractMemberships.xlsx");
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
                var contractMemberships = (await (await (new ExcelService<ContractMembership>(_context)
                    .ExtractDataFromExcel(ImportExcel)))
                    .ValidateExtractedData())
                    .RetrieveExtractedData(out var errors);

                if (errors.Count == 0)
                {
                    _baseDataWork.ContractMemberships.AddRange(contractMemberships);
                    var status = await _baseDataWork.SaveChangesAsync();

                    if (status > 0)
                        TempData["StatusMessage"] = contractMemberships.Count +
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

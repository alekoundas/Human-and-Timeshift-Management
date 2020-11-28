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
    public class ContractTypeController : MasterController
    {
        private const string XlsxContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        private readonly BaseDbContext _context;
        private BaseDatawork _baseDataWork;

        public ContractTypeController(BaseDbContext BaseDbContext, SecurityDbContext SecurityDbContext)
        {
            _context = BaseDbContext;
            _baseDataWork = new BaseDatawork(BaseDbContext);
        }
        // GET: ContractTypes
        [Authorize(Roles = "ContractType_View")]
        public ActionResult Index()
        {
            ViewData["Title"] = "Σύνολο τύπου συμβάσεων";
            return View();
        }

        // GET: ContractTypes/Details/5
        [Authorize(Roles = "ContractType_View")]
        public async Task<ActionResult> Details(int id)
        {
            if (id == 0)
                return NotFound();

            var contractType = await _baseDataWork.ContractTypes
                .FirstOrDefaultAsync(m => m.Id == id);

            if (contractType == null)
                return NotFound();

            ViewData["Title"] = "Προβολη τύπου σύμβασης ";

            return View(contractType);
        }

        // GET: ContractTypes/Create
        [Authorize(Roles = "ContractType_Create")]
        public ActionResult Create()
        {
            ViewData["Title"] = "Προσθήκη νέου τύπου σύμβασης ";
            return View();
        }

        // POST: ContractTypes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ContractTypeCreate contractType)
        {
            if (ModelState.IsValid)
            {
                var newContractType = ContractTypeCreate.CreateFrom(contractType);
                _baseDataWork.ContractTypes.Add(newContractType);

                var status = await _baseDataWork.SaveChangesAsync();
                if (status > 0)
                    TempData["StatusMessage"] = "Ο τύπος σύμβασης " +
                        contractType.Name +
                    " δημιουργήθηκε με επιτυχία";
                else
                    TempData["StatusMessage"] = "Ωχ! Δεν έγινε προσθήκη νέων εγγραφών.";

                return RedirectToAction(nameof(Index));
            }
            return View(contractType);
        }

        // GET: ContractTypes/Edit/5
        [Authorize(Roles = "Company_Edit")]
        public async Task<ActionResult> Edit(int id)
        {
            if (id == 0)
                return NotFound();

            var company = await _context.ContractTypes.FindAsync(id);
            if (company == null)
                return NotFound();

            ViewData["Title"] = "Επεξεργασία τύπου σύμβασης ";
            return View(company);
        }

        // POST: ContractTypes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, ContractType contractType)
        {
            if (id != contractType.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contractType);
                    await _baseDataWork.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                }
                return RedirectToAction(nameof(Index));
            }
            return View(contractType);
        }

        public async Task<ActionResult> DownloadExcelTemplate()
        {
            var excelColumns = new List<string>(new string[] {
                "Name",
                "Description",
                "IsActive"
            });

            var excelPackage = (await (new ExcelService<ContractType>(_context)
               .CreateNewExcel("ContractTypes"))
               .AddSheetAsync(excelColumns))
               .CompleteExcel(out var errors);

            if (errors.Count == 0)
                using (var package = excelPackage)
                    return File(package.GetAsByteArray(), XlsxContentType, "ContractTypes.xlsx");
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


            var excelPackage = (await (new ExcelService<ContractType>(_context)
             .CreateNewExcel("ContractTypes"))
             .AddSheetAsync(excelColumns, "ContractTypes"))
             .CompleteExcel(out var errors);

            if (errors.Count == 0)
                using (var package = excelPackage)
                    return File(package.GetAsByteArray(), XlsxContentType, "ContractTypes.xlsx");
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
                var contractTypes = (await (new ExcelService<ContractType>(_context)
                    .ExtractDataFromExcel(ImportExcel)))
                    .ValidateExtractedData()
                    .RetrieveExtractedData(out var errors);

                if (errors.Count == 0)
                {
                    _baseDataWork.ContractTypes.AddRange(contractTypes);
                    var status = await _baseDataWork.SaveChangesAsync();

                    if (status > 0)
                        TempData["StatusMessage"] = contractTypes.Count +
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

﻿using Bussiness.Service;
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
    public class ContractController : MasterController
    {
        private const string XlsxContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        private readonly BaseDbContext _context;
        private BaseDatawork _baseDataWork;
        public ContractController(BaseDbContext BaseDbContext, SecurityDbContext SecurityDbContext)
        {
            _context = BaseDbContext;
            _baseDataWork = new BaseDatawork(BaseDbContext);
        }

        // GET: Contracts
        [Authorize(Roles = "Contract_View")]
        public ActionResult Index()
        {
            ViewData["Title"] = "Σύνολο συμβάσεων";
            return View();
        }

        // GET: Contracts/Details/5
        [Authorize(Roles = "Contract_View")]
        public async Task<ActionResult> Details(int id)
        {
            if (id == 0)
                return NotFound();

            var Contract = await _context.Contracts
                .Include(x => x.ContractMembership)
                .Include(x => x.ContractType)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Contract == null)
                return NotFound();

            ViewData["Title"] = "Προβολη σύμβασης ";

            return View(Contract);
        }

        // GET: Contracts/Create
        [Authorize(Roles = "Contract_Create")]
        public ActionResult Create()
        {
            ViewData["Title"] = "Προσθήκη νέας σύμβασης ";
            return View();
        }

        // POST: Contracts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ContractCreate Contract)
        {
            if (ModelState.IsValid)
            {
                _baseDataWork.Contracts.Add(ContractCreate.CreateFrom(Contract));

                var status = await _baseDataWork.SaveChangesAsync();
                if (status > 0)
                    TempData["StatusMessage"] = "H σύμβαση " +
                        Contract.Title +
                    " δημιουργήθηκε με επιτυχία";
                else
                    TempData["StatusMessage"] = "Ωχ! Δεν έγινε προσθήκη νέων εγγραφών.";

                return RedirectToAction(nameof(Index));
            }
            return View(Contract);
        }

        // GET: Contracts/Edit/5
        [Authorize(Roles = "Contract_Edit")]
        public async Task<ActionResult> Edit(int id)
        {
            if (id == 0)
                return NotFound();

            var contract = await _context.Contracts
                .Include(x => x.ContractMembership)
                .Include(x => x.ContractType)
                .FirstOrDefaultAsync(x => x.Id == id);

            var result = ContractEdit.CreateFrom(contract);

            if (contract == null)
                return NotFound();

            ViewData["Title"] = "Επεξεργασία σύμβασης ";
            return View(result);
        }

        // POST: Contracts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, ContractEdit contract)
        {
            if (id != contract.Id)
                return NotFound();

            var result = ContractEdit.CreateFrom(contract);
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(result);
                    await _baseDataWork.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                }
                return RedirectToAction(nameof(Index));
            }
            return View(result);
        }


        public async Task<ActionResult> DownloadExcelTemplate()
        {
            var excelColumns = new List<string>(new string[] {
                "Title",
                "HoursPerWeek",
                "HoursPerDay",
                "WorkingDaysPerWeek",
                "DayOfDaysPerWeek",
                "Description",
                "GrossSalaryPerHour",
                "NetSalaryPerHour",
                "ContractMembershipId",
                "ContractTypeId",
                "IsActive"
            });

            var excelPackage = (await (new ExcelService<Contract>(_context)
               .CreateNewExcel("Contracts"))
               .AddSheetAsync(excelColumns))
               .CompleteExcel(out var errors);

            if (errors.Count == 0)
                using (var package = excelPackage)
                    return File(package.GetAsByteArray(), XlsxContentType, "Contracts.xlsx");
            else
                TempData["StatusMessage"] = "Ωχ! " + string.Join("", errors);

            return View();
        }

        public async Task<ActionResult> DownloadExcelWithData()
        {
            var excelColumns = new List<string>(new string[] {
               "Title",
                "HoursPerWeek",
                "HoursPerDay",
                "WorkingDaysPerWeek",
                "DayOfDaysPerWeek",
                "Description",
                "GrossSalaryPerHour",
                "NetSalaryPerHour",
                "ContractMembershipId",
                "ContractTypeId",
                "IsActive"
            });


            var excelPackage = (await (new ExcelService<Contract>(_context)
             .CreateNewExcel("Contracts"))
             .AddSheetAsync(excelColumns, "Contracts"))
             .CompleteExcel(out var errors);

            if (errors.Count == 0)
                using (var package = excelPackage)
                    return File(package.GetAsByteArray(), XlsxContentType, "Contracts.xlsx");
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
                var contracts = (await (new ExcelService<Contract>(_context)
                    .ExtractDataFromExcel(ImportExcel)))
                    .ValidateExtractedData()
                    .RetrieveExtractedData(out var errors);

                if (errors.Count == 0)
                {
                    _baseDataWork.Contracts.AddRange(contracts);
                    var status = await _baseDataWork.SaveChangesAsync();

                    if (status > 0)
                        TempData["StatusMessage"] = contracts.Count +
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

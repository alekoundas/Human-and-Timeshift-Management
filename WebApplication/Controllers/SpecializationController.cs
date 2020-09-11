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

namespace WebApplication.Controllers
{
    public class SpecializationController : Controller
    {
        private readonly BaseDbContext _context;
        private BaseDatawork _baseDataWork;
        public SpecializationController(BaseDbContext BaseDbContext, SecurityDbContext SecurityDbContext)
        {
            _context = BaseDbContext;
            _baseDataWork = new BaseDatawork(BaseDbContext);
        }

        // GET: Specializations
        [Authorize(Roles = "Specialization_View")]
        public IActionResult Index()
        {
            ViewData["Title"] = "Σύνολο ρόλων";
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

            ViewData["Title"] = "Προβολή ρόλου";
            return View(specialization);
        }

        // GET: Specializations/Create
        [Authorize(Roles = "Specialization_Create")]
        public IActionResult Create()
        {
            ViewData["Title"] = "Προσθήκη ρόλου";

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

            ViewData["Title"] = "Επεξεργασία ρόλου";
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

        private bool SpecializationExists(int id)
        {
            return _context.Specializations.Any(e => e.Id == id);
        }
    }
}

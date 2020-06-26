using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataAccess;
using DataAccess.Models.Entity;

namespace WebApplication.Controllers
{
    public class WorkPlaceController : Controller
    {
        private readonly BaseDbContext _context;

        public WorkPlaceController(BaseDbContext context)
        {
            _context = context;
        }

        // GET: WorkPlace
        public async Task<IActionResult> Index()
        {
            return View(await _context.WorkPlaces.ToListAsync());
        }

        // GET: WorkPlace/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workPlace = await _context.WorkPlaces
                .FirstOrDefaultAsync(m => m.Id == id);
            if (workPlace == null)
            {
                return NotFound();
            }

            return View(workPlace);
        }

        // GET: WorkPlace/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: WorkPlace/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,MyProperty,Id,CreatedOn")] WorkPlace workPlace)
        {
            if (ModelState.IsValid)
            {
                _context.Add(workPlace);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(workPlace);
        }

        // GET: WorkPlace/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workPlace = await _context.WorkPlaces.FindAsync(id);
            if (workPlace == null)
            {
                return NotFound();
            }
            return View(workPlace);
        }

        // POST: WorkPlace/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Title,Description,MyProperty,Id,CreatedOn")] WorkPlace workPlace)
        {
            if (id != workPlace.Id)
            {
                return NotFound();
            }

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
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(workPlace);
        }

        // GET: WorkPlace/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workPlace = await _context.WorkPlaces
                .FirstOrDefaultAsync(m => m.Id == id);
            if (workPlace == null)
            {
                return NotFound();
            }

            return View(workPlace);
        }

        // POST: WorkPlace/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var workPlace = await _context.WorkPlaces.FindAsync(id);
            _context.WorkPlaces.Remove(workPlace);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkPlaceExists(int id)
        {
            return _context.WorkPlaces.Any(e => e.Id == id);
        }
    }
}

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

namespace WebApplication.Controllers
{
    public class CustomerController : Controller
    {
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
            {
                return NotFound();
            }

            var customer = await _context.Customers.Include(x => x.Company)
                .FirstOrDefaultAsync(z => z.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            ViewData["Title"] = "Προβολή πελάτη ";
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
        public async Task<IActionResult> Create(CustomerCreateViewModel customer)
        {
            if (ModelState.IsValid)
            {
                _baseDataWork.Customers.Add(
                    CustomerCreateViewModel.CreateFrom(customer));
                await _baseDataWork.CompleteAsync();
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
            return View(customer);
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}

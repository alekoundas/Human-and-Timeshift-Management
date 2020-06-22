using System;
using System.Threading.Tasks;
using DataAccess;
using DataAccess.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace WebApplication.Controllers
{
    public class UserController : Controller
    {
        private readonly BaseDbContext _context;


        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        //private readonly ILogger<Create> _logger;
        public UserController(BaseDbContext context,
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager
            /*ILogger<Create> logger*/)
        {
            _context = context;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _userManager = userManager;
            //_logger = logger;
        }

        // GET:  User/Index
        [Authorize(Roles = "User_View")]
        public IActionResult Index()
        {
            return View();
        }

        // GET:  User/Create
        [Authorize(Roles = "User_Create")]
        public IActionResult Create()
        {
            var viewModel = new ApplicationUser();
            return View(viewModel);
        }

        // POST:  User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User_Create")]
        public async Task<IActionResult> Create(ApplicationUser viewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _userManager.CreateAsync(viewModel, "P@ssw0rd");
                if(result.Succeeded)
                    TempData["StatusMessage"] = "Ο χρήστης δημιουργήθηκε με επιτυχία.";
                else
                    TempData["StatusMessage"] = "Ωχ! Ο χρήστης δεν δημιουργήθηκε.";
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);

        }

        // GET:  User/Delete/Id
        [Authorize(Roles = "User_Delete")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Empoyees
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST:  User/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User_Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _context.Empoyees.FindAsync(id);
            _context.Empoyees.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}

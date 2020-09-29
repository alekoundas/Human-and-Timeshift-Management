using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bussiness;
using Bussiness.Repository.Security.Interface;
using DataAccess;
using DataAccess.Models;
using DataAccess.Models.Identity;
using DataAccess.ViewModels.ApplicationUserRoles;
using DataAccess.ViewModels.ApplicationUsers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;



namespace WebApplication.Controllers
{
    public class UserController : Controller
    {
        private readonly ISecurityDatawork _datawork;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public UserController(SecurityDbContext dbContext,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager)
        {
            _datawork = new SecurityDataWork(dbContext);
            _signInManager = signInManager;
            _userManager = userManager;
        }

        // GET:  User/Index
        [Authorize(Roles = "User_View")]
        public IActionResult Index()
        {
            ViewData["Title"] = "Σύνολο χρηστών ";

            return View();
        }

        // GET:  User/Create
        [Authorize(Roles = "User_Create")]
        public IActionResult Create()
        {
            ViewData["Title"] = "Προσθήκη χρήστη";

            return View();
        }

        // GET: Account/ChangePassword
        [HttpGet]
        public async Task<IActionResult> ChangePassword(string userId, string returnUrl)
        {
            var viewModel = new ApplicationUserChangePassword();
            viewModel.UserId = userId;
            viewModel.ReturnUrl = returnUrl ?? Url.Content("~/");

            ViewData["Title"] = "Υποχρεωτική αλλαγή κωδικού!";
            ViewData["Details"] = "Βρίσκεστε εδώ διότι είναι το πρώτο σας Login στην  εφαρμογή, ή κάποιος διαχειριστής ζήτησε επαναφορά του κωδικού σας";
            return View(viewModel);
        }

        // POST: Account/ChangePassword
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ApplicationUserChangePassword viewModel)
        {
            if (viewModel.Password1 == viewModel.Password2)
            {
                var user = await _datawork.ApplicationUsers
                    .FirstOrDefaultAsync(x => x.Id == viewModel.UserId);

                if (user == null)
                {
                    TempData["StatusMessage"] = "Ωχ! Ο κωδικός του χρήστη δεν ενημερώθηκε";
                    return Redirect(Url.Content("~/"));//Go to Home
                }

                user.HasToChangePassword = false;
                user.PasswordHash = _userManager.PasswordHasher
                    .HashPassword(user, viewModel.Password1);

                var status = await _userManager.UpdateAsync(user);
                if (!status.Succeeded)
                {
                    TempData["StatusMessage"] = "Ωχ! Ο κωδικός του χρήστη δεν ενημερώθηκε";
                    return Redirect(Url.Content("~/"));//Go to Home
                }
            }

            TempData["StatusMessage"] = "Ο κωδικός του χρήστη ενημερώθηκε με επιτυχία";
            return RedirectToAction("Login", "Account");
        }


        // POST:  User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User_Create")]
        public async Task<IActionResult> Create(ApplicationUserCreate viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = ApplicationUserCreate.CreateFrom(viewModel);
                var result = await _userManager.CreateAsync(user, "@TempPass1");
                if (result.Succeeded)
                    TempData["StatusMessage"] = "Ο χρήστης δημιουργήθηκε με επιτυχία. Κωδικός: @TempPass1 .";
                else
                    TempData["StatusMessage"] = "Ωχ! Ο χρήστης δεν δημιουργήθηκε.";
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }

        // GET:  User/Edit/Id
        [Authorize(Roles = "User_Edit")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
                return NotFound();

            var user = _datawork.ApplicationUsers.Get(id.ToString());
            if (user == null)
                return NotFound();

            var returnViewModel = ApplicationUsersEdit.CreateFrom(user);
            returnViewModel.WorkPlaceRoles = new List<WorkPlaceRoleValues>();

            var applicationWorkPlaceRoles = await _datawork.ApplicationRoles
                .GetWorkPlaceRolesByUserId(user.Id);

            returnViewModel.WorkPlaceRoles = applicationWorkPlaceRoles
                .Select(x => new WorkPlaceRoleValues
                {
                    WorkPlaceId = x.WorkPlaceId,
                    Name = x.WorkPlaceName
                }).ToList();

            ViewData["Title"] = "Επεξεργασία χρήστη";

            return View(returnViewModel);
        }
        // GET:  User/Profile/Id
        //[Authorize(Roles = "User_Edit")]
        public async Task<IActionResult> Profile(string id)
        {
            if (id == null)
                return NotFound();

            var user = _datawork.ApplicationUsers.Get(id.ToString());
            if (user == null)
                return NotFound();

            var returnViewModel = ApplicationUsersEdit.CreateFrom(user);
            returnViewModel.WorkPlaceRoles = new List<WorkPlaceRoleValues>();

            var applicationWorkPlaceRoles = await _datawork.ApplicationRoles
                .GetWorkPlaceRolesByUserId(user.Id);

            returnViewModel.WorkPlaceRoles = applicationWorkPlaceRoles
                .Select(x => new WorkPlaceRoleValues
                {
                    WorkPlaceId = x.WorkPlaceId,
                    Name = x.WorkPlaceName
                }).ToList();

            ViewData["Title"] = "Προφίλ χρήστη";

            return View(returnViewModel);
        }

        // POST:  User/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User_Edit")]
        public async Task<IActionResult> Edit(string id, ApplicationUser applicationUser)
        {
            if (id != applicationUser.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    applicationUser.UserName = applicationUser.Email;
                     await _userManager.UpdateAsync(applicationUser);
                    TempData["StatusMessage"] = "Ο χρήστης ενημερώθηκε με επιτυχία.";

                }
                catch (Exception /*e*/)
                {
                    TempData["StatusMessage"] = "Ωχ! Ο χρήστης δεν ενημερώθηκε.";

                }
                return RedirectToAction(nameof(Index));
            }
            return View(applicationUser);
        }

        // GET:  User/Details/Id
        [Authorize(Roles = "User_View")]
        public IActionResult Details(string id)
        {
            var viewModel = _datawork.ApplicationUsers.Get(id);
            ViewData["Title"] = "Προβολή χρήστη";

            return View(viewModel);
        }

        private bool UserExists(string id)
        {
            return _datawork.ApplicationUsers.Get(id) == null;
        }
    }
}













//// GET:  User/Edit/Id
//[Authorize(Roles = "User_Edit")]
//public IActionResult Edit(string id)
//{
//    var viewModel = _datawork.ApplicationUsers.Get(id);
//    return View(viewModel);
//}

//// POST:  User/Edit
//[HttpPost]
//[ValidateAntiForgeryToken]
//[Authorize(Roles = "User_Edit")]
//public IActionResult Edit([FromBody] ApplicationUser viewModel)
//{
//    if (ModelState.IsValid)
//    {
//        _datawork.ApplicationUsers.Update(viewModel);
//        _datawork.Complete();
//        TempData["StatusMessage"] = "Ο χρήστης ενημερώθηκε με επιτυχία.";

//        return RedirectToAction(nameof(Edit), new { id = viewModel.Id });

//    }
//    TempData["StatusMessage"] = "Ωχ! Ο χρήστης δεν ενημερώθηκε.";
//    return View(viewModel);
//}

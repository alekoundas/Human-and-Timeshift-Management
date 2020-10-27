using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Bussiness;
using Bussiness.Repository.Security.Interface;
using DataAccess;
using DataAccess.Models.Identity;
using DataAccess.ViewModels.View.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApplication.Controllers
{
    public class AccountController : Controller
    {
        private readonly ISecurityDatawork _datawork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginViewModel> _logger;

        public AccountController(SecurityDbContext dbContex,
            SignInManager<ApplicationUser> signInManager,
            ILogger<LoginViewModel> logger,
            UserManager<ApplicationUser> userManager)
        {
            _datawork = new SecurityDataWork(dbContex);
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        // GET: Account/Login
        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            LoginViewModel viewModel = new LoginViewModel();
            viewModel.ReturnUrl = returnUrl ?? Url.Content("~/");

            if (!string.IsNullOrEmpty(viewModel.ErrorMessage))
                ModelState.AddModelError(string.Empty, viewModel.ErrorMessage);

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            ViewBag.Title = "Είσοδος χρήστη";
            return View(viewModel);
        }



        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(viewModel.LoginUserNameOrEmail);
                if (user == null)
                    user = await _userManager.FindByNameAsync(viewModel.LoginUserNameOrEmail);
                if (user != null)
                {
                    //Check if user needs to change password
                    if (user.HasToChangePassword)
                    {
                        return RedirectToAction("ChangePassword", "User", new { userId = user.Id, returnUrl = viewModel.ReturnUrl });
                    }
                    var result = await _signInManager.CheckPasswordSignInAsync(user, viewModel.Password, lockoutOnFailure: true);
                    if (result.Succeeded)
                    {
                        var roles = new List<ApplicationRole>();
                        var customClaims = new List<Claim>();
                        try
                        {
                            roles = await _datawork.ApplicationUserRoles.GetRolesFormLoggedInUserEmail(_userManager, viewModel.LoginUserNameOrEmail);
                        }
                        catch (Exception e)
                        {
                            throw;
                        }

                        //Add roles from db
                        foreach (var role in roles)
                            customClaims.Add(new Claim(ClaimTypes.Role as string, role.Name));

                        customClaims.Add(new Claim(ClaimTypes.Name as string, user.FirstName));
                        customClaims.Add(new Claim(ClaimTypes.Email as string, user.Email));
                        customClaims.Add(new Claim("UserID", user.Id));
                        customClaims.Add(new Claim("FirstName", user.FirstName));
                        customClaims.Add(new Claim("LastName", user.LastName));
                        customClaims.Add(new Claim("Email", user.Email));

                        var claimsIdentity = new ClaimsIdentity(customClaims,
                            CookieAuthenticationDefaults.AuthenticationScheme);
                        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                        await _signInManager.Context.SignInAsync(IdentityConstants.ApplicationScheme,
                            claimsPrincipal,
                            new AuthenticationProperties { IsPersistent = viewModel.RememberMe });

                        if (string.IsNullOrWhiteSpace(viewModel.ReturnUrl))
                            return RedirectToAction("Index", "Home");
                        else
                            return Redirect(viewModel.ReturnUrl);

                    }
                }
            }
            // If we got this far, something failed, redisplay form
            TempData["StatusMessage"] = "Ωχ! Τα στοιχεία που δώσατε, φαίνεται να είναι λανθασμένα";
            return View();
        }
        // GET: Account/Register
        [HttpGet]
        public IActionResult Register(string returnUrl = null)
        {
            ViewBag.Title = "Εγγραφή χρήστη";
            RegisterViewModel model = new RegisterViewModel();
            model.ReturnUrl = returnUrl;
            return View(model);
        }

        // POST: Account/Register
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return View();
        }

        // GET: Account/Logout
        [HttpGet]
        public IActionResult Logout()
        {
            _signInManager.SignOutAsync();
            TempData["StatusMessage"] = "Αποσυνδεθήκατε με επιτυχία.";
            return RedirectToAction("Index", "Home");
        }


    }
}
//TempData["StatusMessage"] = "Το χρονοδιάγραμμα δημιουργήθηκε με επιτυχία.";

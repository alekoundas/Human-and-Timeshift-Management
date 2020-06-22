using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DataAccess;
using DataAccess.Models.Identity;
using DataAccess.ViewModels.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace WebApplication.Controllers
{
    public class AccountController : Controller
    {
        private readonly SecurityDbContext _dbContex;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginVM> _logger;

        public AccountController(SecurityDbContext dbContex,
            SignInManager<ApplicationUser> signInManager,
            ILogger<LoginVM> logger,
            UserManager<ApplicationUser> userManager)
        {
            _dbContex = dbContex;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        // GET: Account/Login
        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            LoginVM viewModel = new LoginVM();
            viewModel.ReturnUrl = returnUrl ?? Url.Content("~/");

            if (!string.IsNullOrEmpty(viewModel.ErrorMessage))
                ModelState.AddModelError(string.Empty, viewModel.ErrorMessage);

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            return View(viewModel);
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM viewModel)
        {

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(viewModel.Email);
                if (user != null)
                {
                    var result = await _signInManager.CheckPasswordSignInAsync(user, viewModel.Password, lockoutOnFailure: true);
                    var userRoles = await _dbContex.UserRoles.Where(x => x.UserId == _userManager.FindByEmailAsync(viewModel.Email).Result.Id.ToString()).ToListAsync();
                    var roles = new List<IdentityRole>();

                    if (result.Succeeded)
                    {
                        var customClaims = new List<Claim>();

                        if (userRoles.Count() != 0)
                        {
                            roles = _dbContex.Roles.ToList().Where(y => userRoles.Where(z => z.RoleId == y.Id).Count()>0).ToList();
                            foreach (var role in roles)
                                customClaims.Add(new Claim(ClaimTypes.Role as string, role.Name));
                        }
                        var claimsIdentity = new ClaimsIdentity(customClaims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                        await _signInManager.Context.SignInAsync(IdentityConstants.ApplicationScheme,
                            claimsPrincipal, new AuthenticationProperties { IsPersistent = viewModel.RememberMe });

                        if (string.IsNullOrWhiteSpace(viewModel.ReturnUrl))
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            return Redirect(viewModel.ReturnUrl);
                        }
                    }
                }
            }
            // If we got this far, something failed, redisplay form
            return View();
        }

        // GET: Account/Logout
        [HttpGet]
        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            return View();
        }


    }
}

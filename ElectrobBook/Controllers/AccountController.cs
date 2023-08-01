
using Domin.Entity;
using Domin.ViewModel;
using Infrastructure;
using Infrastructure.constant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;

using System.Security.Claims;
using System.Text.RegularExpressions;

namespace ElectrobBook.Controllers
{

    [Authorize]
    public class AccountController : Controller
    {

        private readonly RoleManager<IdentityRole> _identityRoles;
        private readonly UserManager<AppUserModel> _identityUser;
        private readonly ElectroBookDbContext _dbContext;
        private readonly SignInManager<AppUserModel> _signInManager;



        public AccountController(RoleManager<IdentityRole> identityRoles, UserManager<AppUserModel> identityUser, ElectroBookDbContext dbContext, SignInManager<AppUserModel> signInManager)
        {
            _identityRoles = identityRoles;
            _identityUser = identityUser;
            _dbContext = dbContext;
            _signInManager = signInManager;

        }
        private bool IsPasswordValid(string password)
        {
            bool hasNonAlphanumeric = Regex.IsMatch(password, "^(?=.*[\\W_]).+$@");
            bool hasUppercase = Regex.IsMatch(password, "[A-Z]");
            bool hasLowercase = Regex.IsMatch(password, "[a-z]");
            bool hasDigit = Regex.IsMatch(password, "[0-9]");


            if (hasNonAlphanumeric && hasUppercase && !hasLowercase && !hasDigit && password.Length >= 10)
            {
                // The password meets the configured requirements
                return true;
            }
            else
            {
                // The password does not meet the configured requirements
                return false;
            }
        }

        [AllowAnonymous]
        public async Task<IActionResult> login()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> login(Login model)
        {


            if (ModelState.IsValid)
            {
                var user = await _identityUser.FindByEmailAsync(model.Email);
                var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                    return RedirectToAction("Index", "Home");
                else
                    ModelState.AddModelError("Email", "Invalid login attempt.");
                return View(model);
            }

            return BadRequest();
        }


        public async Task<IActionResult> logout()
        {


            await _signInManager.SignOutAsync();

            return RedirectToAction(nameof(login));
        }





        [AllowAnonymous]
        public async Task<IActionResult> Register()
        {

            var model = new UserModel();
           

            return View(model);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        //create User and assign role
        public async Task<IActionResult> Register(UserModel model)
        {

            if (ModelState.ErrorCount <= 1)
            {
                //create 

                bool PassIsValid = IsPasswordValid(model.Password);
                if (PassIsValid)
                {
                    // password is invalid - display an error message to the user
                }
                else
                {
                    model.Id = Guid.NewGuid().ToString();
                    var user = new AppUserModel()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Email = model.Email,
                        //ActiveUser = model.ActiveUser,
                        Name = model.Name,
                        UserName = model.Email,

                    };
                    var result = await _identityUser.CreateAsync(user, model.Password);
                    if (result.Succeeded) { await _identityUser.AddToRoleAsync(user, "BASIC"); }

                }

            }
            return RedirectToAction("Users", "Perimssion");

        }





    }
}

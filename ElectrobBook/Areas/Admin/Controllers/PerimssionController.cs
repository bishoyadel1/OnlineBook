
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

namespace ElectrobBook.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    public class PerimssionController : Controller
    {
        private readonly RoleManager<IdentityRole> _identityRoles;
        private readonly UserManager<AppUserModel> _identityUser;
        private readonly ElectroBookDbContext _dbContext;
        private readonly SignInManager<AppUserModel> _signInManager;



        public PerimssionController(RoleManager<IdentityRole> identityRoles, UserManager<AppUserModel> identityUser, ElectroBookDbContext dbContext, SignInManager<AppUserModel> signInManager)
        {
            _identityRoles = identityRoles;
            _identityUser = identityUser;
            _dbContext = dbContext;
            _signInManager = signInManager;

        }
      

        public async Task<IActionResult> Role()
        {
            var model = await _identityRoles.Roles.ToListAsync();


            return View(model);
        }

        public IActionResult Users()
        {
            IEnumerable<UsersView> model = _dbContext.UsersView.ToList();
            return View(model);

        }

        public IActionResult CreateRole()
        {
            var model = new NewRole();


            return View(model);
        }


        public async Task<IActionResult> UpdateRole(string Id)
        {


            if (Id == null)
            {
                return NotFound();
            }
            else
            {
                var role = await _identityRoles.FindByIdAsync(Id);
                var model = new NewRole() { RoleId = role.Id, RoleName = role.Name };
                return View("CreateRole", model);
            }





        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(NewRole model)
        {
            if (ModelState.IsValid)
            {
                if (model.RoleId == "1")
                {
                    model.RoleId = Guid.NewGuid().ToString();
                    var role = new IdentityRole()
                    {
                        Id = model.RoleId,
                        Name = model.RoleName,
                    };

                    await _identityRoles.CreateAsync(role);
                    return RedirectToAction(nameof(role));
                }
                //update
                else
                {
                    var role = await _identityRoles.FindByIdAsync(model.RoleId);
                    role.Id = model.RoleId;
                    role.Name = model.RoleName;
                    await _identityRoles.UpdateAsync(role);
                    return RedirectToAction(nameof(role));

                }

            }


            return View(model);
        }

        public async Task<IActionResult> Delete(string Id)
        {


            if (Id == null)
            {
                return NotFound();
            }
            else
            {
                var role = await _identityRoles.FindByIdAsync(Id);
                await _identityRoles.DeleteAsync(role);
                return RedirectToAction(nameof(Role));
            }





        }
        // Reg Form
       

        [HttpPost]
        [ValidateAntiForgeryToken]
        //create User and assign role
        public async Task<IActionResult> UpdateUser(NewUser model)
        {


            if (ModelState.ErrorCount <= 4)
            {
                //create 


                var user = await _identityUser.FindByIdAsync(model.UserModel.Id);


                if (user == null) { return NotFound(); }
                else
                {
                    user.Name = model.UserModel.Name;
                    user.Email = model.UserModel.Email;
                //    user.ActiveUser = model.UserModel.ActiveUser;
                    //      user.ImageUser = model.UserModel.ImageUser;
                    var result = await _identityUser.UpdateAsync(user);


                    if (result.Succeeded)
                    {

                        return RedirectToAction(nameof(Users));
                    }
                    else
                        return BadRequest(result);



                }



            }
            return RedirectToAction(nameof(Users));

        }
        //send user to Update
        public async Task<IActionResult> UserData(string Id)
        {

            if (Id == null)
            {
                return RedirectToAction(nameof(Users));
            }

            var user = await _identityUser.FindByIdAsync(Id);
            var role = await _identityUser.GetRolesAsync(user);



            var model = new NewUser()
            {
                Roles = await _identityRoles.Roles.ToListAsync(),
                UserModel = new UserModel()

            };

            model.UserModel.Name = user.Name;
            model.UserModel.Email = user.Email;
      //      model.UserModel.ActiveUser = user.ActiveUser;
      //      model.UserModel.RoleName = role.First();
            model.UserModel.Id = Id;

            return View("Register", model);



        }
        public async Task<IActionResult> DeleteUser(string Id)
        {

            if (Id == null)
            {
                return RedirectToAction(nameof(Users));
            }

            var user = await _identityUser.FindByIdAsync(Id);
            var role = await _identityUser.GetRolesAsync(user);



            var result = await _identityUser.DeleteAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Users));
            }
            else
                return NotFound();



        }

        public async Task<IActionResult> AssignRole(string Id)
        {

            if (Id == null)
            {
                return RedirectToAction(nameof(Users));
            }

            var user = await _identityUser.FindByIdAsync(Id);
            //   var role = await _identityUser.GetRolesAsync(user);
            var roles = await _identityRoles.Roles.ToListAsync();
            var model = new AddOrDeleteRoleFromUser()
            {
                Name = user.Name.ToString(),
                UserId = user.Id,
                Roles = roles.Select(role => new SelectRole { RoleName = role.Name, IsSelected = _identityUser.IsInRoleAsync(user, role.Name).Result, }).ToList(),


            };


            return View(model);




        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignRole(AddOrDeleteRoleFromUser model)
        {
            if (ModelState.IsValid)
            {
                var user = await _identityUser.FindByIdAsync(model.UserId);
                var olduserrole = await _identityUser.GetRolesAsync(user);
                for (var i = 0; i < model.Roles.Count; i++)
                {
                    if (model.Roles[i].IsSelected)
                    {
                        if (!olduserrole.Contains(model.Roles[i].RoleName))
                            await _identityUser.AddToRoleAsync(user, model.Roles[i].RoleName);
                    }
                    else
                    {
                        if (olduserrole.Contains(model.Roles[i].RoleName))
                            await _identityUser.RemoveFromRoleAsync(user, model.Roles[i].RoleName);
                    }
                }


                return RedirectToAction(nameof(Users));

            }

            return RedirectToAction(nameof(Users));
        }
        public async Task<IActionResult> AssignPerimssion(string RoleId)
        {

            if (RoleId == null)
            {
                return RedirectToAction(nameof(Users));
            }
            var role = await _identityRoles.FindByIdAsync(RoleId);
            var roleclim = await _identityRoles.GetClaimsAsync(role);
            var AllPermission = helper.AllPermission();
            var Permissions = AllPermission.Select(p => new Perimssion() { PermissionName = p }).ToList();

            foreach (var permission in Permissions)
            {
                if (roleclim.Any(p => p.Value == permission.PermissionName))
                {
                    permission.IsSelected = true;
                }
            }
            var model = new AddOrDeletePerimssionFromRole() { RoleName = role.Name, RoleId = RoleId, Perimssions = Permissions };


            return View(model);




        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignPerimssion(AddOrDeletePerimssionFromRole model)
        {

            if (ModelState.IsValid)
            {
                var role = await _identityRoles.FindByIdAsync(model.RoleId);
                var oldClaimrole = await _identityRoles.GetClaimsAsync(role);
                for (var i = 0; i < model.Perimssions.Count; i++)
                {
                    if (model.Perimssions[i].IsSelected)
                    {
                        if (!oldClaimrole.Any(p => p.Value == model.Perimssions[i].PermissionName))
                        {
                            await _identityRoles.AddClaimAsync(role, new Claim("Perimssion", model.Perimssions[i].PermissionName));
                        }

                    }
                    else
                    {
                        if (oldClaimrole.Any(p => p.Value == model.Perimssions[i].PermissionName))
                        {
                            await _identityRoles.RemoveClaimAsync(role, new Claim("Perimssion", model.Perimssions[i].PermissionName));
                        }
                    }



                }


                return RedirectToAction(nameof(Role));




            }
            return RedirectToAction(nameof(Role));

        }
    }
}

using Domin.ViewModel;
using Infrastructure.constant;
// Add this line
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Seeds
{
    public static class DefaultUser
    {
        public static async Task DefaultAdmin(UserManager<AppUserModel> userManager , RoleManager<IdentityRole> roleManager) {

            var user = new AppUserModel() {
                Name = "Bishooooooooooooo",
                Email = "SuberAdmin1$@Admin.com",
                Id = Guid.NewGuid().ToString()
            };
         var getuser =   await roleManager.FindByIdAsync(user.Id);
            if (getuser == null)
            {
                await userManager.CreateAsync(user, "SuberAdmin1$@Admin.com");
                await roleManager.SeedClaimsForSuperUser();
            }
          
            
        }
        public static async Task DefaultUsers(UserManager<AppUserModel> userManager)
        {

            var user = new AppUserModel()
            {
                Name = "Bishooooooooooooo",
                Email = "Basic1$@Admin.com",
                Id = Guid.NewGuid().ToString()
            };
            await userManager.CreateAsync(user, "Basic1$@Admin.com");
        }
        public static async Task SeedClaimsForSuperUser(this RoleManager<IdentityRole> roleManager)
        {
            IdentityRole adminRole = await roleManager.FindByNameAsync(helper.Roels.SuperAdmin.ToString());

            if (adminRole != null)
            {
                 await roleManager.AddPermissionClaims(adminRole, "Products");
            }
        }

        public static async Task AddPermissionClaims(this RoleManager<IdentityRole> roleManager, IdentityRole role, string module)
        {
            if (role != null && !string.IsNullOrEmpty(module))
            {
                var getClims = await roleManager.GetClaimsAsync(role);
                var Permission = helper.Permission(module);

                foreach (var permission in Permission)
                {
                    if (!getClims.Any(i => i.Type == "Permission" && i.Value == permission))
                    {
                        await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
                    }
                }
            }


        }
    }
}

using Infrastructure.constant;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Seeds
{
    public static class DefaultRoles
    {
        
        public static async Task DefaultRole(RoleManager<IdentityRole> roleManager)
        {
           
            await roleManager.CreateAsync(new IdentityRole( helper.Roels.SuperAdmin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(helper.Roels.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(helper.Roels.Basic.ToString()));
        }
    }
}

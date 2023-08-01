using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.constant
{
    public class helper
    {

        public enum Roels{
            SuperAdmin, Admin , Basic
        }
        public enum Depart
        {
            Products, Stock
        }
        public static List<string> Permission(string module) { 
            
            return new List<string> ()
            {        $"Permission.{module}.Edit",
                     $"Permission.{module}.Create",
                     $"Permission.{module}.View",
                     $"Permission.{module}.Delete"

            }; }
        public static List<string> AllPermission()
        {
            var allpermission = new List<string>();

            var depart = Enum.GetValues(typeof(Depart));
            foreach (var permission in depart)
            {
                allpermission.AddRange(Permission(permission.ToString()));

            }

            return allpermission;
        }

    }
}

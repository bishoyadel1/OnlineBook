using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domin.Entity
{
    public class Category
    {
        public Guid Id { get; set; }
        [Required,MaxLength(50),MinLength(3)]
        public string ? Name{ get; set; }
        [Required, MaxLength(50), MinLength(3)]
        public string Description { get; set; }
        public int CurrentStaut { get; set; }
    }
}

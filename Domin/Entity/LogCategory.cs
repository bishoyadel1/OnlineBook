using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domin.Entity
{
    public class LogCategory
    {
        public Guid Id { get; set; }
        public string? Action { get; set; }
        public DateTime DateTime { get; set; }
        public Guid UserId { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
    }
}

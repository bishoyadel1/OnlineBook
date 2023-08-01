using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domin.Entity
{
    public class LogBook
    {
        public Guid Id { get; set; }
        public string? Action { get; set; }
        public DateTime DateTime { get; set; }
        public Guid UserId { get; set; }
        public Guid BookId { get; set; }
        public Book Book { get; set; }
    }
}


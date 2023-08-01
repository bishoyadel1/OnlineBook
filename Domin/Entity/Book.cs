using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domin.Entity
{
    public class Book
    {
        public Guid Id { get; set; }
        [Required, MaxLength(50), MinLength(3)]
        public string Name { get; set; }

        [Required, MaxLength(50), MinLength(3)]
        public string Author { get; set; }
        public byte[] BookImage { get; set; }
        public string FileName { get; set; }
        public string Description { get; set; }
        public bool Publish { get; set; }
        public int CurrentStaut { get; set; }

        public Guid CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        public Guid SubCategoryId { get; set; }


    }
}

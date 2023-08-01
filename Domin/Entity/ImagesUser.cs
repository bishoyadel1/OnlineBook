using Domin.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domin.Entity
{
    public class ImagesUser
    {
        public Guid Id { get; set; }
        public byte[] Image { get; set; }

        public AppUserModel AppUserModel { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.IRepository
{
    public interface IServicesCategoryLog<T> where T : class
    {
       IEnumerable<T> GetAll();
        Task<T> FindById(Guid Id);
      
        bool Update(Guid Id, Guid UserId);
      bool Delete(Guid Id, Guid UserId);
      bool Save (Guid Id, Guid UserId);
    }
}

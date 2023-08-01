using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.IRepository
{
    public interface IServicesCategory<T> where T : class

    {
       IEnumerable<T> GetAll();
       T FindById(Guid Id);
       T FindByName(string Name);
       bool Save(T entity);
       bool Delete(Guid id);

    }
}

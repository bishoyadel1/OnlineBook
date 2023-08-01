using Domin.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.IRepository.ServicesRepository
{
    public class ServicesLogRepository : IServicesCategoryLog<LogCategory>

    {

        private readonly ElectroBookDbContext _context;
        public ServicesLogRepository(ElectroBookDbContext context) {
            _context = context;
        }

        public bool Delete(Guid Id, Guid UserId)
        {
            try
            {
                var result = new LogCategory()
                {

                    Id = Guid.NewGuid(),
                    CategoryId = Id,
                    UserId = UserId,
                    DateTime = DateTime.Now,
                    Action = "Update"
                };
                _context.Add(result);
                _context.SaveChanges();
                return true;
            }
            catch (Exception) { return false; }
        }

        public async Task<LogCategory> FindById(Guid Id)
        {
            try
            {
                return await _context.LogCategorys.FirstOrDefaultAsync(id => id.Id.Equals(Id));
            }
            catch (Exception) { return null; }
        }


        public IEnumerable<LogCategory> GetAll()
        {
            try
            {
                return _context.LogCategorys.Include(lc => lc.Category).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool Save(Guid Id, Guid UserId)
        {
            try
            {
                var logCategory = new LogCategory
                {
                    Id = Guid.NewGuid(),
                    Action = "Save",
                    DateTime = DateTime.Now,
                    UserId = UserId,
                    CategoryId = Id
                };
                _context.LogCategorys.Add(logCategory);
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Update(Guid Id, Guid UserId)
        {
            try
            {
                var result = new LogCategory()
                {
                    Id= Guid.NewGuid(),
                    CategoryId = Id,
                    UserId = UserId,
                    DateTime = DateTime.Now,
                    Action = "Update"
                };
                _context.Add(result);
                 _context.SaveChanges();
                return true;
            }
            catch (Exception) { return false; }
        }
    }
}

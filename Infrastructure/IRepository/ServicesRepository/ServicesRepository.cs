using Domin.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.IRepository.ServicesRepository
{
    public class ServicesRepository : IServicesCategory<Category>
    {
        private readonly ElectroBookDbContext _Context;
        public ServicesRepository(ElectroBookDbContext onlineBookDbContext) { 
        
        _Context= onlineBookDbContext;
        }

      

        public  bool Delete(Guid id)
        {
            try
            {
                var category =  _Context.Categorys.Find(id);
                if(category != null)
                {

                    category.CurrentStaut = 0;
                    _Context.Categorys.Update(category);
                    _Context.SaveChanges();
                    return true;
                }
                
                else { return false; }
            }
            catch (Exception) { return false; }
        }

        public Category FindById(Guid Id)
        {
            try
            {
                return  _Context.Categorys.FirstOrDefault(id => id.Id.Equals(Id));

            }catch(Exception ) { return null; }
        }

        public Category FindByName(string Name)
        {
            try
            {
                return  _Context.Categorys.Where(item => item.CurrentStaut > 0).FirstOrDefault(i => i.Name.Equals(Name.Trim()));

            }
            catch (Exception) { return null; }
        }

        public IEnumerable<Category> GetAll()
        {
            try
            {
                return  _Context.Categorys.OrderBy(l=>l.Name).Where(item=>item.CurrentStaut > 0).ToList();

            }
            catch (Exception) { return null; }
        }

        public bool Save(Category entity)
        {
            try
            {
                var model = _Context.Categorys.Find(entity.Id);
                if(model == null)
                {
                    var category = new Category();
                   category.Id = entity.Id;
                   category.Name = entity.Name;
                   category.Description = entity.Description;
                    category.CurrentStaut = entity.CurrentStaut;
                     _Context.Add(category);
                }  
                else
                {
                   
                    model.Id = entity.Id;
                    model.Name = entity.Name;
                    model.Description = entity.Description;
                    model.CurrentStaut = entity.CurrentStaut;
                       _Context.Update(model);
                    
                }
                _Context.SaveChanges();
                return true;

            }
            catch (Exception) { return false; }
        }

    

       
    }
}

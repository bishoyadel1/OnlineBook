using Domin.Entity;
using Domin.ViewModel;
using Infrastructure;
using Infrastructure.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace OnlineBook.Areas.Admin.Controllers
{
    [Area("admin")]
    public class CategoriesController : Controller
    {
        private readonly IServicesCategoryLog<LogCategory> _categoryLog;
        private readonly IServicesCategory<Category> _category;
        private readonly UserManager<AppUserModel> _UserManager;
        private readonly ElectroBookDbContext _context;

        public CategoriesController(IServicesCategoryLog<LogCategory> categoryLog , IServicesCategory<Category> category , UserManager<AppUserModel> UserManager , ElectroBookDbContext dbContext) {
            _category = category;
            _UserManager = UserManager;
            _categoryLog= categoryLog;
            _category = category;
        }


        public async Task<IActionResult> Categories()
        {

            var model = new CategoriesViewModel() { categories = _category.GetAll() };

            return View(model);
        }
        public async Task<IActionResult> LogCategories()
        {
            var model = new LogCategoriesViewModel
            {
                logcategories = _categoryLog.GetAll(),
                categories = _category.GetAll().Where(c => c.CurrentStaut <= 0)
            };

            return View(model);
        }

        public async Task<IActionResult> Create()
        {

            var model = new NewCategory();
            

            return View(model);
        
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NewCategory model)
        {
            var userId = _UserManager.GetUserId(User);
            if (ModelState.IsValid)

            { if(model.Id == "1")
                {
                    var category = new Category()
                    {
                        Id = Guid.NewGuid(),
                        Name = model.Name,
                        Description = model.Description,
                        CurrentStaut = 1
                    };

                   

                    var result = _category.Save(category);
                    if (result)
                    {

                        var log = _categoryLog.Save(category.Id, Guid.Parse(userId));

                        return RedirectToAction(nameof(Categories));
                    }
                    else { return View(model); }
                }
            else
            //update
                {
                   
                   var category = new Category() { Id= Guid.Parse( model.Id), Name = model.Name, Description = model.Description ,CurrentStaut = 1
                   };
                    
                    var result = _category.Save(category);
                    if(result)
                    {
                        var log = _categoryLog.Update(category.Id , Guid.Parse(userId));

                        return RedirectToAction(nameof(Categories));
                    }
                    else { return View(model); }
                }
               

            }

            return View(model);
        }
        public async Task<IActionResult> Update(string Id)
        {

            var category = _category.FindById(Guid.Parse(Id));

            var model = new NewCategory() { Id = Id.ToString() , Name = category.Name , Description = category.Description };

            return View("Create",model);

        }
        public async Task<IActionResult> Delete(string Id)
        {
            var userId = _UserManager.GetUserId(User);
            if (Id!= null)
            {
                if(_category.Delete(Guid.Parse(Id)))
                {
                    _categoryLog.Delete(Guid.Parse(Id), Guid.Parse(userId));
                    return RedirectToAction(nameof(Categories));
                }
                else
                    return View(nameof(Categories));

            }


            return View(nameof(Categories));

        }
    }
}

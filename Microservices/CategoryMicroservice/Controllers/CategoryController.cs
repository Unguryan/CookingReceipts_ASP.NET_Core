using CategoryMicroservice.Models;
using Core.Attributes;
using Core.Context;
using Core.Context.Dbo;
using Core.Models;
using Core.RabbitMQ.Models;
using Interfaces.Models;
using Interfaces.RabbitMQ;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CategoryMicroservice.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : Controller
    {

        private readonly CookingContext _context;
        private readonly IRabbitBus _bus;

        public CategoryController(CookingContext context, IRabbitBus bus)
        {
            _context = context;
            _bus = bus;
        }

        [HttpGet()]
        [CustomTokenAuthentication("Owner, Admin")]
        public async Task<IEnumerable<ICategory>> GetAllCategories()
        {
            var list = new List<ICategory>();
            foreach (var item in await _context.Categories.ToListAsync())
            {
                list.Add(new Category(item));
            }

            return list;
        }

        [HttpPost("AddCategory")]
        [CustomTokenAuthentication("Owner, Admin")]
        public async Task<ICategory> AddCategory(AddCategoryModel model)
        {
            if (string.IsNullOrEmpty(model?.Name) ||
                await _context.Categories.AnyAsync(c => c.Name.Equals(model.Name, StringComparison.OrdinalIgnoreCase)))
            {
                ModelState.AddModelError("Error", $"You are already have {model.Name} category");
                return new Category(-1, "");
            }

            var res = await _context.Categories.AddAsync(new CategoryDbo() { Name = model.Name });
            await _context.SaveChangesAsync();

            return new Category(res.Entity);
        }

        [HttpPost("ChangeCategory")]
        [CustomTokenAuthentication("Owner, Admin")]
        public async Task<ICategory> ChangeCategory(ChangeCategoryModel model)
        {
            if (string.IsNullOrEmpty(model?.Name) ||
                !await _context.Categories.AnyAsync(c => c.Id == model.Id))
            {
                ModelState.AddModelError("Error", $"You do not have {model.Name} category");
                return new Category(-1, "");
            }

            var res = await _context.Categories.FirstOrDefaultAsync(c => c.Id == model.Id);

            if(res == null)
            {
                ModelState.AddModelError("Error", $"You do not have {model.Name} category");
                return new Category(-1, "");
            }

            res.Name = model.Name;
            await _context.SaveChangesAsync();

            return new Category(res);
        }

        [HttpPost("RemoveCategory")]
        [CustomTokenAuthentication("Owner, Admin")]
        public async Task<ICategory> RemoveCategory(RemoveCategoryModel model)
        {
            if (!await _context.Categories.AnyAsync(c => c.Id == model.Id))
            {
                ModelState.AddModelError("Error", $"You do not have category with {model.Id} ID");
                return new Category(-1, "");
            }

            var res = await _context.Categories.FirstOrDefaultAsync(c => c.Id == model.Id);

            if (res == null)
            {
                ModelState.AddModelError("Error", $"You do not have category with {model.Id} ID");
                return new Category(-1, "");
            }

            var temp = new Category(res);
            _context.Categories.Remove(res);
            await _context.SaveChangesAsync();
            //TODO: Notify Receips Service, and remove all receipts with this category
            await _bus.SendAsync<RemovedCategoryRabbitModel>("Categories", new RemovedCategoryRabbitModel() { Id = res.Id});


            return temp;
        }
    }
}

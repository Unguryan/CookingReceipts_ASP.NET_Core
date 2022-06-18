using Core.Attributes;
using Core.Context;
using Core.Context.Dbo;
using Core.Models;
using Interfaces.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReceiptsMicroservice.Models;

namespace ReceiptsMicroservice.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReceiptsController : Controller
    {

        private readonly CookingContext _context;

        public ReceiptsController(CookingContext context)
        {
            _context = context;
        }

        [HttpGet()]
        public async Task<IEnumerable<IReceipt>> GetReceipts()
        {
            var list = new List<IReceipt>();
            foreach (var item in await _context.Receipts
                .Include(r => r.Categories).Include(r => r.Owner).ToListAsync())
            {
                list.Add(new Receipt(item));
            }

            return list;
        }

        [HttpGet("{id}")]
        public async Task<IReceipt> GetReceipt(int id)
        {
            var res = await _context.Receipts.Include(x => x.Owner).Include(x => x.Categories)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (res == null)
            {
                ModelState.AddModelError("Error", $"Receipt not found");
                return new Receipt(-1, null, "", "", "");
            }


            return new Receipt(res);
        }

        [HttpGet("ByName/{name}")]
        public async Task<IEnumerable<IReceipt>> GetReceiptsByName(string name)
        {
            var list = new List<IReceipt>();
            foreach (var item in _context.Receipts
                .Include(r => r.Categories).Include(r => r.Owner).AsEnumerable()
                .Where(r => r.Name.Contains(name, StringComparison.OrdinalIgnoreCase)))
            {
                list.Add(new Receipt(item));
            }

            return list;
        }

        [HttpGet("ByCategory/{caterogyId}")]
        public async Task<IEnumerable<IReceipt>> GetReceiptsByCategory(int caterogyId)
        {
            var list = new List<IReceipt>();
            foreach (var item in await _context.Receipts
                .Include(r => r.Categories).Include(r => r.Owner).AsQueryable()
                .Where(r => r.Categories.Any(c => c.Id == caterogyId)).ToListAsync())
            {
                list.Add(new Receipt(item));
            }

            return list;
        }

        [HttpGet("ByUser/{userId}")]
        public async Task<IEnumerable<IReceipt>> GetUserReceipts(int userId)
        {
            var list = new List<IReceipt>();
            foreach (var item in await _context.Receipts
                .Include(r => r.Categories).Include(r => r.Owner).AsQueryable()
                .Where(r => r.Owner.Id == userId).ToListAsync())
            {
                list.Add(new Receipt(item));
            }

            return list;
        }

        [CustomTokenAuthentication("Owner, Admin, User")]
        [HttpGet("LikedReceipts/{userId}")]
        public async Task<IEnumerable<IReceipt>> GetLikedReceipts(int userId)
        {
            //TODO: Get From attibute/query user to check, that them are same
            var list = new List<IReceipt>();
            var u = await _context.Users.Include(x => x.LikedReceipts).FirstOrDefaultAsync(u => u.Id == userId);
            if(u == null || u.LikedReceipts == null || !u.LikedReceipts.Any())
            {
                return list;
            }

            //TODO: Fix it
            foreach (var item in _context.Receipts.Include(r => r.Owner).Include(r => r.Categories).AsEnumerable()
                .Where(r => u.LikedReceipts.Any(lr => lr.Id == r.Id)))
            {
                list.Add(new Receipt(item));
            }

            return list;
        }

        [HttpPost("AddReceipt")]
        [CustomTokenAuthentication("Owner, Admin, User")]
        public async Task<IReceipt> AddReceipt(AddReceiptModel model)
        {
            //TODO: Get From attibute/query user to check, that them are same
            var u = await _context.Users.FirstOrDefaultAsync(u => u.Id == model.OwnerId);  

            if (u == null)
            {
                ModelState.AddModelError("Error", $"User undefined");
                return new Receipt(-1, null, "", "", "");
            }

            var receipt = new ReceiptDbo()
            {
                Name = model.Name,
                Owner = u,
                Description = model.Description,
                Ingredients = model.Ingredients,
                Categories = new List<CategoryDbo>()
            };

            foreach (var item in await _context.Categories.AsQueryable()
                .Where(c => model.Categories.Any(catId => catId == c.Id)).ToListAsync())
            {
                receipt.Categories.Add(item);
            }

            var res = await _context.Receipts.AddAsync(receipt);
            await _context.SaveChangesAsync();

            return new Receipt(res.Entity);
        }

        [HttpPost("ChangeReceipt")]
        [CustomTokenAuthentication("Owner, Admin, User")]
        public async Task<IReceipt> ChangeReceipt(ChangeReceiptModel model)
        {
            //TODO: Get From attibute/query user to check, that them are same
            var u = await _context.Users.FirstOrDefaultAsync(u => u.Id == model.OwnerId);

            if (u == null)
            {
                ModelState.AddModelError("Error", $"User undefined");
                return new Receipt(-1, null, "", "", "");
            }

            var recToChange = await _context.Receipts.FirstOrDefaultAsync(r => r.Id == model.ReceiptId);

            if (recToChange == null)
            {
                ModelState.AddModelError("Error", $"Receipt undefined");
                return new Receipt(-1, null, "", "", "");
            }

            recToChange.Name = model.Name;
            recToChange.Description = model.Description;
            recToChange.Ingredients = model.Ingredients;
            recToChange.Categories = new List<CategoryDbo>();

            foreach (var item in await _context.Categories.AsQueryable()
                .Where(c => model.Categories.Any(catId => catId == c.Id)).ToListAsync())
            {
                recToChange.Categories.Add(item);
            }

            await _context.SaveChangesAsync();

            return new Receipt(recToChange);
        }

        [HttpPost("RemoveReceipt")]
        [CustomTokenAuthentication("Owner, Admin, User")]
        public async Task<IReceipt> RemoveReceipt(RemoveReceiptModel model)
        {
            //TODO: Get From attibute/query user to check, that them are same
            
            var rec = await _context.Receipts.Include(x => x.Owner).Include(x => x.Categories).FirstOrDefaultAsync(r => r.Id == model.Id);

            if (rec == null)
            {
                ModelState.AddModelError("Error", $"Receipt undefined");
                return new Receipt(-1, null, "", "", "");
            }

            var temp = new Receipt(rec);
            _context.Receipts.Remove(rec);
            await _context.SaveChangesAsync();

            return temp;
        }

        [HttpPost("AddLikedReceipt")]
        [CustomTokenAuthentication("Owner, Admin, User")]
        public async Task<IReceipt> AddLikedReceipt(AddOrRemoveLikedReceiptModel model)
        {
            //TODO: Get From attibute/query user to check, that them are same
            var u = await _context.Users.Include(x => x.LikedReceipts).ThenInclude(r => r.Owner).FirstOrDefaultAsync(u => u.Id == model.UserId);
            if (u == null || u.LikedReceipts == null || u.LikedReceipts.Any(x => x.Id == model.ReceiptId))
            {
                ModelState.AddModelError("Error", $"Receipt undefined");
                return new Receipt(-1, null, "", "", "");
            }

            var r = await _context.Receipts.Include(r => r.Owner).FirstOrDefaultAsync(r => r.Id == model.ReceiptId);
            u.LikedReceipts.Add(r);
            await _context.SaveChangesAsync();

            return new Receipt(r);
        }

        [HttpPost("RemoveLikedReceipt")]
        [CustomTokenAuthentication("Owner, Admin, User")]
        public async Task<IReceipt> RemoveLikedReceipt(AddOrRemoveLikedReceiptModel model)
        {
            //TODO: Get From attibute/query user to check, that them are same
            var u = await _context.Users.Include(x => x.LikedReceipts).ThenInclude(r => r.Owner).FirstOrDefaultAsync(u => u.Id == model.UserId);
            if (u == null || u.LikedReceipts == null || !u.LikedReceipts.Any() || !u.LikedReceipts.Any(x => x.Id == model.ReceiptId))
            {
                ModelState.AddModelError("Error", $"Receipt undefined");
                return new Receipt(-1, null, "", "", "");
            }

            var r = u.LikedReceipts.FirstOrDefault(x => x.Id == model.ReceiptId);
            u.LikedReceipts.Remove(r);
            await _context.SaveChangesAsync();

            return new Receipt(r);
        }
    }
}


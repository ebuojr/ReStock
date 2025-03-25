
using ReStockApi.Models;

namespace ReStockApi.Services.Reorder
{
    public class ReorderService : IReorderService
    {
        private readonly ReStockDbContext _db;
        public ReorderService(ReStockDbContext db)
        {
            _db = db;
        }

        public async Task<bool> ProcessReorderAsync(Models.Reorder reorder)
        {
            await _db.Reorders.AddAsync(reorder);
            _db.SaveChanges();
            return true;
        }
    }
}

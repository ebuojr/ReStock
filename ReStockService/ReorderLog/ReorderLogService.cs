using Microsoft.EntityFrameworkCore;
using ReStockDomain;

namespace ReStockService.ReorderLog
{
    public class ReorderLogService : IReorderLogService
    {
        private readonly ReStockDbContext _db;

        public ReorderLogService(ReStockDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<ReOrderLog>> GetLogsByItemNoItemNoAsync(int storeNo, string ItemNo)
            => await _db.ReOrderLogs.Where(l => l.StoreNo == storeNo && l.ItemNo == ItemNo).ToListAsync();

        public async Task<IEnumerable<ReOrderLog>> GetLogsByStoreNoAsync(int storeNo)
            => await _db.ReOrderLogs.Where(x => x.StoreNo == storeNo).ToListAsync();

        public async Task LogAsync(int storeNo, string ItemNo, int quantity, string eventType, string description, bool ordered)
        {
            var log = new ReOrderLog
            {
                StoreNo = storeNo,
                ItemNo = ItemNo,
                Quantity = quantity,
                LogTime = DateTime.Now,
                EventType = eventType,
                Description = description,
                Error = !ordered
            };
            _db.ReOrderLogs.Add(log);
            await _db.SaveChangesAsync();
        }
    }
}

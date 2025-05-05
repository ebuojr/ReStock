using Microsoft.EntityFrameworkCore;
using ReStockApi.Models;

namespace ReStockApi.Services.ReorderLog
{
    public class ReorderLogService : IReorderLogService
    {
        private readonly ReStockDbContext _db;
        private readonly ILogger<ReorderLogService> _logger;
        public ReorderLogService(ReStockDbContext db, ILogger<ReorderLogService> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<IEnumerable<ReOrderLog>> GetLogsAsync(DateTime fromdate, string type, string no, string storeNo)
        {
            IQueryable<ReOrderLog> query = _db.ReOrderLogs;

            if (fromdate != DateTime.MinValue)
                query = query.Where(x => x.LogTime >= fromdate);
            if (!string.IsNullOrEmpty(type))
                query = query.Where(x => x.EventType == type);
            if (!string.IsNullOrEmpty(no))
                query = query.Where(x => x.ItemNo == no);
            if (!string.IsNullOrEmpty(storeNo))
                query = query.Where(x => x.StoreNo.ToString() == storeNo);

            return await query.OrderByDescending(x => x.LogTime).Take(500).ToListAsync();
        }

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

using Microsoft.EntityFrameworkCore;
using ReStockDomain;

namespace ReStockService.Threshold
{
    public class ThresholdService : IThresholdService
    {
        private readonly ReStockDbContext _db;

        public ThresholdService(ReStockDbContext db)
        {
            _db = db;
        }

        public async Task CreateThreshold(InventoryThreshold threshold)
        {
            await _db.InventoryThresholds.AddAsync(threshold);
            await _db.SaveChangesAsync();
        }

        public async Task<InventoryThreshold> GetThresholdAsync(int storeNo, string ItemNo)
            => await _db.InventoryThresholds.FirstOrDefaultAsync(threshold => threshold.StoreNo == storeNo && threshold.ItemNo == ItemNo);

        public async Task UpdateThresholdAsync(InventoryThreshold threshold)
        {
            _db.InventoryThresholds.Update(threshold);
            await _db.SaveChangesAsync();
        }
    }
}

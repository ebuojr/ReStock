using Microsoft.EntityFrameworkCore;
using ReStockDomain;

namespace ReStockService.Store
{
    public class StoreService : IStoreService
    {
        private readonly ReStockDbContext _db;

        public StoreService(ReStockDbContext db)
        {
            _db = db;
        }

        public async Task CreateNewStore(ReStockDomain.Store store)
        {
            await _db.Stores.AddAsync(store);
            await _db.SaveChangesAsync();
        }

        public async Task<List<ReStockDomain.Store>> GetAllStores()
            => await _db.Stores.ToListAsync();

        public async Task<ReStockDomain.Store> GetStore(int storeNo)
            => await _db.Stores.FirstOrDefaultAsync(s => s.No == storeNo);

        public async Task UpdateStore(ReStockDomain.Store store)
        {
            _db.Stores.Update(store);
            await _db.SaveChangesAsync();
        }
    }
}

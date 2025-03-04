using Microsoft.EntityFrameworkCore;
using ReStockApi.Models;

namespace ReStockApi.Services.Store
{
    public class StoreService : IStoreService
    {
        private readonly ReStockDbContext _db;

        public StoreService(ReStockDbContext db)
        {
            _db = db;
        }

        public async Task CreateNewStore(Models.Store store)
        {
            await _db.Stores.AddAsync(store);
            await _db.SaveChangesAsync();
        }

        public async Task<List<Models.Store>> GetAllStores()
            => await _db.Stores.ToListAsync();

        public async Task<Models.Store> GetStore(int storeNo)
            => await _db.Stores.FirstOrDefaultAsync(s => s.No == storeNo);

        public async Task UpdateStore(Models.Store store)
        {
            _db.Stores.Update(store);
            await _db.SaveChangesAsync();
        }
    }
}

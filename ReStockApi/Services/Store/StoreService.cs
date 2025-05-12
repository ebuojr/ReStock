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

        public async Task DeleteStore(int id)
        {
            var item = await _db.Stores.FirstOrDefaultAsync(s => s.Id == id);

            if (item == null)
                throw new Exception($"Store with id {id} not found");

            _db.Stores.Remove(item);
            await _db.SaveChangesAsync();
        }

        public async Task<List<Models.Store>> GetAllStores()
            => await _db.Stores.ToListAsync();

        public async Task<Models.Store> GetStore(int storeNo)
            => await _db.Stores.FirstOrDefaultAsync(s => s.No == storeNo);

        public async Task StoreExists(int storeNo)
        {
            var exists = await _db.Stores.AnyAsync(s => s.No == storeNo);
            if (!exists)
                throw new ArgumentNullException(nameof(storeNo), "Store number does not exist.");
        }

        public async Task UpdateStore(Models.Store store)
        {
            _db.Stores.Update(store);
            await _db.SaveChangesAsync();
        }
    }
}

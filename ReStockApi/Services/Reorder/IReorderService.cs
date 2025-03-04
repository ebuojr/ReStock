namespace ReStockApi.Services.Reorder
{
    public interface IReorderService
    {
        Task<bool> ProcessReorderAsync(int storeNo, string ItemNo, int quantity);
    }
}

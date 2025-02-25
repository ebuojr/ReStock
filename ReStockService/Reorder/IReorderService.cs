namespace ReStockService.Reorder
{
    public interface IReorderService
    {
        Task<bool> ProcessReorderAsync(int storeNo, string productNo, int quantity);
    }
}

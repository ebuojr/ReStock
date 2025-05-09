﻿using ReStockApi.Models;

namespace ReStockApi.Services.ReorderLog
{
    public interface IReorderLogService
    {
        Task<IEnumerable<ReOrderLog>> GetLogsAsync(DateTime fromdate, string type, string no, string storeNo);
        Task LogAsync(int storeNo, string ItemNo, int quantity, string eventType, string description, bool ordered);
    }
}

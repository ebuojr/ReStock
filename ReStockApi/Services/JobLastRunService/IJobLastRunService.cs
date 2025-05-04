using ReStockApi.Models;

namespace ReStockApi.Services.JobLastRunService
{
    public interface IJobLastRunService
    {
        Task<JobLastRun> GetLastRunByType(string type);
        Task UpdateJobLastRunAsync(string type, DateTime lastRun);
    }
}

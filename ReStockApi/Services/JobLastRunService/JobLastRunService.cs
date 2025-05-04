using Microsoft.EntityFrameworkCore;
using ReStockApi.Models;

namespace ReStockApi.Services.JobLastRunService
{
    public class JobLastRunService : IJobLastRunService
    {
        private readonly ReStockDbContext _db;
        public JobLastRunService(ReStockDbContext db)
        {
            _db = db;
        }
        public async Task<JobLastRun> GetLastRunByType(string jobName)
            => await _db.JobLastRuns.FirstOrDefaultAsync(job => job.Type == jobName);

        public async Task UpdateJobLastRunAsync(string jobName, DateTime lastRun)
        {
            var job = await GetLastRunByType(jobName);
            if (job != null)
            {
                job.LastRunTime = lastRun;
                await _db.SaveChangesAsync();
            }
        }
    }
}

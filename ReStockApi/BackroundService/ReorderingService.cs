
namespace ReStockApi.BackroundService
{
    public class ReorderingService : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (Helper.WorkerServiceSetting.IsReadyToRun)
                {
                    Console.WriteLine("ReorderingService is running...");
                    var startTime = DateTime.Now;
                    Helper.WorkerServiceSetting.IsRunning = true;
                    Helper.WorkerServiceSetting.IsReadyToRun = false;

                    // Simulate some work being done
                    await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);

                    Helper.WorkerServiceSetting.LastRun = startTime;
                    Helper.WorkerServiceSetting.IsRunning = false;
                    Console.WriteLine($"ReorderingService executed at: {startTime}");
                }

                await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
            }
        }
    }
}

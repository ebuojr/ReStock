using ReStockApi.Services.JobLastRunService;
using ReStockApi.Services.Threshold;


namespace ReStockApi.BackroundService
{
    public class ReorderingService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public ReorderingService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

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

                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var _jobLastRunService = scope.ServiceProvider.GetRequiredService<IJobLastRunService>();
                        var _thresholdService = scope.ServiceProvider.GetRequiredService<IThresholdService>();

                        var lastrun = await _jobLastRunService.GetLastRunByType("ThresholdProductSync");
                        if (lastrun.LastRunTime <= DateTime.Now.AddMinutes(-1))
                        {
                            await _thresholdService.SyncThresholds();
                            await _jobLastRunService.UpdateJobLastRunAsync("ThresholdProductSync", startTime);
                            Console.WriteLine($"ThresholdProductSync executed at: {startTime}");
                        }
                    }

                    Helper.WorkerServiceSetting.LastRun = startTime;
                    Helper.WorkerServiceSetting.IsRunning = false;
                    Console.WriteLine($"ReorderingService executed at: {startTime}");
                }

                await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
            }
        }
    }
}

using ReStockApi.Services.JobLastRunService;
using ReStockApi.Services.Reorder;
using ReStockApi.Services.SalesOrder;
using ReStockApi.Services.Store;
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
                try
                {
                    var startTime = DateTime.Now;
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var _jobLastRunService = scope.ServiceProvider.GetRequiredService<IJobLastRunService>();
                        var _thresholdService = scope.ServiceProvider.GetRequiredService<IThresholdService>();
                        var _storeService = scope.ServiceProvider.GetRequiredService<IStoreService>();
                        var _salesOrderServoce = scope.ServiceProvider.GetRequiredService<ISalesOrderService>();
                        var _reorderService = scope.ServiceProvider.GetRequiredService<IReorderService>();

                        var lastrun = await _jobLastRunService.GetLastRunByType("ThresholdProductSync");
                        if (lastrun.LastRunTime <= DateTime.Now.AddMinutes(-1))
                        {
                            await _thresholdService.SyncThresholds();
                            await _jobLastRunService.UpdateJobLastRunAsync("ThresholdProductSync", startTime);
                            Console.WriteLine($"ThresholdProductSync executed at: {startTime}");
                        }

                        if (Helper.WorkerServiceSetting.IsReadyToRun)
                        {

                            Helper.WorkerServiceSetting.IsRunning = true;
                            Helper.WorkerServiceSetting.IsReadyToRun = false;

                            // stores
                            var stores = await _storeService.GetAllStores();

                            // foreach stores for reorder
                            foreach (var store in stores)
                            {
                                // get all reorders
                                var reorders = await _reorderService.CreatePotentialOrdersByStoreNoAsync(store.No);

                                // create sales orders
                                if (reorders != null && reorders.Count > 0)
                                    await _salesOrderServoce.CreateSalesOrderAsync(reorders);
                            }

                            Helper.WorkerServiceSetting.LastRun = startTime;
                            Helper.WorkerServiceSetting.IsRunning = false;
                            Console.WriteLine($"ReorderingService executed at: {startTime}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    // log and sende email to the responsible person
                    Console.WriteLine($"Error in ReorderingService: {ex.Message}");
                }
                finally
                {
                    await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
                }
            }
        }
    }
}

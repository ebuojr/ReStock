
namespace ReStockApi.Helper
{
    public class WorkerServiceSetting
    {
        public static bool IsReadyToRun { get; set; } = true;
        public static bool IsRunning { get; set; } = false;
        public static DateTime LastRun { get; set; } = DateTime.MinValue;
    }
}

using Microsoft.AspNetCore.Mvc;

namespace ReStockApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkerServiceController : ControllerBase
    {
        [HttpGet("service-status")]
        public IActionResult GetServiceStatus()
        {
            return Ok(Helper.WorkerServiceSetting.IsRunning);
        }

        [HttpPost("run")]
        public IActionResult RunService()
        {
            Helper.WorkerServiceSetting.IsReadyToRun = true;
            return Ok("Service is running.");
        }
    }
}

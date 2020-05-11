using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TestAsync.Helpers;
using TestAsync.Interfaces;

namespace TestAsync.Controllers
{
    [ApiController]
    [Route("data")]
    public class DataController : ControllerBase
    {

        private readonly ILogger<DataController> _logger;
        private readonly IBusinessLogicService _service;

        public DataController(ILogger<DataController> logger, IBusinessLogicService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        public async Task<int> Get()
        {
            var index = 1000;

            CommonHelper.LogRequestTreadInfo(_logger, index);

            await Task.Delay(200);

            CommonHelper.LogRequestTreadInfo(_logger, index);

            return index;
        }

        [HttpGet("{index}")]
        public async Task<string> Get(int index)
        {
            CommonHelper.LogRequestTreadInfo(_logger, index, "Start request");

            var count = await _service.GetCountAsync(index);

            CommonHelper.LogRequestTreadInfo(_logger, index, "End request");

            return count;
        }
    }
}

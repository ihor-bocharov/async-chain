using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TestAsync.Helpers;
using TestAsync.Interfaces;

namespace TestAsync.Services
{
    public class BusinessLogicService : IBusinessLogicService
    {
        private readonly IDataService _dataService;
        private readonly ILogger<IBusinessLogicService> _logger;

        public BusinessLogicService(IDataService dataService, ILogger<IBusinessLogicService> logger)
        {
            _dataService = dataService;
            _logger = logger;
        }

        public async Task<string> GetCountAsync(int index)
        {
            CommonHelper.LogRequestTreadInfo(_logger, index);

            var count = await _dataService.GetCountAsync(index);

            CommonHelper.LogRequestTreadInfo(_logger, index);

            await _dataService.LongDataProcessingAsync(index);

            CommonHelper.LogRequestTreadInfo(_logger, index);

            return count;
        }
    }
}

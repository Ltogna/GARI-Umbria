using AgronicaNetCore.Anagrafe.BIZ.Resources;
using AgronicaNetCore.Anagrafe.BIZ.Services;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace AgronicaCoreNet6.BusinessLayer.Services.Test
{
    public class TestService : BaseServiceAnagrafeBIZ, ITestService
    {
        private readonly ILogger<TestService> _logger;

        public TestService(ILogger<TestService> logger, IServiceProvider serviceProvider, IStringLocalizer<Messages> localizer) : base(serviceProvider, localizer) { 
            _logger = logger;
            
        }

        public (string,int,DateTime) GetPippo()
        {
            return new("ciao", 1, DateTime.Now);
        }
        
    }
}

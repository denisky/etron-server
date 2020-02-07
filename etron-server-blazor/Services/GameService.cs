using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EtronServer.Services
{
    public class GameService : BackgroundService
    {
        private readonly ILogger<GameService> _logger;
        private Timer _timer;

        public GameService(ILogger<GameService> logger)
        {
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
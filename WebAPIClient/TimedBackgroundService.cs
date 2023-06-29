using Microsoft.Extensions.Hosting;
using Polly;
using Polly.Retry;

namespace WebAPIClient
{
    public class TimedBackgroundService : IHostedService, IDisposable
    {
        private Timer _timer;
        static int maxAttempt = 2;
        static int time = 2;

        AsyncRetryPolicy retryPolicy = Policy.
             Handle<HttpRequestException>(exeption => { return true; })
            .Or<Exception>(exeption => { return true; })
            .WaitAndRetryAsync(maxAttempt, time => TimeSpan.FromSeconds(2 * time));

        public Task StartAsync(CancellationToken stoppingToken)
        {
            RateStorage.MarkRatesAsExpires();
            {
                _timer = new Timer(state => retryPolicy.ExecuteAsync(RateStorage.GetAndSaveRates).Wait(), null, TimeSpan.Zero, TimeSpan.FromMinutes(60));
                return Task.CompletedTask;
            }
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _timer.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer.Dispose();
        }
    }
}


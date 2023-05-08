using System.Threading.RateLimiting;

namespace InvestmentTracker.AlphaVantage
{
    public enum AlphaVantageApiLimit { Free, Premium, Unlimited };

    public class AlphaVantageClientFactory
    {
        public static AlphaVantageClient GetClient(string apiKey, HttpClient httpClient, bool disposeHttpClient, RateLimiter rateLimiter, bool disposeRateLimiter)
        {
            return new AlphaVantageClient(apiKey, httpClient, disposeHttpClient, rateLimiter, disposeRateLimiter);
        }

        public static AlphaVantageClient GetClient(string apiKey, AlphaVantageApiLimit limit)
        {
            RateLimiter rateLimiter;

            switch (limit)
            {
                case AlphaVantageApiLimit.Free:
                    FixedWindowRateLimiterOptions options = new FixedWindowRateLimiterOptions();
                    options.PermitLimit = 5;
                    options.QueueLimit = Int32.MaxValue;
                    options.AutoReplenishment = true;
                    options.Window = TimeSpan.FromMinutes(1);

                    rateLimiter = new FixedWindowRateLimiter(options);
                    break;
                default:
                    rateLimiter = GetNoOpRateLimiter();
                    break;
            }

            return GetClient(apiKey, new HttpClient(), true, rateLimiter, true);
        }

        public static AlphaVantageClient GetClient(string apiKey)
        {
            return GetClient(apiKey, AlphaVantageApiLimit.Unlimited);
        }

        private static RateLimiter GetNoOpRateLimiter()
        {
            // This would be a bit simpler if the NoopLimiter were exposed publicly, but as it is the only way to get
            // to it is to go through a RateLimitPartition. The actual parameters here don't matter for a NoopLimiter
            // , so just for simplicity we'll use null.
            return RateLimitPartition.GetNoLimiter<AlphaVantageClient?>(null).Factory.Invoke(null);
        }
    }
}

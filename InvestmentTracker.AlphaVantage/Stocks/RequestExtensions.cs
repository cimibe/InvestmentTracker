using InvestmentTracker.Common.Api;

namespace InvestmentTracker.AlphaVantage.Stocks
{
    public static class RequestExtensions
    {
        public static Request AddAdjusted(this Request request, bool adjusted)
        {
            return adjusted ? request.AddGetParameter("adjusted", "true") : request.AddGetParameter("adjusted", "false");
        }

        public static Request AddIntradayInterval(this Request request, IntradayInterval interval)
        {
            string intervalString;
            switch (interval)
            {
                case IntradayInterval.OneMinute:
                    intervalString = "1min";
                    break;
                case IntradayInterval.FiveMinutes:
                    intervalString = "5min";
                    break;
                case IntradayInterval.FifteenMinutes:
                    intervalString = "15min";
                    break;
                case IntradayInterval.ThirtyMinutes:
                    intervalString = "30min";
                    break;
                case IntradayInterval.OneHour:
                    intervalString = "60min";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(interval));
            }

            return request.AddGetParameter("interval", intervalString);
        }

        public static Request AddSymbol(this Request request, string symbol)
        {
            return request.AddGetParameter("symbol", symbol);
        }

        public static Request AddOutputSize(this Request request, OutputSize outputSize)
        {
            switch (outputSize)
            {
                case OutputSize.Compact:
                    return request.AddGetParameter("outputsize", "compact");
                case OutputSize.Full:
                    return request.AddGetParameter("outputsize", "full");
                default:
                    throw new ArgumentException(nameof(outputSize));
            }
        }
    }
}

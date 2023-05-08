using System.Threading.RateLimiting;

using InvestmentTracker.Common.Api;

namespace InvestmentTracker.AlphaVantage
{
    public class AlphaVantageClient : ApiClient
    {
        private const string BaseUrl = "https://www.alphavantage.co/query";
        internal AlphaVantageClient(string apiKey, HttpClient httpClient, bool disposeHttpClient, RateLimiter limiter, bool disposeLimiter)
            : base(apiKey, httpClient, disposeHttpClient, limiter, disposeLimiter)
        {
        }

        protected override Request AppendApiKey(Request request, string apiKey)
        {
            return request.AddGetParameter("apikey", apiKey);
        }

        public Request GetRequest()
        {
            return new Request(BaseUrl);
        }

        public Request GetRequest(string function)
        {
            return new Request(BaseUrl).AddGetParameter("function", function);
        }
    }
}
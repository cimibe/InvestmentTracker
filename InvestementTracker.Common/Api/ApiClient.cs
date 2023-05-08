using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.RateLimiting;

namespace InvestmentTracker.Common.Api
{
    public abstract class ApiClient : IDisposable
    {
        private string _apiKey;

        private HttpClient _httpClient;
        private bool _disposeHttpClient;

        private RateLimiter _limiter;
        private bool _disposeLimiter;

        protected ApiClient(string apiKey, HttpClient httpClient, bool disposeHttpClient, RateLimiter limiter, bool disposeLimiter)
        {
            _apiKey = apiKey;

            _httpClient = httpClient;
            _disposeHttpClient = disposeHttpClient;

            _limiter = limiter;
            _disposeLimiter = disposeLimiter;
        }

        protected abstract Request AppendApiKey(Request request, string apiKey);

        public void Dispose()
        {
            if (_disposeHttpClient)
            {
                _httpClient.Dispose();
            }

            if (_disposeLimiter)
            {
                _limiter.Dispose();
            }

            GC.SuppressFinalize(this);
        }

        public async Task<T?> SendRequestAsync<T>(Request request, JsonSerializerOptions? options = default, CancellationToken cancellation = default)
        {
            using RateLimitLease lease = await _limiter.AcquireAsync(1, cancellation);

            return await _httpClient.GetFromJsonAsync<T>(AppendApiKey(request, _apiKey).ToUri(), options, cancellation);
        }

        // This is temporary, but helpful for testing purposes.
        public async Task<T?> TestSendRequestAsync<T>(Request request, JsonSerializerOptions? options = default, CancellationToken cancellation = default)
        {
            using RateLimitLease lease = await _limiter.AcquireAsync(1, cancellation);

            HttpResponseMessage response = await _httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, AppendApiKey(request, _apiKey).ToUri()));

            StreamReader reader = new StreamReader(response.Content.ReadAsStream());
            string rawResponse = reader.ReadToEnd();

            return JsonSerializer.Deserialize<T>(rawResponse, options);
        }
    }
}

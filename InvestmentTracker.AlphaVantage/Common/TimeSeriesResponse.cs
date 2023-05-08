using System.Text.Json.Serialization;

namespace InvestmentTracker.AlphaVantage.Common
{
    public class TimeSeriesResponse<M, T>
        where M : new()
    {
        [JsonPropertyName("Meta Data")]
        public M MetaData { get; set; }

        public TimeSeries<T> Series { get; set; }

        public TimeSeriesResponse()
        {
            MetaData = new();
            Series = new();
        }
    }
}

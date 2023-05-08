using InvestmentTracker.AlphaVantage.Common;
using System.Text.Json.Serialization;

namespace InvestmentTracker.AlphaVantage.Stocks
{
    public class IntradayStockTimeSeriesMetadata
    {
        [JsonPropertyName("1. Information")]
        public string Information { get; set; }

        [JsonPropertyName("2. Symbol")]
        public string Symbol { get; set; }

        [JsonPropertyName("3. Last Refreshed")]
        public DateTime LastRefreshed { get; set; }

        [JsonPropertyName("4. Interval")]
        public string Interval { get; set; }

        [JsonPropertyName("5. Output Size")]
        public string OutputSize { get; set; }

        [JsonPropertyName("6. Time Zone")]
        public string TimeZone { get; set; }

        public IntradayStockTimeSeriesMetadata()
        {
            Information = string.Empty;
            Symbol = string.Empty;
            LastRefreshed = DateTime.MinValue;
            Interval = string.Empty;
            OutputSize = string.Empty;
            TimeZone = string.Empty;
        }
    }

    public class StockTimeSeriesMetadata
    {
        [JsonPropertyName("1. Information")]
        public string Information { get; set; }

        [JsonPropertyName("2. Symbol")]
        public string Symbol { get; set; }

        [JsonPropertyName("3. Last Refreshed")]
        public DateTime LastRefreshed { get; set; }

        [JsonPropertyName("4. Output Size")]
        public string OutputSize { get; set; }

        [JsonPropertyName("5. Time Zone")]
        public string TimeZone { get; set; }

        public StockTimeSeriesMetadata()
        {
            Information = string.Empty;
            Symbol = string.Empty;
            LastRefreshed = DateTime.MinValue;
            OutputSize = string.Empty;
            TimeZone = string.Empty;
        }
    }

    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public class StockDataPoint
    {
        [JsonPropertyName("1. open")]
        public decimal Open { get; set; }

        [JsonPropertyName("2. high")]
        public decimal High { get; set; }

        [JsonPropertyName("3. low")]
        public decimal Low { get; set; }

        [JsonPropertyName("4. close")]
        public decimal Close { get; set; }

        [JsonPropertyName("5. volume")]
        public long Volume { get; set; }

        public StockDataPoint() { }
    }

    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public class AdjustedStockDataPoint
    {
        [JsonPropertyName("1. open")]
        public decimal Open { get; set; }

        [JsonPropertyName("2. high")]
        public decimal High { get; set; }

        [JsonPropertyName("3. low")]
        public decimal Low { get; set; }

        [JsonPropertyName("4. close")]
        public decimal Close { get; set; }

        [JsonPropertyName("5. adjusted close")]
        public decimal AdjustedClose { get; set; }

        [JsonPropertyName("6. volume")]
        public long Volume { get; set; }

        [JsonPropertyName("7. dividend amount")]
        public decimal Dividend { get; set; }

        [JsonPropertyName("8. split coefficient")]
        public decimal SplitCoefficient { get; set; }

        public AdjustedStockDataPoint() { }
    }

    public class IntradayStockTimeSeries : TimeSeriesResponse<IntradayStockTimeSeriesMetadata, StockDataPoint> { }

    public class StockTimeSeries : TimeSeriesResponse<StockTimeSeriesMetadata, StockDataPoint> { }

    public class AdjustedStockTimeSeries : TimeSeriesResponse<StockTimeSeriesMetadata, AdjustedStockTimeSeries> { }
}

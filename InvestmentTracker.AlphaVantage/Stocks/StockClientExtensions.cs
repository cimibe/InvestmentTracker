using InvestmentTracker.Common.Api;
using System.Text.Json;

namespace InvestmentTracker.AlphaVantage.Stocks
{
    public static class StockClientExtensions
    {
        private static SeriesNamingPolicy DailySeriesNaming = new SeriesNamingPolicy(StockHistoryInterval.Daily, false);
        private static SeriesNamingPolicy WeeklySeriesNaming = new SeriesNamingPolicy(StockHistoryInterval.Weekly, false);
        private static SeriesNamingPolicy MonthlySeriesNaming = new SeriesNamingPolicy(StockHistoryInterval.Monthly, false);

        private static SeriesNamingPolicy DailyAdjustedSeriesNaming = new SeriesNamingPolicy(StockHistoryInterval.Daily, true);
        private static SeriesNamingPolicy WeeklyAdjustedSeriesNaming = new SeriesNamingPolicy(StockHistoryInterval.Weekly, true);
        private static SeriesNamingPolicy MonthlyAdjustedSeriesNaming = new SeriesNamingPolicy(StockHistoryInterval.Monthly, true);


        private static Task<T?> SendTimeSeriesRequestAsync<T>(this AlphaVantageClient client, string function, string symbol, OutputSize size, CancellationToken cancellation, JsonNamingPolicy policy)
        {
            using (Request request = client.GetRequest(function))
            {
                request.AddSymbol(symbol).AddOutputSize(size);
                return client.SendRequestAsync<T>(request, new JsonSerializerOptions { PropertyNamingPolicy = policy }, cancellation);
            }
        }

        /* The following APIs haven't been tested yet, so commenting them out.
         * Parameter naming is probably going to be an issue here.
        public static Task<StockTimeSeries?> GetStockIntradaySeries(
            this AlphaVantageClient client
            , string symbol
            , IntradayInterval interval
            , OutputSize size = OutputSize.Compact
            , CancellationToken cancellation = default)
        {
            using (Request request = client.GetRequest("TIME_SERIES_INTRADAY"))
            {
                request.AddSymbol(symbol).AddOutputSize(size).AddIntradayInterval(interval).AddAdjusted(false);
                return client.SendRequestAsync<StockTimeSeries>(request, null, cancellation);
            }
        }

        public static Task<StockTimeSeries?> GetStockAdjustedIntradaySeries(
            this AlphaVantageClient client
            , string symbol
            , IntradayInterval interval
            , OutputSize size = OutputSize.Compact
            , CancellationToken cancellation = default)
        {
            using (Request request = client.GetRequest("TIME_SERIES_INTRADAY"))
            {
                request.AddSymbol(symbol).AddOutputSize(size).AddIntradayInterval(interval).AddAdjusted(true);
                return client.SendRequestAsync<StockTimeSeries>(request, null, cancellation);
            }
        }
        */

        public static Task<StockTimeSeries?> GetStockSeries(
            this AlphaVantageClient client
            , string symbol
            , StockHistoryInterval interval
            , OutputSize size = OutputSize.Compact
            , CancellationToken cancellation = default)
        {
            switch (interval)
            {
                case StockHistoryInterval.Daily:
                    return SendTimeSeriesRequestAsync<StockTimeSeries>(client, "TIME_SERIES_DAILY", symbol, size, cancellation, DailySeriesNaming);
                case StockHistoryInterval.Weekly:
                    return SendTimeSeriesRequestAsync<StockTimeSeries>(client, "TIME_SERIES_WEEKLY", symbol, size, cancellation, WeeklySeriesNaming);
                case StockHistoryInterval.Monthly:
                    return SendTimeSeriesRequestAsync<StockTimeSeries>(client, "TIME_SERIES_MONTHLY", symbol, size, cancellation, MonthlySeriesNaming);
                default:
                    throw new ArgumentOutOfRangeException(nameof(interval));
            }
        }

        public static Task<AdjustedStockTimeSeries?> GetStockAdjustedSeries(
            this AlphaVantageClient client
            , string symbol
            , StockHistoryInterval interval
            , OutputSize size = OutputSize.Compact
            , CancellationToken cancellation = default)
        {
            switch (interval)
            {
                case StockHistoryInterval.Daily:
                    return SendTimeSeriesRequestAsync<AdjustedStockTimeSeries>(client, "TIME_SERIES_DAILY_ADJUSTED", symbol, size, cancellation, DailyAdjustedSeriesNaming);
                case StockHistoryInterval.Monthly:
                    return SendTimeSeriesRequestAsync<AdjustedStockTimeSeries>(client, "TIME_SERIES_WEEKLY_ADJUSTED", symbol, size, cancellation, WeeklyAdjustedSeriesNaming);
                case StockHistoryInterval.Weekly:
                    return SendTimeSeriesRequestAsync<AdjustedStockTimeSeries>(client, "TIME_SERIES_MONTHLY_ADJUSTED", symbol, size, cancellation, MonthlyAdjustedSeriesNaming);
                default:
                    throw new ArgumentOutOfRangeException(nameof(interval));
            }
        }
    }
}
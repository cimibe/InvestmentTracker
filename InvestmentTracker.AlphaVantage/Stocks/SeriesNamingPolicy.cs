using System.Text.Json;

namespace InvestmentTracker.AlphaVantage.Stocks
{
    /// <summary>
    /// There are two characteristics of the AlphaVantage API that requires special-casing:
    /// 1. Parameters are numbered, so in the case of parameters that appear in one output sequence and not others, the parameter name changes.
    /// 2. The parameter name specifically for the Series itself changes with each API type.
    /// 
    /// This naming policy attempts to address this by providing correct property names for the different cases so that we don't need separate
    /// contracts for each combination.
    /// </summary>
    public class SeriesNamingPolicy : JsonNamingPolicy
    {
        private readonly bool _adjusted;
        private readonly StockHistoryInterval _seriesInterval;

        public SeriesNamingPolicy(StockHistoryInterval interval, bool adjusted)
        {
            _adjusted = adjusted;
            _seriesInterval = interval;
        }

        public override string ConvertName(string name)
        {
            switch (name)
            {
                case "Series":
                    switch (_seriesInterval)
                    {
                        case StockHistoryInterval.Daily:
                            return "Time Series (Daily)";
                        case StockHistoryInterval.Weekly:
                            return _adjusted ? "Weekly Adjusted Time Series" : "Weekly Time Series";
                        case StockHistoryInterval.Monthly:
                            return _adjusted ? "Monthly Adjusted Time Series" : "Monthly Time Series";
                        default:
                            throw new IndexOutOfRangeException(nameof(_seriesInterval));
                    }
                case "5. Time Zone":
                    switch (_seriesInterval)
                    {
                        case StockHistoryInterval.Daily:
                            return name;
                        case StockHistoryInterval.Weekly:
                        case StockHistoryInterval.Monthly:
                            return "4. Time Zone";
                        default:
                            throw new IndexOutOfRangeException(nameof(_seriesInterval));
                    }
                default:
                    return name;
            }
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as SeriesNamingPolicy);
        }

        public bool Equals(SeriesNamingPolicy? policy)
        {
            return policy != null && _seriesInterval == policy._seriesInterval;
        }

        public override int GetHashCode()
        {
            return _seriesInterval.GetHashCode();
        }

        public override string ToString()
        {
            return _seriesInterval.ToString();
        }
    }
}

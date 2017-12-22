
namespace MoneyMarket.Business.CryptoCurrency.Tickers
{
    public class JsonTicker
    {
        //bitstamp and btcturk values
        public string pair { get; set; }
        public decimal high { get; set; }
        public decimal last { get; set; }
        public decimal timestamp { get; set; }
        public decimal bid { get; set; }
        public decimal volume { get; set; }
        public decimal low { get; set; }
        public decimal ask { get; set; }
        public decimal open { get; set; }
        public decimal average { get; set; }
        
        //coinmarketcap values
        public string symbol { get; set; }
        public decimal price_usd { get; set; } 
    }
}


/*
 "pair":"BTCTRY",
 "high":46555.0,
 "last":46550.0,
 "timestamp":1512300388.0,
 "bid":46402.0,
 "volume":253.08,
 "low":45254.0,"
 "ask":46499.0,
 "open":46260.0,
 "average":45863.27
*/

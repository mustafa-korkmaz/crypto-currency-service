
namespace MoneyMarket.Business.CryptoCurrency
{
    /// <summary>
    /// doviz.com api object
    /// </summary>
    public class JsonCurrency
    {
        public decimal selling { get; set; }
        public int update_date { get; set; }
        public int currency { get; set; }
        public decimal buying { get; set; }
        public decimal change_rate { get; set; }
        public string name { get; set; }
        public string full_name { get; set; }
        public string code { get; set; }
    }
}


/*
{
    "selling":3.9141,
    "update_date":1512248398,
    "currency":1,
    "buying":3.9125,
    "change_rate":-0.12248334991962,
    "name":"amerikan-dolari",
    "full_name":"Amerikan Dolar\u0131",
    "code":"USD"
}
*/

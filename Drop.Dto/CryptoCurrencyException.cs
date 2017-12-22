using System;

namespace MoneyMarket.Dto
{
    public class CryptoCurrencyException : DtoBase
    {
        public string Message { get; set; }

        public string StackTrace { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}

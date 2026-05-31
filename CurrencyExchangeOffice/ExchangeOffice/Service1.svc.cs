using System;

namespace ExchangeOffice
{
    public class Service1 : IService1
    {
        public decimal CalculateExchange(string fromCurrency, string toCurrency, decimal amount)
        {
            throw new NotImplementedException("Will be implemented in Lab 6");
        }

        public string GetExchangeRate(string currencyCode)
        {
            throw new NotImplementedException("Will be implemented in Lab 7");
        }

        public string GetAvailableCurrencies()
        {
            throw new NotImplementedException("Will be implemented in Lab 7");
        }
    }
}
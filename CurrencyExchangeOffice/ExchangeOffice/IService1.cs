using System.ServiceModel;

namespace ExchangeOffice
{
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        decimal CalculateExchange(string fromCurrency, string toCurrency, decimal amount);

        [OperationContract]
        string GetExchangeRate(string currencyCode);

        [OperationContract]
        string GetAvailableCurrencies();
    }
}
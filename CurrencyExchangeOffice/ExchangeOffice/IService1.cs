using System.ServiceModel;

namespace ExchangeOffice
{
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        string GetExchangeRate(string currencyCode);

        [OperationContract]
        decimal CalculateExchange(string fromCurrency, string toCurrency, decimal amount);

        [OperationContract]
        string GetAvailableCurrencies();

        [OperationContract]
        string RegisterUser(string username, string password);

        [OperationContract]
        string LoginUser(string username, string password);
    }
}
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

        [OperationContract]
        string GetBalance(string username);

        [OperationContract]
        string TopUpAccount(string username, decimal amount);

        [OperationContract]
        string BuyCurrency(string username, string currencyCode, decimal amount);

        [OperationContract]
        string SellCurrency(string username, string currencyCode, decimal amount);
        
        [OperationContract]
        string GetTransactionHistory(string username);
    }
}
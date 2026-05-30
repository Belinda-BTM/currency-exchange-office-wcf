using System.ServiceModel;

namespace Lab2.CurrencyService
{
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        string GetExchangeRate(string currencyCode);

        [OperationContract]
        string GetMultipleRates(string currencyCodes);
    }
}
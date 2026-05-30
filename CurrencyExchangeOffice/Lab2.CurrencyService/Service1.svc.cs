using System;
using System.Net;

namespace Lab2.CurrencyService
{
    public class Service1 : IService1
    {
        public string GetExchangeRate(string currencyCode)
        {
            try
            {
                string url = $"http://api.nbp.pl/api/exchangerates/rates/A/{currencyCode}/?format=json";

                using (WebClient client = new WebClient())
                {
                    string response = client.DownloadString(url);
                    return response;
                }
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
    }
}
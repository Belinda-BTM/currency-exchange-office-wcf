using System;
using System.Net;
using System.Web.Script.Serialization;

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
                    client.Encoding = System.Text.Encoding.UTF8;
                    string json = client.DownloadString(url);

                    var serializer = new JavaScriptSerializer();
                    dynamic data = serializer.DeserializeObject(json);

                    string currency = data["currency"];
                    string code = data["code"];
                    decimal rate = data["rates"][0]["mid"];
                    string date = data["rates"][0]["effectiveDate"];

                    return $"Currency: {currency} ({code})\nRate: {rate} PLN\nDate: {date}";
                }
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
    }
}
using System;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;

namespace ExchangeOffice
{
    public class Service1 : IService1
    {
        private decimal GetRateFromNBP(string currencyCode)
        {
            if (currencyCode.ToUpper() == "PLN")
                return 1.0m;

            string url = $"http://api.nbp.pl/api/exchangerates/rates/A/{currencyCode}/?format=json";

            using (WebClient client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                string json = client.DownloadString(url);
                var serializer = new JavaScriptSerializer();
                dynamic data = serializer.DeserializeObject(json);
                return data["rates"][0]["mid"];
            }
        }

        public decimal CalculateExchange(string fromCurrency, string toCurrency, decimal amount)
        {
            try
            {
                decimal fromRate = GetRateFromNBP(fromCurrency.ToUpper());
                decimal toRate = GetRateFromNBP(toCurrency.ToUpper());

                decimal amountInPLN = amount * fromRate;
                decimal result = amountInPLN / toRate;

                return Math.Round(result, 2);
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public string GetExchangeRate(string currencyCode)
        {
            try
            {
                string url = $"http://api.nbp.pl/api/exchangerates/rates/A/{currencyCode}/?format=json";

                using (WebClient client = new WebClient())
                {
                    client.Encoding = Encoding.UTF8;
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
            catch (WebException)
            {
                return $"Error: '{currencyCode}' is not a valid currency code.";
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        public string GetAvailableCurrencies()
        {
            try
            {
                string url = "http://api.nbp.pl/api/exchangerates/tables/A/?format=json";

                using (WebClient client = new WebClient())
                {
                    client.Encoding = Encoding.UTF8;
                    string json = client.DownloadString(url);
                    var serializer = new JavaScriptSerializer();
                    dynamic data = serializer.DeserializeObject(json);

                    StringBuilder result = new StringBuilder();
                    result.AppendLine("Available currencies:");
                    result.AppendLine("====================");

                    var rates = data[0]["rates"];
                    foreach (var rate in rates)
                    {
                        result.AppendLine($"{rate["code"]} - {rate["currency"]}");
                    }

                    return result.ToString();
                }
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
    }
}
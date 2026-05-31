using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;

namespace ExchangeOffice
{
    public class Service1 : IService1
    {
        private static Dictionary<string, string> users = new Dictionary<string, string>();

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
                return Math.Round(amountInPLN / toRate, 2);
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
                        result.AppendLine($"{rate["code"]} - {rate["currency"]}");
                    return result.ToString();
                }
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        public string RegisterUser(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return "Error: Username and password cannot be empty.";
            if (users.ContainsKey(username))
                return $"Error: Username '{username}' is already taken.";
            users[username] = password;
            return $"Success: User '{username}' registered successfully!";
        }

        public string LoginUser(string username, string password)
        {
            if (!users.ContainsKey(username))
                return "Error: User not found.";
            if (users[username] != password)
                return "Error: Incorrect password.";
            return $"Success: Welcome back, {username}!";
        }
    }
}
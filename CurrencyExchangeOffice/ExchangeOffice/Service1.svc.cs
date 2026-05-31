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
        private static Dictionary<string, decimal> balances = new Dictionary<string, decimal>();
        private static Dictionary<string, Dictionary<string, decimal>> holdings = new Dictionary<string, Dictionary<string, decimal>>();

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
                return Math.Round(amount * fromRate / toRate, 2);
            }
            catch { return -1; }
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
            catch (WebException) { return $"Error: '{currencyCode}' is not a valid currency code."; }
            catch (Exception ex) { return "Error: " + ex.Message; }
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
            catch (Exception ex) { return "Error: " + ex.Message; }
        }

        public string RegisterUser(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return "Error: Username and password cannot be empty.";
            if (users.ContainsKey(username))
                return $"Error: Username '{username}' is already taken.";
            users[username] = password;
            balances[username] = 0;
            holdings[username] = new Dictionary<string, decimal>();
            return $"Success: User '{username}' registered successfully!";
        }

        public string LoginUser(string username, string password)
        {
            if (!users.ContainsKey(username)) return "Error: User not found.";
            if (users[username] != password) return "Error: Incorrect password.";
            return $"Success: Welcome back, {username}!";
        }

        public string GetBalance(string username)
        {
            if (!users.ContainsKey(username)) return "Error: User not found.";
            StringBuilder result = new StringBuilder();
            result.AppendLine($"PLN Balance: {balances[username]:F2} PLN");
            if (holdings[username].Count > 0)
            {
                result.AppendLine("Currency Holdings:");
                foreach (var holding in holdings[username])
                    result.AppendLine($"  {holding.Key}: {holding.Value:F2}");
            }
            return result.ToString();
        }

        public string TopUpAccount(string username, decimal amount)
        {
            if (!users.ContainsKey(username)) return "Error: User not found.";
            if (amount <= 0) return "Error: Amount must be greater than zero.";
            balances[username] += amount;
            return $"Success: Added {amount:F2} PLN. New balance: {balances[username]:F2} PLN";
        }

        public string BuyCurrency(string username, string currencyCode, decimal amount)
        {
            if (!users.ContainsKey(username)) return "Error: User not found.";
            try
            {
                decimal rate = GetRateFromNBP(currencyCode.ToUpper());
                decimal cost = amount * rate;
                if (balances[username] < cost)
                    return $"Error: Insufficient balance. Need {cost:F2} PLN, have {balances[username]:F2} PLN";
                balances[username] -= cost;
                if (!holdings[username].ContainsKey(currencyCode.ToUpper()))
                    holdings[username][currencyCode.ToUpper()] = 0;
                holdings[username][currencyCode.ToUpper()] += amount;
                return $"Success: Bought {amount:F2} {currencyCode.ToUpper()} for {cost:F2} PLN\nNew PLN balance: {balances[username]:F2}";
            }
            catch { return $"Error: '{currencyCode}' is not a valid currency code."; }
        }

        public string SellCurrency(string username, string currencyCode, decimal amount)
        {
            if (!users.ContainsKey(username)) return "Error: User not found.";
            string code = currencyCode.ToUpper();
            if (!holdings[username].ContainsKey(code) || holdings[username][code] < amount)
                return $"Error: Insufficient {code} holdings.";
            try
            {
                decimal rate = GetRateFromNBP(code);
                decimal earned = amount * rate;
                holdings[username][code] -= amount;
                balances[username] += earned;
                return $"Success: Sold {amount:F2} {code} for {earned:F2} PLN\nNew PLN balance: {balances[username]:F2}";
            }
            catch { return $"Error: '{currencyCode}' is not a valid currency code."; }
        }
    }
}
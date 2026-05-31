using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;

namespace ExchangeOffice
{
    public class Service1 : IService1
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["ExchangeOfficeDB"].ConnectionString;

        private decimal GetRateFromNBP(string currencyCode)
        {
            if (currencyCode.ToUpper() == "PLN") return 1.0m;
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
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string check = "SELECT COUNT(*) FROM Users WHERE Username=@u";
                    using (SqlCommand cmd = new SqlCommand(check, conn))
                    {
                        cmd.Parameters.AddWithValue("@u", username);
                        if ((int)cmd.ExecuteScalar() > 0)
                            return $"Error: Username '{username}' is already taken.";
                    }
                    string insert = "INSERT INTO Users (Username, Password, Balance) VALUES (@u, @p, 0)";
                    using (SqlCommand cmd = new SqlCommand(insert, conn))
                    {
                        cmd.Parameters.AddWithValue("@u", username);
                        cmd.Parameters.AddWithValue("@p", password);
                        cmd.ExecuteNonQuery();
                    }
                }
                return $"Success: User '{username}' registered successfully!";
            }
            catch (Exception ex) { return "Error: " + ex.Message; }
        }

        public string LoginUser(string username, string password)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM Users WHERE Username=@u AND Password=@p";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@u", username);
                        cmd.Parameters.AddWithValue("@p", password);
                        if ((int)cmd.ExecuteScalar() > 0)
                            return $"Success: Welcome back, {username}!";
                    }
                }
                return "Error: Invalid username or password.";
            }
            catch (Exception ex) { return "Error: " + ex.Message; }
        }

        public string GetBalance(string username)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT Balance FROM Users WHERE Username=@u";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@u", username);
                        object result = cmd.ExecuteScalar();
                        if (result == null) return "Error: User not found.";
                        decimal balance = (decimal)result;
                        return $"PLN Balance: {balance:F2} PLN";
                    }
                }
            }
            catch (Exception ex) { return "Error: " + ex.Message; }
        }

        public string TopUpAccount(string username, decimal amount)
        {
            if (amount <= 0) return "Error: Amount must be greater than zero.";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE Users SET Balance = Balance + @a WHERE Username=@u";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@a", amount);
                        cmd.Parameters.AddWithValue("@u", username);
                        cmd.ExecuteNonQuery();
                    }
                    string getBalance = "SELECT Balance FROM Users WHERE Username=@u";
                    using (SqlCommand cmd = new SqlCommand(getBalance, conn))
                    {
                        cmd.Parameters.AddWithValue("@u", username);
                        decimal newBalance = (decimal)cmd.ExecuteScalar();
                        return $"Success: Added {amount:F2} PLN. New balance: {newBalance:F2} PLN";
                    }
                }
            }
            catch (Exception ex) { return "Error: " + ex.Message; }
        }

        public string BuyCurrency(string username, string currencyCode, decimal amount)
        {
            try
            {
                decimal rate = GetRateFromNBP(currencyCode.ToUpper());
                decimal cost = amount * rate;
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    decimal balance;
                    string getBalance = "SELECT Balance FROM Users WHERE Username=@u";
                    using (SqlCommand cmd = new SqlCommand(getBalance, conn))
                    {
                        cmd.Parameters.AddWithValue("@u", username);
                        object result = cmd.ExecuteScalar();
                        if (result == null) return "Error: User not found.";
                        balance = (decimal)result;
                    }
                    if (balance < cost)
                        return $"Error: Insufficient balance. Need {cost:F2} PLN, have {balance:F2} PLN";
                    string update = "UPDATE Users SET Balance = Balance - @cost WHERE Username=@u";
                    using (SqlCommand cmd = new SqlCommand(update, conn))
                    {
                        cmd.Parameters.AddWithValue("@cost", cost);
                        cmd.Parameters.AddWithValue("@u", username);
                        cmd.ExecuteNonQuery();
                    }
                    string insert = "INSERT INTO Transactions (Username, Type, CurrencyCode, Amount, RateAtTime, PlnValue) VALUES (@u, 'BUY', @c, @a, @r, @pln)";
                    using (SqlCommand cmd = new SqlCommand(insert, conn))
                    {
                        cmd.Parameters.AddWithValue("@u", username);
                        cmd.Parameters.AddWithValue("@c", currencyCode.ToUpper());
                        cmd.Parameters.AddWithValue("@a", amount);
                        cmd.Parameters.AddWithValue("@r", rate);
                        cmd.Parameters.AddWithValue("@pln", cost);
                        cmd.ExecuteNonQuery();
                    }
                    decimal newBalance = balance - cost;
                    return $"Success: Bought {amount:F2} {currencyCode.ToUpper()} for {cost:F2} PLN\nNew balance: {newBalance:F2} PLN";
                }
            }
            catch (Exception ex) { return "Error: " + ex.Message; }
        }

        public string SellCurrency(string username, string currencyCode, decimal amount)
        {
            try
            {
                decimal rate = GetRateFromNBP(currencyCode.ToUpper());
                decimal earned = amount * rate;
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string update = "UPDATE Users SET Balance = Balance + @earned WHERE Username=@u";
                    using (SqlCommand cmd = new SqlCommand(update, conn))
                    {
                        cmd.Parameters.AddWithValue("@earned", earned);
                        cmd.Parameters.AddWithValue("@u", username);
                        cmd.ExecuteNonQuery();
                    }
                    string insert = "INSERT INTO Transactions (Username, Type, CurrencyCode, Amount, RateAtTime, PlnValue) VALUES (@u, 'SELL', @c, @a, @r, @pln)";
                    using (SqlCommand cmd = new SqlCommand(insert, conn))
                    {
                        cmd.Parameters.AddWithValue("@u", username);
                        cmd.Parameters.AddWithValue("@c", currencyCode.ToUpper());
                        cmd.Parameters.AddWithValue("@a", amount);
                        cmd.Parameters.AddWithValue("@r", rate);
                        cmd.Parameters.AddWithValue("@pln", earned);
                        cmd.ExecuteNonQuery();
                    }
                    string getBalance = "SELECT Balance FROM Users WHERE Username=@u";
                    using (SqlCommand cmd = new SqlCommand(getBalance, conn))
                    {
                        cmd.Parameters.AddWithValue("@u", username);
                        decimal newBalance = (decimal)cmd.ExecuteScalar();
                        return $"Success: Sold {amount:F2} {currencyCode.ToUpper()} for {earned:F2} PLN\nNew balance: {newBalance:F2} PLN";
                    }
                }
            }
            catch (Exception ex) { return "Error: " + ex.Message; }
        }
    }
}
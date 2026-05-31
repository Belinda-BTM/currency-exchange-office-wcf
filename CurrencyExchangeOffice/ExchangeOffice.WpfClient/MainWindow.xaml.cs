using System;
using System.Windows;
using ExchangeOffice.WpfClient.ExchangeServiceReference;

namespace ExchangeOffice.WpfClient
{
    public partial class MainWindow : Window
    {
        private Service1Client client;
        private string loggedInUser = "";

        public MainWindow()
        {
            InitializeComponent();
            client = new Service1Client();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                txtResults.Text = client.RegisterUser(txtUsername.Text, txtPassword.Password);
            }
            catch (Exception ex) { txtResults.Text = "Error: " + ex.Message; }
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string result = client.LoginUser(txtUsername.Text, txtPassword.Password);
                txtResults.Text = result;
                if (result.StartsWith("Success"))
                    loggedInUser = txtUsername.Text;
            }
            catch (Exception ex) { txtResults.Text = "Error: " + ex.Message; }
        }

        private void Balance_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(loggedInUser))
                { txtResults.Text = "Please login first."; return; }
                txtResults.Text = client.GetBalance(loggedInUser);
            }
            catch (Exception ex) { txtResults.Text = "Error: " + ex.Message; }
        }

        private void History_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(loggedInUser))
                { txtResults.Text = "Please login first."; return; }
                txtResults.Text = client.GetTransactionHistory(loggedInUser);
            }
            catch (Exception ex) { txtResults.Text = "Error: " + ex.Message; }
        }

        private void TopUp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(loggedInUser))
                { txtResults.Text = "Please login first."; return; }
                decimal amount = decimal.Parse(txtTopUp.Text.Trim());
                txtResults.Text = client.TopUpAccount(loggedInUser, amount);
            }
            catch (Exception ex) { txtResults.Text = "Error: " + ex.Message; }
        }

        private void GetRate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string code = txtCurrencyCode.Text.Trim().ToUpper();
                if (string.IsNullOrEmpty(code))
                { txtResults.Text = "Please enter a currency code."; return; }
                txtResults.Text = client.GetExchangeRate(code);
            }
            catch (Exception ex) { txtResults.Text = "Error: " + ex.Message; }
        }

        private void Calculate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string from = txtFromCurrency.Text.Trim().ToUpper();
                string to = txtToCurrency.Text.Trim().ToUpper();
                decimal amount = decimal.Parse(txtAmount.Text.Trim());
                decimal result = client.CalculateExchange(from, to, amount);
                if (result == -1)
                    txtResults.Text = "Error: Invalid currency code.";
                else
                    txtResults.Text = $"{amount} {from} = {result} {to}";
            }
            catch (Exception ex) { txtResults.Text = "Error: " + ex.Message; }
        }

        private void Buy_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(loggedInUser))
                { txtResults.Text = "Please login first."; return; }
                string code = txtTradeCurrency.Text.Trim().ToUpper();
                decimal amount = decimal.Parse(txtTradeAmount.Text.Trim());
                txtResults.Text = client.BuyCurrency(loggedInUser, code, amount);
            }
            catch (Exception ex) { txtResults.Text = "Error: " + ex.Message; }
        }

        private void Sell_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(loggedInUser))
                { txtResults.Text = "Please login first."; return; }
                string code = txtTradeCurrency.Text.Trim().ToUpper();
                decimal amount = decimal.Parse(txtTradeAmount.Text.Trim());
                txtResults.Text = client.SellCurrency(loggedInUser, code, amount);
            }
            catch (Exception ex) { txtResults.Text = "Error: " + ex.Message; }
        }
    }
}
using System;
using System.Windows;
using ExchangeOffice.WpfClient.ExchangeServiceReference;

namespace ExchangeOffice.WpfClient
{
    public partial class MainWindow : Window
    {
        private Service1Client client;

        public MainWindow()
        {
            InitializeComponent();
            client = new Service1Client();
        }

        private void GetRate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string code = txtCurrencyCode.Text.Trim().ToUpper();
                if (string.IsNullOrEmpty(code))
                {
                    txtResults.Text = "Please enter a currency code.";
                    return;
                }
                string result = client.GetExchangeRate(code);
                txtResults.Text = result;
            }
            catch (Exception ex)
            {
                txtResults.Text = "Error: " + ex.Message;
            }
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
            catch (Exception ex)
            {
                txtResults.Text = "Error: " + ex.Message;
            }
        }
    }
}
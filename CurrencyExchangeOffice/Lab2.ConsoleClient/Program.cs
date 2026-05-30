using System;
using Lab2.ConsoleClient.CurrencyServiceReference;

namespace Lab2.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Service1Client client = new Service1Client();

            Console.WriteLine("=== Single Currency Rate ===");
            Console.WriteLine(client.GetExchangeRate("USD"));
            Console.WriteLine();

            Console.WriteLine("=== Multiple Currency Rates ===");
            Console.WriteLine(client.GetMultipleRates("USD,EUR,GBP,CHF"));
            Console.WriteLine();

            Console.WriteLine("=== Invalid Currency Test ===");
            Console.WriteLine(client.GetExchangeRate("XYZ"));

            client.Close();
            Console.ReadLine();
        }
    }
}
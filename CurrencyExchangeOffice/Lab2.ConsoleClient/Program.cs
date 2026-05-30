using System;
using Lab2.ConsoleClient.CurrencyServiceReference;

namespace Lab2.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Service1Client client = new Service1Client();

            Console.WriteLine("=== Currency Exchange Rate Service ===");
            Console.WriteLine(client.GetExchangeRate("USD"));
            Console.WriteLine();
            Console.WriteLine(client.GetExchangeRate("EUR"));
            Console.WriteLine();
            Console.WriteLine(client.GetExchangeRate("GBP"));

            client.Close();
            Console.ReadLine();
        }
    }
}
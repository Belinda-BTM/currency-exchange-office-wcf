using System;
using Lab1.ConsoleClient.ServiceReference1;

namespace Lab1.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Service1Client client = new Service1Client();

            string result = client.SayHello("Belinda");
            Console.WriteLine(result);

            client.Close();
            Console.ReadLine();
        }
    }
}
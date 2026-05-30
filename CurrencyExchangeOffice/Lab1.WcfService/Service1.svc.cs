using System;

namespace Lab1.WcfService
{
    public class Service1 : IService1
    {
        public string SayHello(string name)
        {
            return "Hello, " + name + "! Welcome to WCF.";
        }
    }
}
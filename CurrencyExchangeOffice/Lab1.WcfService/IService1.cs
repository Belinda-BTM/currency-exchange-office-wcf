using System.ServiceModel;

namespace Lab1.WcfService
{
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        string SayHello(string name);
    }
}
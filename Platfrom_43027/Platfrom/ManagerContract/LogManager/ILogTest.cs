using System.ServiceModel;

namespace Gsafety.PTMS.Manager.Contract.LogManager
{
    [ServiceContract]
    public interface ILogTest
    {
        [OperationContract]
        string ShowLogName();

        [OperationContract]
        string ShowLogName2();
    }
}

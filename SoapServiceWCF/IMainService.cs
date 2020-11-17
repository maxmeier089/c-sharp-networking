using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace SoapServiceWCF
{

    [ServiceContract]
    public interface IMainService
    {

        [OperationContract]
        Task<MainData> Create(MainData data);

        [OperationContract]
        Task<MainData> Read(int id);

        [OperationContract]
        Task Update(bool createIfNotExisting, MainData data);

        [OperationContract]
        Task Delete(int id, bool errorIfNotExisting);

    }

}

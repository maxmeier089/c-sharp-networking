using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace ClientChannelFactory
{

    [ServiceContract]
    public interface IMainService
    {

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/main")]
        Task<MainData> Post(MainData data);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/main/{id}")]
        Task<MainData> Get(string id);

        [OperationContract]
        [WebInvoke(Method = "PUT", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/main/{id}?createIfNotExisting={createIfNotExisting}")]
        Task Put(string id, bool createIfNotExisting, MainData data);

        [OperationContract]
        [WebInvoke(Method = "DELETE", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/main/{id}?errorIfNotExisting={errorIfNotExisting}")]
        Task Delete(string id, bool errorIfNotExisting);

    }

}

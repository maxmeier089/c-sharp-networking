using RestServiceWCFContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace SelfHostedRestServiceWCF
{

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class MainService : IMainService
    {

        public async Task<MainData> Post(MainData data)
        {
            Console.WriteLine("POST");

            if (data == null) throw new WebFaultException<string>("Data required", HttpStatusCode.BadRequest);
            if (data.Text == null) throw new WebFaultException<string>("Text required", HttpStatusCode.BadRequest);

            await Task.Run(() => { DataStorage.Instance.Create(data); });

            return data;
        }

        public async Task<MainData> Get(string id)
        {
            Console.WriteLine("GET");

            if (int.TryParse(id, out int intId))
            {
                MainData data = await Task.Run(() => { return DataStorage.Instance.Read(intId); });

                if (data == null) throw new WebFaultException(HttpStatusCode.NotFound);

                return data;
            }
            else
            {
                throw new WebFaultException<string>("Only numbers are allowed as id", HttpStatusCode.BadRequest);
            }
        }

        public async Task Put(string id, bool createIfNotExisting, MainData data)
        {
            Console.WriteLine("PUT");

            if (data == null) throw new WebFaultException<string>("Data required", HttpStatusCode.BadRequest);
            if (data.Text == null) throw new WebFaultException<string>("Text required", HttpStatusCode.BadRequest);

            if (int.TryParse(id, out int intId))
            {
                data.Id = intId;

                if ((!createIfNotExisting) && await Task.Run(() => { return DataStorage.Instance.Read(intId); }) == null)
                {
                    throw new WebFaultException(HttpStatusCode.NotFound);
                }

                await Task.Run(() => { DataStorage.Instance.Update(data); });
            }
            else
            {
                throw new WebFaultException<string>("Only numbers are allowed as id", HttpStatusCode.BadRequest);
            }
        }

        public async Task Delete(string id, bool errorIfNotExisting)
        {
            Console.WriteLine("DELETE");

            if (int.TryParse(id, out int intId))
            {
                if (errorIfNotExisting && await Task.Run(() => { return DataStorage.Instance.Read(intId); }) == null)
                {
                    throw new WebFaultException(HttpStatusCode.NotFound);
                }

                await Task.Run(() => { DataStorage.Instance.Delete(intId); });
            }
            else
            {
                throw new WebFaultException<string>("Only numbers are allowed as id", HttpStatusCode.BadRequest);
            }
        }
    }
}

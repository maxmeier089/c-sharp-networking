using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace SoapServiceWCF
{
    public class MainService : IMainService
    {

        public async Task<MainData> Create(MainData data)
        {
            if (data == null) throw new WebFaultException<string>("Data required", HttpStatusCode.BadRequest);
            if (data.Text == null) throw new WebFaultException<string>("Text required", HttpStatusCode.BadRequest);

            await Task.Run(() => { DataStorage.Instance.Create(data); });

            return data;
        }

        public async Task<MainData> Read(int id)
        {
            MainData data = await Task.Run(() => { return DataStorage.Instance.Read(id); });

            if (data == null) throw new WebFaultException(HttpStatusCode.NotFound);

            return data;
        }

        public async Task Update(bool createIfNotExisting, MainData data)
        {
            if (data == null) throw new WebFaultException<string>("Data required", HttpStatusCode.BadRequest);
            if (data.Text == null) throw new WebFaultException<string>("Text required", HttpStatusCode.BadRequest);

            if ((!createIfNotExisting) && await Task.Run(() => { return DataStorage.Instance.Read(data.Id); }) == null)
            {
                throw new WebFaultException(HttpStatusCode.NotFound);
            }

            await Task.Run(() => { DataStorage.Instance.Update(data); });
        }

        public async Task Delete(int id, bool errorIfNotExisting)
        {
            if (errorIfNotExisting && await Task.Run(() => { return DataStorage.Instance.Read(id); }) == null)
            {
                throw new WebFaultException(HttpStatusCode.NotFound);
            }

            await Task.Run(() => { DataStorage.Instance.Delete(id); });

        }

    }
}

using ClientApp.SoapService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp
{
    public partial class MainViewModel : ViewModel
    {

        private async Task SoapCreate()
        {
            try
            {
                IMainService mainService = new MainServiceClient();

                SoapService.MainData requestData = new SoapService.MainData() { Text = RequestText };

                SoapService.MainData responseData = await mainService.CreateAsync(requestData);

                ResponseId = responseData.Id.ToString();
                ResponseText = responseData.Text;

                SoapStatus = "OK";
            }
            catch (FaultException fex)
            {
                SoapStatus = fex.Code.Name;
            }
            catch (Exception)
            {
                SoapStatus = "Error";
            }
        }

        private async Task SoapRead()
        {
            int? requestIdInt = await GetIdAsInt();

            if (!requestIdInt.HasValue) return;

            try
            {
                IMainService mainService = new MainServiceClient();

                SoapService.MainData responseData = await mainService.ReadAsync(requestIdInt.Value);

                ResponseId = responseData.Id.ToString();
                ResponseText = responseData.Text;

                SoapStatus = "OK";
            }
            catch (FaultException fex)
            {
                SoapStatus = fex.Code.Name;
            }
            catch (Exception)
            {
                SoapStatus = "Error";
            }
        }

        private async Task SoapUpdate()
        {
            int? requestIdInt = await GetIdAsInt();

            if (!requestIdInt.HasValue) return;

            try
            {
                IMainService mainService = new MainServiceClient();

                SoapService.MainData requestData = new SoapService.MainData() { Id = requestIdInt.Value, Text = RequestText };

                await mainService.UpdateAsync(CreateIfNotExisting, requestData);

                SoapStatus = "OK";
            }
            catch (FaultException fex)
            {
                SoapStatus = fex.Code.Name;
            }
            catch (Exception)
            {
                SoapStatus = "Error";
            }
        }

        private async Task SoapDelete()
        {
            int? requestIdInt = await GetIdAsInt();

            if (!requestIdInt.HasValue) return;

            try
            {
                IMainService mainService = new MainServiceClient();

                await mainService.DeleteAsync(requestIdInt.Value, ErrorIfNotExisting);

                SoapStatus = "OK";
            }
            catch (FaultException fex)
            {
                SoapStatus = fex.Code.Name;
            }
            catch (Exception)
            {
                SoapStatus = "Error";
            }
        }

    }
}

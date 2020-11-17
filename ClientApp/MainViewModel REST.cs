using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp
{
    public partial class MainViewModel : ViewModel
    {

        private HttpClient CreateRestClient()
        {
            // create HTTP client
            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri(URL)
            };

            // add header
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }

        private void ProcessRestResponse(HttpResponseMessage httpResponse)
        {
            Response = httpResponse.Content.ReadAsStringAsync().Result;
            HTTPStatus = httpResponse.StatusCode.ToString();

            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                try
                {
                    // deserialize response object
                    using (MemoryStream stream = new MemoryStream(Encoding.Unicode.GetBytes(Response)))
                    {
                        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(MainData));
                        MainData responseData = (MainData)serializer.ReadObject(stream);
                        ResponseId = responseData.Id.HasValue ? responseData.Id.Value.ToString() : "";
                        ResponseText = responseData.Text;
                    }
                }
                catch (Exception)
                {
                    ResponseText = "Deserialization error";
                }
            }
        }

        private async Task RestPost()
        {
            MainData requestData = new MainData() { Text = RequestText };

            // serialize request to JSON
            using (MemoryStream stream = new MemoryStream())
            {
                Request = await Task.Run(() =>
                {
                    // serializer
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(MainData));
                    serializer.WriteObject(stream, requestData);
                    return Encoding.Default.GetString(stream.ToArray());
                });

                Notify("Request");
            }

            string relativeURL = GetAndDisplayRelativeURL("main");

            // get response text
            await Task.Run(async () =>
            {
                try
                {
                    // HTTP POST
                    HttpResponseMessage httpResponse = await CreateRestClient().PostAsync(relativeURL, new StringContent(Request, Encoding.UTF8, "application/json"));

                    // process response
                    ProcessRestResponse(httpResponse);
                }
                catch (Exception ex)
                {
                    Response = ex.ToString();
                }
            });
        }

        private async Task RestGet()
        {
            int? requestIdInt = await GetIdAsInt();

            if (!requestIdInt.HasValue) return;

            string relativeURL = GetAndDisplayRelativeURL("main/" + requestIdInt.Value);

            // get response text
            await Task.Run(async () =>
            {
                try
                {
                    // HTTP GET
                    HttpResponseMessage httpResponse = await CreateRestClient().GetAsync(relativeURL);

                    // process response
                    ProcessRestResponse(httpResponse);
                }
                catch (Exception ex)
                {
                    Response = ex.ToString();
                }
            });
        }

        private async Task RestPut()
        {
            int? requestIdInt = await GetIdAsInt();

            if (!requestIdInt.HasValue) return;

            MainData requestData = new MainData() { Id = requestIdInt.Value, Text = RequestText };

            // serialize request to JSON
            using (MemoryStream stream = new MemoryStream())
            {
                Request = await Task.Run(() =>
                {
                    // serializer
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(MainData));
                    serializer.WriteObject(stream, requestData);
                    return Encoding.Default.GetString(stream.ToArray());
                });

                Notify("Request");
            }

            string relativeURL = GetAndDisplayRelativeURL("main/" + requestIdInt.Value + "?createIfNotExisting=" + CreateIfNotExisting);

            // get response text
            await Task.Run(async () =>
            {
                try
                {
                    // HTTP PUT
                    HttpResponseMessage httpResponse = await CreateRestClient().PutAsync(relativeURL, new StringContent(Request, Encoding.UTF8, "application/json"));

                    // display response
                    Response = httpResponse.Content.ReadAsStringAsync().Result;
                    HTTPStatus = httpResponse.StatusCode.ToString();
                }
                catch (Exception ex)
                {
                    Response = ex.ToString();
                }
            });
        }

        private async Task RestDelete()
        {
            int? requestIdInt = await GetIdAsInt();

            if (!requestIdInt.HasValue) return;

            string relativeURL = GetAndDisplayRelativeURL("main/" + requestIdInt.Value + "?errorIfNotExisting=" + ErrorIfNotExisting);

            // get response text
            await Task.Run(async () =>
            {
                try
                {
                    // HTTP DELETE
                    HttpResponseMessage httpResponse = await CreateRestClient().DeleteAsync(relativeURL);

                    // display response
                    Response = httpResponse.Content.ReadAsStringAsync().Result;
                    HTTPStatus = httpResponse.StatusCode.ToString();
                }
                catch (Exception ex)
                {
                    Response = ex.ToString();
                }
            });
        }

    }
}

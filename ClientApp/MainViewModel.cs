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
using System.Windows.Input;
using Windows.ApplicationModel.Activation;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace ClientApp
{

    public enum CommunicationMode { Rest, Soap}

    public enum CommunicationMethod { Create, Read, Update, Delete }


    public partial class MainViewModel : ViewModel
    {


        private CommunicationMode communicationMode;

        public CommunicationMode CommunicationMode
        {
            get => communicationMode;
            set
            {
                communicationMode = value;

                Notify("Create");
                Notify("Read");
                Notify("Update");
                Notify("Delete");
                Notify("RestVisibility");
                Notify("SoapVisibility");
            }
        }


        private CommunicationMethod communicationMethod;

        public CommunicationMethod CommunicationMethod
        {
            get => communicationMethod;
            set
            {
                communicationMethod = value;

                RequestIdVisible = new[] { CommunicationMethod.Read, CommunicationMethod.Update, CommunicationMethod.Delete }.Contains(CommunicationMethod) ? Visibility.Visible : Visibility.Collapsed;
                Notify("RequestIdVisible");

                RequestTextVisible = new[] { CommunicationMethod.Create, CommunicationMethod.Update }.Contains(CommunicationMethod) ? Visibility.Visible : Visibility.Collapsed;
                Notify("RequestTextVisible");

                CreateIfNotExistingVisible = CommunicationMethod == CommunicationMethod.Update ? Visibility.Visible : Visibility.Collapsed;
                Notify("CreateIfNotExistingVisible");

                ErrorIfNotExistingVisible = CommunicationMethod == CommunicationMethod.Delete ? Visibility.Visible : Visibility.Collapsed;
                Notify("ErrorIfNotExistingVisible");
            }
        }


        private string url = "http://localhost:55555/";

        public string URL { get => url; set { url = value; selectedURLPreset = null; Notify("SelectedURLPreset"); } }


        public class URLPreset
        {
            public string Name { get; set; }

            public string URL { get; set; }
        }

        public List<URLPreset> URLPresets { get; } = new List<URLPreset>()
        {
            new URLPreset() { Name = "RestServiceCore", URL = "http://localhost:55555/"},
            new URLPreset() { Name = "RestServiceWCF", URL = "http://localhost:4444/MainService.svc/"},
            new URLPreset() { Name = "SelfHostedRestServiceWCF", URL = "http://localhost:333/"}
        };

        private URLPreset selectedURLPreset;
        
        public URLPreset SelectedURLPreset
        {
            get => selectedURLPreset;
            set
            {
                selectedURLPreset = value;
                url = selectedURLPreset.URL;
                Notify("URL");
            }
        }


        public string RelativeURL { get; private set; }


        public string Create
        {
            get
            {
                switch (communicationMode)
                {
                    case CommunicationMode.Rest: return "POST";
                    case CommunicationMode.Soap: return "Create";
                }
                return "Create";
            }
        }

        public string Read
        {
            get
            {
                switch (communicationMode)
                {
                    case CommunicationMode.Rest: return "GET";
                    case CommunicationMode.Soap: return "Read";
                }
                return "Read";
            }
        }

        public string Update
        {
            get
            {
                switch (communicationMode)
                {
                    case CommunicationMode.Rest: return "PUT";
                    case CommunicationMode.Soap: return "Update";
                }
                return "Update";
            }
        }

        public string Delete
        {
            get
            {
                switch (communicationMode)
                {
                    case CommunicationMode.Rest: return "DELETE";
                    case CommunicationMode.Soap: return "Delete";
                }
                return "Delete";
            }
        }

        public Visibility RestVisibility { get { return CommunicationMode == CommunicationMode.Rest ? Visibility.Visible : Visibility.Collapsed; } }

        public Visibility SoapVisibility { get { return CommunicationMode == CommunicationMode.Soap ? Visibility.Visible : Visibility.Collapsed; } }


        public Visibility RequestIdVisible { get; set; }

        public string RequestId { get; set; } = "";


        public Visibility RequestTextVisible { get; set; }

        public string RequestText { get; set; } = "";


        public Visibility CreateIfNotExistingVisible { get; set; }

        public bool CreateIfNotExisting { get; set; } = false;


        public Visibility ErrorIfNotExistingVisible { get; set; }

        public bool ErrorIfNotExisting { get; set; } = false;


        public string HTTPStatus { get; set; }

        public string SoapStatus { get; set; }


        public string ResponseId { get; set; }

        public string ResponseText { get; set; }


        public string Request { get; private set; }

        public string Response { get; private set; }



        public ICommand MakeRequest { get; private set; }

        private async void MakeRequestImplementation()
        {
            Request = "";
            Notify("Request");

            RelativeURL = "";
            Notify("RelativeURL");

            Response = "";
            Notify("Response");

            HTTPStatus = "";
            Notify("HTTPStatus");

            SoapStatus = "";
            Notify("SoapStatus");

            ResponseId = "";
            Notify("ResponseId");

            ResponseText = "";
            Notify("ResponseText");

            if (CommunicationMode == CommunicationMode.Rest)
            {
                if (CommunicationMethod == CommunicationMethod.Create)
                {
                    await RestPost();
                }
                else if (CommunicationMethod == CommunicationMethod.Read)
                {
                    await RestGet();
                }
                else if (CommunicationMethod == CommunicationMethod.Update)
                {
                    await RestPut();
                }
                else if (CommunicationMethod == CommunicationMethod.Delete)
                {
                    await RestDelete();
                }

                Notify("Response");
                Notify("HTTPStatus");
            }
            else if (CommunicationMode == CommunicationMode.Soap)
            {
                if (CommunicationMethod == CommunicationMethod.Create)
                {
                    await SoapCreate();
                }
                else if (CommunicationMethod == CommunicationMethod.Read)
                {
                    await SoapRead();
                }
                else if (CommunicationMethod == CommunicationMethod.Update)
                {
                    await SoapUpdate();
                }
                else if (CommunicationMethod == CommunicationMethod.Delete)
                {
                    await SoapDelete();
                }

                Notify("SoapStatus");
            }

            Notify("ResponseId");
            Notify("ResponseText");
        }


        private string GetAndDisplayRelativeURL(string relativeURL)
        {
            RelativeURL = relativeURL;
            Notify("RelativeURL");
            return relativeURL;
        }

        private async Task<int?> GetIdAsInt()
        {
            if (int.TryParse(RequestId, out int requestIdInt))
            {
                return requestIdInt;
            }
            else
            {
                await new MessageDialog("Id needs to be a number", "Id error").ShowAsync();
                return null;
            }
        }


        public static MainViewModel Instance { get; private set; }


        static MainViewModel()
        {
            Instance = new MainViewModel();
        }

        MainViewModel()
        {
            MakeRequest = new RelayCommand(MakeRequestImplementation);
        }

    }
}

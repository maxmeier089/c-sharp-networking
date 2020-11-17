using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace ClientApp
{

    public sealed partial class MainPage : Page
    {

        public MainPage()
        {
            this.InitializeComponent();
            this.DataContext = MainViewModel.Instance;
        }


        private void Rest_Checked(object sender, RoutedEventArgs e)
        {
            MainViewModel.Instance.CommunicationMode = CommunicationMode.Rest;
        }

        private void Soap_Checked(object sender, RoutedEventArgs e)
        {
            MainViewModel.Instance.CommunicationMode = CommunicationMode.Soap;
        }


        private void Create_Checked(object sender, RoutedEventArgs e)
        {
            MainViewModel.Instance.CommunicationMethod = CommunicationMethod.Create;
        }

        private void Read_Checked(object sender, RoutedEventArgs e)
        {
            MainViewModel.Instance.CommunicationMethod = CommunicationMethod.Read;
        }

        private void Update_Checked(object sender, RoutedEventArgs e)
        {
            MainViewModel.Instance.CommunicationMethod = CommunicationMethod.Update;
        }

        private void Delete_Checked(object sender, RoutedEventArgs e)
        {
            MainViewModel.Instance.CommunicationMethod = CommunicationMethod.Delete;
        }

    }
}

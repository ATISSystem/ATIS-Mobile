﻿
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using ATISMobile.PublicProcedures;

namespace ATISMobile.AdvertisingManagement
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdvertisingPage : ContentPage
    {

        #region "General Properties"
        private string _title;
        public new string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged(); }
        }
        #endregion

        #region "Subroutins And Functions"

        public AdvertisingPage()
        {
            this.BindingContext = this;
            InitializeComponent();
            BtnDownloadATISMobileLastVersion.Clicked += BtnDownloadATISMobileLastVersion_Clicked;
            BtnVirtualNet.Clicked += BtnVirtualNet_Clicked;
            DownloadAndViewAdvertisingImg();
        }

        private void BtnVirtualNet_Clicked(object sender, EventArgs e)
        {
            try { OnBtnVirtualNet("https://eitaa.com/joinchat/1708654968C25ccccaab1"); }
            catch (Exception ex)
            { DisplayAlert("ATISMobile-Error", ex.Message, "OK"); }
        }

        private void OnBtnVirtualNet(string YourLink)
        { try { Device.OpenUri(new Uri(YourLink)); } catch (Exception ex) { throw ex; } }

        private async void DownloadAndViewAdvertisingImg()
        {
            try
            {
                HttpClient client = new HttpClient();
                client.MaxResponseContentBufferSize = 256000;
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer");
                Stream stream = await client.GetStreamAsync(Properties.Resources.RestfulWebServiceProtocol + "://" + Properties.Resources.RestfulWebServiceURLFirst + "/Advertising.jpg");
                Image img = new Image();
                img.Source = ImageSource.FromStream(() => stream);
                ImgAdvertising.Source = img.Source;
            }
            catch (System.Net.WebException ex)
            { await DisplayAlert("ATISMobile-Error", ATISMobilePredefinedMessages.ATISWebApiNotReachedMessage, "OK"); }
            catch (Exception ex)
            { await DisplayAlert("ATISMobile-Error", ex.Message, "OK"); }
        }


        #endregion

        #region "Events"
        #endregion

        #region "Event Handlers"
        private async void BtnDownloadATISMobileLastVersion_Clicked(object sender, EventArgs e)
        {
            try
            { await Browser.OpenAsync(Properties.Resources.DownloadLinkURL, BrowserLaunchMode.SystemPreferred); }
            catch (Exception ex)
            { await DisplayAlert("ATISMobile-Error", ex.Message, "OK"); }
        }

        #endregion

        #region "Override Methods"
        #endregion

        #region "Abstract Methods"
        #endregion

        #region "Implemented Members"
        #endregion





    }
}
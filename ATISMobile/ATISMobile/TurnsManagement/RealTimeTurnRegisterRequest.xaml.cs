using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Diagnostics;
using System.Windows.Input;
using Microsoft.Win32;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using Xamarin.Forms.Internals;
using System.Net.Http.Headers;

using ATISMobile.Enums;
using ATISMobile.Models;
using ATISMobile.PublicProcedures;
using ATISMobile.HttpClientInstance;
using ATISMobile.SecurityAlgorithmsManagement.HashingAlgorithms;
using ATISMobile.SecurityAlgorithmsManagement;


namespace ATISMobile.TurnsManagement
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RealTimeTurnRegisterRequest : ContentPage
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

        public RealTimeTurnRegisterRequest()
        {
            this.BindingContext = this;
            InitializeComponent();
            BtnSendRequest.Clicked += BtnSendRequest_Clicked; BtnInquiry.Clicked += BtnInquiry_Clicked;
            _LPPelak.Focused += _LPPelak_Focused;
            _LPSerial.Focused += _LPSerial_Focused;
        }

        private void ClearandReady(Entry Sender)
        { Sender.Text = string.Empty; BtnSendRequest.IsEnabled = false ; BtnInquiry.IsEnabled = true ; }




        #endregion

        #region "Events"
        #endregion

        #region "Event Handlers"

        private async void BtnSendRequest_Clicked(object sender, EventArgs e)
        {
            try
            {
                ((Button)sender).IsEnabled = false;
                var Action = await DisplayAlert("ATISMobile", "پلاک را تایید می کنید؟", "بله", "خیر");
                if (Action)
                {
                    var LPPelak = _LPPelak.Text.Trim();
                    var LPSerial = _LPSerial.Text.Trim();

                    await Nonce.GetNonce();
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/api/Turns/RealTimeTurnRegisterRequest");
                    var Content = ATISMobileWebApiMClassManagement.GetMobileNumber() + ";" + Hashing.GetSHA256Hash(ATISMobileWebApiMClassManagement.GetApiKey() + Nonce.CurrentNonce + ATISMobileWebApiMClassManagement.UserLast5Digit + LPPelak + LPSerial) + ";" + LPPelak + ";" + LPSerial;
                    request.Content = new StringContent(JsonConvert.SerializeObject(Content), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await HttpClientOnlyInstance.HttpClientInstance().SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    { await DisplayAlert("ATISMobile", "فرآیند با موفقیت انجام شد", "تایید"); return; }
                    else
                    { await DisplayAlert("ATISMobile-Failed", JsonConvert.DeserializeObject<string>(response.Content.ReadAsStringAsync().Result), "تایید"); }
                }
            }
            catch (System.Net.WebException ex)
            { await DisplayAlert("ATISMobile-Error", ATISMobilePredefinedMessages.ATISWebApiNotReachedMessage, "OK"); }
            catch (Exception ex)
            { await DisplayAlert("ATISMobile-Error", ex.Message, "OK"); }
        }

        private async void BtnInquiry_Clicked(object sender, EventArgs e)
        {
            try
            {
                ((Button)sender).IsEnabled = false;
                var LPPelak = _LPPelak.Text.Trim();
                var LPSerial = _LPSerial.Text.Trim();
                await Nonce.GetNonce();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/api/TruckDrivers/TruckDriverInqueryWithLicensePlate");
                var Content = ATISMobileWebApiMClassManagement.GetMobileNumber() + ";" + Hashing.GetSHA256Hash(ATISMobileWebApiMClassManagement.GetApiKey() + Nonce.CurrentNonce + ATISMobileWebApiMClassManagement.UserLast5Digit + LPPelak + LPSerial) + ";" + LPPelak + ";" + LPSerial;
                request.Content = new StringContent(JsonConvert.SerializeObject(Content), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await HttpClientOnlyInstance.HttpClientInstance().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var myTruckDriver = JsonConvert.DeserializeObject<string>(content);
                    _LblTruckDriver.Text = myTruckDriver;
                    BtnSendRequest.IsEnabled  = IsEnabled = true;
                }
                else
                { await DisplayAlert("ATISMobile-Failed", JsonConvert.DeserializeObject<string>(response.Content.ReadAsStringAsync().Result), "تایید"); }
            }
            catch (System.Net.WebException ex)
            { await DisplayAlert("ATISMobile-Error", ATISMobilePredefinedMessages.ATISWebApiNotReachedMessage, "OK"); }
            catch (Exception ex)
            { await DisplayAlert("ATISMobile-Error", ex.Message, "OK"); }
            ((Button)sender).IsEnabled = true;
        }

        private void _LPSerial_Focused(object sender, FocusEventArgs e)
        { ClearandReady((Entry)sender); }

        private void _LPPelak_Focused(object sender, FocusEventArgs e)
        { ClearandReady((Entry)sender); }


        #endregion

        #region "Override Methods"
        #endregion

        #region "Abstract Methods"
        #endregion

        #region "Implemented Members"
        #endregion

    }
}
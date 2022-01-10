using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using ATISMobile.HttpClientInstance;
using ATISMobile.PublicProcedures;
using ATISMobile.SecurityAlgorithmsManagement;
using ATISMobile.SecurityAlgorithmsManagement.HashingAlgorithms;

namespace ATISMobile.TruckDriverManagement
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SendTruckAndTruckDriverChangeRequest : ContentPage
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

        public SendTruckAndTruckDriverChangeRequest()
        {
            this.BindingContext = this;
            InitializeComponent();
            EntryPelakforTruckDriverChange.Focused += EntryPelakforTruckDriverChange_Focused;
            EntrySerialforTruckDriverChange.Focused += EntrySerialforTruckDriverChange_Focused;
            EntryTruckDriverNationalCode.Focused += EntryTruckDriverNationalCode_Focused;
            EntryPelakforTruckChange.Focused += EntryPelakforTruckChange_Focused;
            EntrySerialforTruckChange.Focused += EntrySerialforTruckChange_Focused;
            EntryNewTruckLicensePlate.Focused += EntryNewTruckLicensePlate_Focused;
            BtnSendTruckDriverChangeRequest.Clicked += BtnSendTruckDriverChangeRequest_Clicked;
            BtnCurrentTruckDriverInqueryforTruckDriverChange.Clicked += BtnCurrentTruckDriverInqueryforTruckDriverChange_Clicked;
            BtnSendTruckChangeRequest.Clicked += BtnSendTruckChangeRequest_Clicked;
            BtnCurrentTruckDriverInqueryforTruckChange.Clicked += BtnCurrentTruckDriverInqueryforTruckChange_Clicked;
        }

        private void ClearandReadyforTruckDriverChange(Entry Sender)
        {
            LblCurrentTruckDriverforTruckDriverChange.Text = string.Empty;
            Sender.Text = string.Empty;
            BtnSendTruckDriverChangeRequest.IsEnabled = false;
        }

        private void ClearandReadyforTruckChange(Entry Sender)
        {
            LblCurrentTruckDriverforTruckChange.Text = string.Empty;
            Sender.Text = string.Empty;
            BtnSendTruckChangeRequest.IsEnabled = false;
        }


        #endregion

        #region "Events"
        #endregion

        #region "Event Handlers"

        private async void BtnCurrentTruckDriverInqueryforTruckDriverChange_Clicked(object sender, EventArgs e)
        {
            try
            {
                ((Button)sender).IsEnabled = false;
                var LPPelak = EntryPelakforTruckDriverChange.Text.Trim();
                var LPSerial = EntrySerialforTruckDriverChange.Text.Trim();
                await Nonce.GetNonce();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/api/TruckDrivers/TruckDriverInqueryWithLicensePlate");
                var Content = ATISMobileWebApiMClassManagement.GetMobileNumber() + ";" + Hashing.GetSHA256Hash(ATISMobileWebApiMClassManagement.GetApiKey() + Nonce.CurrentNonce + ATISMobileWebApiMClassManagement.UserLast5Digit + LPPelak + LPSerial) + ";" + LPPelak + ";" + LPSerial;
                request.Content = new StringContent(JsonConvert.SerializeObject(Content), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await HttpClientOnlyInstance.HttpClientInstance().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var myTruckDriver = JsonConvert.DeserializeObject<string>(content);
                    LblCurrentTruckDriverforTruckDriverChange.Text = myTruckDriver;
                    BtnSendTruckDriverChangeRequest.IsEnabled = true;
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

        private async void BtnSendTruckDriverChangeRequest_Clicked(object sender, EventArgs e)
        {
            try
            {
                ((Button)sender).IsEnabled = false;
                var LPPelak = EntryPelakforTruckDriverChange.Text.Trim();
                var LPSerial = EntrySerialforTruckDriverChange.Text.Trim();
                var NewTruckDriverNationalCode = EntryTruckDriverNationalCode.Text.Trim();
                await Nonce.GetNonce();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/api/TruckDrivers/SendTruckDriverChangeRequestMessage");
                var Content = ATISMobileWebApiMClassManagement.GetMobileNumber() + ";" + Hashing.GetSHA256Hash(ATISMobileWebApiMClassManagement.GetApiKey() + Nonce.CurrentNonce + ATISMobileWebApiMClassManagement.UserLast5Digit + LPPelak + LPSerial + NewTruckDriverNationalCode) + ";" + LPPelak + ";" + LPSerial + ";" + NewTruckDriverNationalCode;
                request.Content = new StringContent(JsonConvert.SerializeObject(Content), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await HttpClientOnlyInstance.HttpClientInstance().SendAsync(request);
                if (response.IsSuccessStatusCode)
                { await DisplayAlert("ATISMobile", "فرآیند با موفقیت انجام شد", "تایید"); return; }
                else
                { await DisplayAlert("ATISMobile-Failed", JsonConvert.DeserializeObject<string>(response.Content.ReadAsStringAsync().Result), "تایید"); }
            }
            catch (System.Net.WebException ex)
            { await DisplayAlert("ATISMobile-Error", ATISMobilePredefinedMessages.ATISWebApiNotReachedMessage, "OK"); }
            catch (Exception ex)
            { await DisplayAlert("ATISMobile-Error", ex.Message, "OK"); }
        }

        private void EntryTruckDriverNationalCode_Focused(object sender, FocusEventArgs e)
        { ClearandReadyforTruckDriverChange((Entry)sender); }

        private void EntrySerialforTruckDriverChange_Focused(object sender, FocusEventArgs e)
        { ClearandReadyforTruckDriverChange((Entry)sender); }

        private void EntryPelakforTruckDriverChange_Focused(object sender, FocusEventArgs e)
        { ClearandReadyforTruckDriverChange((Entry)sender); }

        private async void BtnCurrentTruckDriverInqueryforTruckChange_Clicked(object sender, EventArgs e)
        {
            try
            {
                ((Button)sender).IsEnabled = false;
                var LPPelak = EntryPelakforTruckChange.Text.Trim();
                var LPSerial = EntrySerialforTruckChange.Text.Trim();
                await Nonce.GetNonce();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/api/TruckDrivers/TruckDriverInqueryWithLicensePlate");
                var Content = ATISMobileWebApiMClassManagement.GetMobileNumber() + ";" + Hashing.GetSHA256Hash(ATISMobileWebApiMClassManagement.GetApiKey() + Nonce.CurrentNonce + ATISMobileWebApiMClassManagement.UserLast5Digit + LPPelak + LPSerial) + ";" + LPPelak + ";" + LPSerial;
                request.Content = new StringContent(JsonConvert.SerializeObject(Content), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await HttpClientOnlyInstance.HttpClientInstance().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var myTruckDriver = JsonConvert.DeserializeObject<string>(content);
                    LblCurrentTruckDriverforTruckChange.Text = myTruckDriver;
                    BtnSendTruckChangeRequest.IsEnabled = true;
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

        private async void BtnSendTruckChangeRequest_Clicked(object sender, EventArgs e)
        {
            try
            {
                ((Button)sender).IsEnabled = false;
                var LPPelak = EntryPelakforTruckChange.Text.Trim();
                var LPSerial = EntrySerialforTruckChange.Text.Trim();
                var NewTruckLicensePlate = EntryNewTruckLicensePlate.Text.Trim();
                await Nonce.GetNonce();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/api/Trucks/SendTruckChangeRequestMessage");
                var Content = ATISMobileWebApiMClassManagement.GetMobileNumber() + ";" + Hashing.GetSHA256Hash(ATISMobileWebApiMClassManagement.GetApiKey() + Nonce.CurrentNonce + ATISMobileWebApiMClassManagement.UserLast5Digit + LPPelak + LPSerial + NewTruckLicensePlate) + ";" + LPPelak + ";" + LPSerial + ";" + NewTruckLicensePlate;
                request.Content = new StringContent(JsonConvert.SerializeObject(Content), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await HttpClientOnlyInstance.HttpClientInstance().SendAsync(request);
                if (response.IsSuccessStatusCode)
                { await DisplayAlert("ATISMobile", "فرآیند با موفقیت انجام شد", "تایید"); return; }
                else
                { await DisplayAlert("ATISMobile-Failed", JsonConvert.DeserializeObject<string>(response.Content.ReadAsStringAsync().Result), "تایید"); }
            }
            catch (System.Net.WebException ex)
            { await DisplayAlert("ATISMobile-Error", ATISMobilePredefinedMessages.ATISWebApiNotReachedMessage, "OK"); }
            catch (Exception ex)
            { await DisplayAlert("ATISMobile-Error", ex.Message, "OK"); }
        }

        private void EntrySerialforTruckChange_Focused(object sender, FocusEventArgs e)
        { ClearandReadyforTruckChange((Entry)sender); }

        private void EntryPelakforTruckChange_Focused(object sender, FocusEventArgs e)
        { ClearandReadyforTruckChange((Entry)sender); }

        private void EntryNewTruckLicensePlate_Focused(object sender, FocusEventArgs e)
        { ClearandReadyforTruckChange((Entry)sender); }

        #endregion

        #region "Override Methods"
        #endregion

        #region "Abstract Methods"
        #endregion

        #region "Implemented Members"
        #endregion

    }
}
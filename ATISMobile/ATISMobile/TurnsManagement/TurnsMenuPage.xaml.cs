
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


namespace ATISMobile.TurnsManagement
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TurnsMenuPage : ContentPage
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

        public TurnsMenuPage()
        {
            InitializeComponent();
            try
            {
                BtnTurnIssueRequestVisablity();
                this.Appearing += TurnsMenuPage_Appearing;
            }
            catch (Exception ex)
            { DisplayAlert("ATISMobile-Error", ex.Message, "OK"); }

        }

        public async void BtnTurnIssueRequestVisablity()
        {
            try
            {
                _BtnTurnIssueRequest.IsEnabled = false;
                await Nonce.GetNonce();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/api/Turns/IsVirtualTurnsActive");
                var Content = ATISMobileWebApiMClassManagement.GetMobileNumber() + ";" + Hashing.GetSHA256Hash(ATISMobileWebApiMClassManagement.GetApiKey() + Nonce.CurrentNonce);
                request.Content = new StringContent(JsonConvert.SerializeObject(Content), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await HttpClientOnlyInstance.HttpClientInstance().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (!JsonConvert.DeserializeObject<bool>(content))
                    { _BtnTurnIssueRequest.IsVisible = false; }
                }
                else
                { await DisplayAlert("ATISMobile-Failed", JsonConvert.DeserializeObject<string>(response.Content.ReadAsStringAsync().Result), "تایید"); }
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

        private void TurnsMenuPage_Appearing(object sender, EventArgs e)
        {
            _BtnGetTurns.IsEnabled = true;
            _BtnTurnIssueRequest.IsEnabled = true;
            _BtnTurnCancellationRequest.IsEnabled = true;
        }

        private async void _BtnGetTurns_ClickedEvent(object sender, EventArgs e)
        {
            _BtnGetTurns.IsEnabled = false;
            TurnsPage _TurnsPage = new TurnsPage();
            _TurnsPage.Title = "لیست نوبت ها";
            await Navigation.PushAsync(_TurnsPage);
        }

        private async void _BtnTurnIssueRequest_ClickedEvent(object sender, EventArgs e)
        {
            try
            {
                _BtnTurnIssueRequest.IsEnabled = false;
                await Nonce.GetNonce();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/api/Turns/TurnIssueRequest");
                var Content = ATISMobileWebApiMClassManagement.GetMobileNumber() + ";" + Hashing.GetSHA256Hash(ATISMobileWebApiMClassManagement.GetApiKey() + Nonce.CurrentNonce + ATISMobileWebApiMClassManagement.UserLast5Digit);
                request.Content = new StringContent(JsonConvert.SerializeObject(Content), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await HttpClientOnlyInstance.HttpClientInstance().SendAsync(request);
                if (response.IsSuccessStatusCode)
                { await DisplayAlert("ATISMobile", "درخواست با موفقیت ارسال شد", "OK"); }
                else
                { await DisplayAlert("ATISMobile-Failed", JsonConvert.DeserializeObject<string>(response.Content.ReadAsStringAsync().Result), "تایید"); }
            }
            catch (System.Net.WebException ex)
            { await DisplayAlert("ATISMobile-Error", ATISMobilePredefinedMessages.ATISWebApiNotReachedMessage, "OK"); }
            catch (Exception ex)
            { await DisplayAlert("ATISMobile-Error", ex.Message, "OK"); }

        }

        private async void _BtnTurnCancellationRequest_ClickedEvent(object sender, EventArgs e)
        {
            try
            {
                _BtnTurnCancellationRequest.IsEnabled = false;
                await Nonce.GetNonce();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/api/Turns/TurnCancellationRequest");
                var Content = ATISMobileWebApiMClassManagement.GetMobileNumber() + ";" + Hashing.GetSHA256Hash(ATISMobileWebApiMClassManagement.GetApiKey() + Nonce.CurrentNonce + ATISMobileWebApiMClassManagement.UserLast5Digit);
                request.Content = new StringContent(JsonConvert.SerializeObject(Content), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await HttpClientOnlyInstance.HttpClientInstance().SendAsync(request);
                if (response.IsSuccessStatusCode)
                { await DisplayAlert("ATISMobile", "درخواست با موفقیت ارسال شد", "OK"); }
                else
                { await DisplayAlert("ATISMobile-Failed", JsonConvert.DeserializeObject<string>(response.Content.ReadAsStringAsync().Result), "تایید"); }
            }
            catch (System.Net.WebException ex)
            { await DisplayAlert("ATISMobile-Error", ATISMobilePredefinedMessages.ATISWebApiNotReachedMessage, "OK"); }
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
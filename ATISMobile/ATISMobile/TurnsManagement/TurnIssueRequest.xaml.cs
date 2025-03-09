
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using ATISMobile.SecurityAlgorithmsManagement;
using ATISMobile.PublicProcedures;
using ATISMobile.SecurityAlgorithmsManagement.HashingAlgorithms;
using ATISMobile.HttpClientInstance;
using ATISMobile.Models;

namespace ATISMobile.TurnsManagement
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TurnIssueRequest : ContentPage
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
        public TurnIssueRequest()
        {
            this.BindingContext = this;
            InitializeComponent();
            ViewInformation();
        }

        public async void ViewInformation()
        {
            try
            {
                await Nonce.GetNonce();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/api/Turns/GetSequentialTurns");
                var Content = ATISMobileWebApiMClassManagement.GetMobileNumber() + ";" + Hashing.GetSHA256Hash(ATISMobileWebApiMClassManagement.GetApiKey() + Nonce.CurrentNonce);
                request.Content = new StringContent(JsonConvert.SerializeObject(Content), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await HttpClientOnlyInstance.HttpClientInstance().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var _List = JsonConvert.DeserializeObject<List<SequentialTurns>>(content);
                    if (_List.Count == 0)
                    { _ListView.IsVisible = false; }
                    else
                    { _ListView.ItemsSource = _List; }
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
        private async void BtnSelect_ClickedEvent(Object sender, EventArgs e)
        {
            try
            {
                ((Button)sender).IsEnabled = false;
                var Action = await DisplayAlert("ATISMobile", "انتخاب صف نوبت را تایید می کنید؟", "بله", "خیر");
                if (Action)
                {
                    var SeqTId = ((Label)((Button)sender).Parent.FindByName("LblSequentialTurnId")).Text.Trim();

                    await Nonce.GetNonce();
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/api/Turns/TurnIssueRequest");
                    var Content = ATISMobileWebApiMClassManagement.GetMobileNumber() + ";" + Hashing.GetSHA256Hash(ATISMobileWebApiMClassManagement.GetApiKey() + Nonce.CurrentNonce + ATISMobileWebApiMClassManagement.UserLast5Digit + SeqTId.ToString()) + ";" + SeqTId.ToString();
                    request.Content = new StringContent(JsonConvert.SerializeObject(Content), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await HttpClientOnlyInstance.HttpClientInstance().SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                        await DisplayAlert("ATISMobile", "نوبت برای شما صادر شد و تا لحظاتی دیگر قابل مشاهده خواهد بود", "تایید");
                        return;
                    }
                    else
                    { await DisplayAlert("ATISMobile-Failed", JsonConvert.DeserializeObject<string>(response.Content.ReadAsStringAsync().Result), "تایید"); }
                }
            }
            catch (System.Net.WebException ex)
            { await DisplayAlert("ATISMobile-Error", ATISMobilePredefinedMessages.ATISWebApiNotReachedMessage, "OK"); }
            catch (Exception ex)
            { await DisplayAlert("ATISMobile-Error", ex.Message, "OK"); }
            ((Button)sender).IsEnabled = true;
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
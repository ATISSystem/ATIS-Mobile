using ATISMobile.HttpClientInstance;
using ATISMobile.PublicProcedures;
using ATISMobile.SecurityAlgorithmsManagement;
using ATISMobile.SecurityAlgorithmsManagement.HashingAlgorithms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ATISMobile.TurnsManagement
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TurnsCancellation : ContentPage
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

        public TurnsCancellation()
        {
            try
            {
                this.BindingContext = this;
                InitializeComponent();
                BtnSendRequest.Clicked += BtnSendRequest_Clicked;
                EntryTopSequentialTurnNumber.Focused += EntryTopSequentialTurnNumber_Focused;
                EntryYearShamsi.Focused += EntryYearShamsi_Focused;
                ViewFirstActiveTurn();
            }
            catch (Exception ex)
            { DisplayAlert("ATISMobile-Error", ex.Message, "OK"); }
        }

        private void ClearandReady(Entry Sender)
        { Sender.Text = string.Empty; BtnSendRequest.IsEnabled = true; }

        private async void ViewFirstActiveTurn()
        {
            try
            {
                await Nonce.GetNonce();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/api/Turns/GetFirstActiveTurn");
                var Content = ATISMobileWebApiMClassManagement.GetMobileNumber() + ";" + Hashing.GetSHA256Hash(ATISMobileWebApiMClassManagement.GetApiKey() + Nonce.CurrentNonce);
                request.Content = new StringContent(JsonConvert.SerializeObject(Content), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await HttpClientOnlyInstance.HttpClientInstance().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var LastTurnIdWhichCancelledDuringTurnsCancellationProcess = JsonConvert.DeserializeObject<string>(content);
                    LblLastTurnIdWhichCancelledDuringTurnsCancellationProcess.Text = LastTurnIdWhichCancelledDuringTurnsCancellationProcess;
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

        private void EntryYearShamsi_Focused(object sender, FocusEventArgs e)
        { ClearandReady((Entry)sender); }

        private void EntryTopSequentialTurnNumber_Focused(object sender, FocusEventArgs e)
        { ClearandReady((Entry)sender); }

        private async void BtnSendRequest_Clicked(object sender, EventArgs e)
        {
            try
            {
                ((Button)sender).IsEnabled = false;
                var Action = await DisplayAlert("ATISMobile", "اطلاعات وارد شده را تایید می کنید؟", "بله", "خیر");
                if (Action)
                {
                    var TopSequentialTurnNumber = EntryTopSequentialTurnNumber.Text.Trim();
                    var YearShamsi = EntryYearShamsi.Text.Trim();

                    await Nonce.GetNonce();
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/api/Turns/TurnsCancellation");
                    var Content = ATISMobileWebApiMClassManagement.GetMobileNumber() + ";" + Hashing.GetSHA256Hash(ATISMobileWebApiMClassManagement.GetApiKey() + Nonce.CurrentNonce + ATISMobileWebApiMClassManagement.UserLast5Digit + TopSequentialTurnNumber + YearShamsi) + ";" + TopSequentialTurnNumber + ";" + YearShamsi;
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

        #endregion

        #region "Override Methods"
        #endregion

        #region "Abstract Methods"
        #endregion

        #region "Implemented Members"
        #endregion

    }
}
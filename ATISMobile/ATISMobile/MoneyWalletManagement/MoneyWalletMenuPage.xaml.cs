using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
using System.Net.Http.Headers;

using ATISMobile.PublicProcedures;
using ATISMobile.Models;
using ATISMobile.HttpClientInstance;
using ATISMobile.SecurityAlgorithmsManagement.HashingAlgorithms;
using ATISMobile.SecurityAlgorithmsManagement;

namespace ATISMobile.MoneyWalletManagement
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MoneyWalletMenuPage : ContentPage
    {
        #region "General Properties"
        #endregion

        #region "Subroutins And Functions"
        public MoneyWalletMenuPage()
        {
            InitializeComponent();
            try
            {
                ViewMoneyWalletReminderCharge();
                this.Appearing += MoneyWalletMenuPage_Appearing;
            }
            catch (Exception ex)
            { DisplayAlert("ATISMobile-Error", ex.Message, "OK"); }
        }

        private async void ViewMoneyWalletReminderCharge()
        {
            try
            {
                await Nonce.GetNonce();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/api/MoneyWalletReminderCharge/GetMoneyWalletReminderCharge");
                var Content = ATISMobileWebApiMClassManagement.GetMobileNumber() + ";" + Hashing.GetSHA256Hash(ATISMobileWebApiMClassManagement.GetApiKey() + Nonce.CurrentNonce );
                request.Content = new StringContent(JsonConvert.SerializeObject(Content), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await HttpClientOnlyInstance.HttpClientInstance().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    MessageStruct Result = JsonConvert.DeserializeObject<MessageStruct>(content);
                    _LblReminderCharge.Text = Result.Message1;
                    _LblReminderChargeHeader.IsVisible = true;
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
        private void MoneyWalletMenuPage_Appearing(object sender, EventArgs e)
        { _BtnMoneyWalletCharging.IsEnabled = true; _BtnMoneyWalletTransactions.IsEnabled = true; }

        private async void _BtnMoneyWalletTransactions_ClickedEvent(object sender, EventArgs e)
        {
            _BtnMoneyWalletTransactions.IsEnabled = false;
            MoneyWalletTransactionsPage _MoneyWallettTransactionsPage = new MoneyWalletTransactionsPage();
            await Navigation.PushAsync(_MoneyWallettTransactionsPage);
        }

        private async void _BtnMoneyWalletCharging_ClickedEvent(object sender, EventArgs e)
        {
            _BtnMoneyWalletCharging.IsEnabled = false;
            MoneyWalletChargingPage _MoneyWalletChargingPage = new MoneyWalletChargingPage();
            await Navigation.PushAsync(_MoneyWalletChargingPage);
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
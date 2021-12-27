using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Http.Headers;

using ATISMobile.Models;
using ATISMobile.PublicProcedures;
using ATISMobile.HttpClientInstance;
using ATISMobile.SecurityAlgorithmsManagement.HashingAlgorithms;
using ATISMobile.SecurityAlgorithmsManagement;

namespace ATISMobile.MoneyWalletManagement
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MoneyWalletTransactionsPage : ContentPage
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
        public MoneyWalletTransactionsPage()
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
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/api/MoneyWalletAccounting/GetMoneyWalletAccounting");
                var Content = ATISMobileWebApiMClassManagement.GetMobileNumber() + ";" + Hashing.GetSHA256Hash(ATISMobileWebApiMClassManagement.GetApiKey() + Nonce.CurrentNonce);
                request.Content = new StringContent(JsonConvert.SerializeObject(Content), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await HttpClientOnlyInstance.HttpClientInstance().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    List<MoneyWalletAccounting> _List = new List<MoneyWalletAccounting>();
                    _List = JsonConvert.DeserializeObject<List<MoneyWalletAccounting>>(content);
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
        private void _BtnRefreshTransactions_ClickedEvent(object sender, EventArgs e)
        {
            try
            { ViewInformation(); }
            catch (Exception ex)
            { DisplayAlert("ATISMobile-Error", ex.Message, "OK"); }
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
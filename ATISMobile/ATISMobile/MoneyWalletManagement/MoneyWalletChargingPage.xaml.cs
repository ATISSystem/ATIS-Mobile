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

using ATISMobile.Models;
using ATISMobile.PublicProcedures;
using ATISMobile.HttpClientInstance;
using ATISMobile.SecurityAlgorithmsManagement.Hashing;
using ATISMobile.SecurityAlgorithmsManagement;

namespace ATISMobile.MoneyWalletManagement
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MoneyWalletChargingPage : ContentPage
    {
        #region "General Properties"
        #endregion

        #region "Subroutins And Functions"
        public MoneyWalletChargingPage()
        { InitializeComponent(); }


        #endregion

        #region "Events"
        #endregion

        #region "Event Handlers"
        private void _Btn50000_ClickedEvent(object sender, EventArgs e)
        { _LblAmount.Text = $"{50000:n0}"; }

        private void _Btn40000_ClickedEvent(object sender, EventArgs e)
        { _LblAmount.Text = $"{40000:n0}"; }

        private void _Btn30000_ClickedEvent(object sender, EventArgs e)
        { _LblAmount.Text = $"{30000:n0}"; }

        private void _Btn20000_ClickedEvent(object sender, EventArgs e)
        { _LblAmount.Text = $"{20000:n0}"; }

        private void _Btn10000_ClickedEvent(object sender, EventArgs e)
        { _LblAmount.Text = $"{10000:n0}"; }

        private void _Btn5000_ClickedEvent(object sender, EventArgs e)
        { _LblAmount.Text = $"{5000:n0}"; }

        private async void _BtnGotoCharge_ClickedEvent(object sender, EventArgs e)
        {
            try
            {
                Int64 Amount = System.Convert.ToInt64(_LblAmount.Text.Replace(",", string.Empty));
                if (Amount.ToString() == "0" || Amount.ToString() == string.Empty)
                { throw new Exception("مبلغ مورد نظر خود را انتخاب کنید"); }

                var nonce = new Nonce();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/api/MoneyWalletChargingAPI/PaymentRequest");
                var Content = ATISMobileWebApiMClassManagement.GetMobileNumber() + ";" + Hashing.GetSHA256Hash(ATISMobileWebApiMClassManagement.GetApiKey() + nonce.CurrentNonce  + ATISMobileWebApiMClassManagement.UserLast5Digit + Amount.ToString()) + ";" + Amount.ToString();
                HttpResponseMessage response = await HttpClientOnlyInstance.HttpClientInstance().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var myMS = JsonConvert.DeserializeObject<MessageStruct>(content);
                    //NoError : Message2=https://sandbox.zarinpal.com/pg/StartPay/ Message1=Autority
                    //Error   : Message1=Error Message String
                    if (myMS.ErrorCode == false)
                    { Device.OpenUri(new Uri(myMS.Message2 + myMS.Message1)); }
                    else
                    { }
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

        #region "Override Methods"
        #endregion

        #region "Abstract Methods"
        #endregion

        #region "Implemented Members"
        #endregion



    }
}
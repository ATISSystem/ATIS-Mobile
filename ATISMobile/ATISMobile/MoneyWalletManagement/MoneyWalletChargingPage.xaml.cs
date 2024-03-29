﻿using System;
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
using ATISMobile.SecurityAlgorithmsManagement.HashingAlgorithms;
using ATISMobile.SecurityAlgorithmsManagement;

namespace ATISMobile.MoneyWalletManagement
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MoneyWalletChargingPage : ContentPage
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
        public MoneyWalletChargingPage()
        {
            this.BindingContext = this;
            InitializeComponent();
        }


        #endregion

        #region "Events"
        #endregion

        #region "Event Handlers"
   
        private async void _BtnGotoCharge_ClickedEvent(object sender, EventArgs e)
        {
            try
            {
                Int64 Amount = System.Convert.ToInt64(_EntryAmount.Text.Replace(",", string.Empty));
                if (Amount.ToString() == "0" || Amount.ToString() == string.Empty)
                { throw new Exception("مبلغ مورد نظر خود را وارد نمایید"); }

                await Nonce.GetNonce();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/api/MoneyWalletChargingAPI/PaymentRequest");
                var Content = ATISMobileWebApiMClassManagement.GetMobileNumber() + ";" + Hashing.GetSHA256Hash(ATISMobileWebApiMClassManagement.GetApiKey() + Nonce.CurrentNonce + ATISMobileWebApiMClassManagement.UserLast5Digit + Amount.ToString()) + ";" + Amount.ToString();
                request.Content = new StringContent(JsonConvert.SerializeObject(Content), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await HttpClientOnlyInstance.HttpClientInstance().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var myMS = JsonConvert.DeserializeObject<MessageStruct>(content);
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
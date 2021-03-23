﻿using System;
using System.IO;
using System.Net.Http;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using ATISMobile.Models;
using ATISMobile.PublicProcedures;
using ATISMobile.HttpClientInstance;

namespace ATISMobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VerificationCodeEntryPage : ContentPage
    {
        #region "General Properties"
        #endregion

        #region "Subroutins And Functions"
        public VerificationCodeEntryPage()
        { InitializeComponent(); }

        public void SetInf(string YourVerificationCode, string YourMobileNumber)
        { _LabelMobileNumber.Text = YourMobileNumber; }

        protected override bool OnBackButtonPressed()
        { return true; }

        #endregion

        #region "Events"
        #endregion

        #region "Event Handlers"
        private async void _ButtonSend_ClickedEvent(Object sender, EventArgs e)
        {
            try
            {
                if (_EntryVerificatinCode.Text.Trim() == string.Empty)
                { await DisplayAlert("ATISMobile-Error", "اطلاعات را به طور کامل وارد کنید", "تایید"); return; }

                _ButtonSend.IsEnabled = false; _ButtonSend.BackgroundColor = Color.Gray;
                string myMobileNumber = _LabelMobileNumber.Text.Trim();
                string myVerificationCode = _EntryVerificatinCode.Text.Trim();

                //HttpClient _Client = new HttpClient();
                //_Client.BaseAddress = new Uri(ATISMobileWebApiMClassManagement.GetATISMobileWebApiHostUrl());
                //_Client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "/api/SoftwareUsers/LoginSoftwareUser");
                request.Headers.Add("AuthCode", ATISMobileWebApiMClassManagement.GetAuthCode2PartHashed());
                request.Headers.Add("MobileNumber", myMobileNumber);
                request.Headers.Add("VerificationCode", myVerificationCode);
                HttpResponseMessage response = await HttpClientOnlyInstance.HttpClientInstance().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var UserId = JsonConvert.DeserializeObject<Int64>(content);
                    String TargetPath = System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                    TargetPath = Path.Combine(TargetPath, "AMUStatus.txt");
                    if (System.IO.File.Exists(TargetPath) == false)
                    { await DisplayAlert("ATISMobile-Failed", "بانک اطلاعاتی موجود نیست", "تایید"); }
                    else
                    {
                        System.IO.File.WriteAllText(TargetPath, "login;" + UserId.ToString() + ";" + _LabelMobileNumber.Text.Trim());
                        NavigationPage _MenuPage = new NavigationPage(new MenuPage(false));
                        NavigationPage.SetHasNavigationBar(_MenuPage, false);
                        _MenuPage.BarBackgroundColor = Color.Black;
                        await Navigation.PushAsync(_MenuPage);
                    }
                }
                else { await DisplayAlert("ATISMobile-Failed", JsonConvert.DeserializeObject<string>(response.Content.ReadAsStringAsync().Result), "تایید"); }
            }
            catch (System.Net.WebException ex)
            { await DisplayAlert("ATISMobile-Error", ATISMobilePredefinedMessages.ATISWebApiNotReachedMessage, "OK"); }
            catch (Exception ex)
            { await DisplayAlert("ATISMobile-Error", ex.Message, "OK"); }
            _ButtonSend.IsEnabled = true; _ButtonSend.BackgroundColor = Color.Green;
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
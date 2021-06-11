﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Http;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http.Headers;

using ATISMobile.PublicProcedures;
using ATISMobile.Models;
using ATISMobile.HttpClientInstance;
using ATISMobile.SecurityAlgorithmsManagement;
using ATISMobile.SecurityAlgorithmsManagement.HashingAlgorithms;


namespace ATISMobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : ContentPage
    {
        #region "General Properties"
        private Boolean _IsBackButtonActive = true;
        #endregion

        #region "Subroutins And Functions"
        public MenuPage(Boolean YourIsBackButtonActive)
        {
            InitializeComponent();
            _IsBackButtonActive = YourIsBackButtonActive;
            ShowProcesses();
        }

        private async void ActivateSoftwareUser()
        {
            System.IO.File.WriteAllText(ATISMobileWebApiMClassManagement.GetTargetPath(), "");
            MobileEntryPage _MobileEntryPage = new MobileEntryPage(false);
            await Navigation.PushAsync(_MobileEntryPage);
        }

        public async void ShowProcesses()
        {
            try
            {
                await Nonce.GetNonce();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/api/MobileProcesses/GetMobileProcesses");
                var Content = ATISMobileWebApiMClassManagement.GetMobileNumber() + ";" + Hashing.GetSHA256Hash(ATISMobileWebApiMClassManagement.GetApiKey() + Nonce.CurrentNonce);
                request.Content = new StringContent(JsonConvert.SerializeObject(Content), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await HttpClientOnlyInstance.HttpClientInstance().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var _Lst = JsonConvert.DeserializeObject<List<MobileProcess>>(content);
                    if (_Lst.Count == 0)
                    {; }
                    else
                    { _ListView.ItemsSource = _Lst; }
                    return;
                }
                else
                { await DisplayAlert("ATISMobile-Failed", JsonConvert.DeserializeObject<string>(response.Content.ReadAsStringAsync().Result), "تایید"); }
            }
            catch (System.Net.WebException ex)
            { await DisplayAlert("ATISMobile-Error", ATISMobilePredefinedMessages.ATISWebApiNotReachedMessage, "OK"); }
            catch (Exception ex)
            { await DisplayAlert("ATISMobile-Error", ex.Message, "OK"); }
            ActivateSoftwareUser();
        }


        #endregion

        #region "Events"
        #endregion

        #region "Event Handlers"
        async void OnTapGestureRecognizerTapped(object sender, EventArgs args)
        {
            try
            {
                await Nonce.GetNonce();
                string TargetMobileProcess = (((Label)sender).Parent.FindByName("_TargetMobileProcess") as Label).Text;
                string TargetMobileProcessId = (((Label)sender).Parent.FindByName("_TargetMobileProcessId") as Label).Text;

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, new Uri("/api/Permissions/ExistPermission"));
                var Content = ATISMobileWebApiMClassManagement.GetMobileNumber() + ";" + Hashing.GetSHA256Hash(ATISMobileWebApiMClassManagement.GetApiKey() + Nonce.CurrentNonce + TargetMobileProcessId) + ";" + TargetMobileProcessId;
                request.Content = new StringContent(JsonConvert.SerializeObject(Content), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await HttpClientOnlyInstance.HttpClientInstance().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var ExistPermission = JsonConvert.DeserializeObject<bool>(content);
                    if (ExistPermission)
                    {
                        var pageType = Type.GetType(TargetMobileProcess);
                        var page = Activator.CreateInstance(pageType) as Page;
                        await Navigation.PushAsync(page);
                    }
                    else
                    { await DisplayAlert("ATISMobile", "مجوز دسترسی به این فرآیند را ندارید", "تایید"); }
                }
                else
                { await DisplayAlert("ATISMobile-Failed", JsonConvert.DeserializeObject<string>(response.Content.ReadAsStringAsync().Result), "تایید"); }
            }
            catch (System.Net.WebException ex)
            { await DisplayAlert("ATISMobile-Error", ATISMobilePredefinedMessages.ATISWebApiNotReachedMessage, "OK"); }
            catch (Exception ex)
            { await DisplayAlert("ATISMobile-Error", ex.Message, "OK"); }
        }

        protected override bool OnBackButtonPressed()
        {
            if (_IsBackButtonActive)
            { return false; }
            else
            { return true; }
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
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Diagnostics;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Http.Headers;

using ATISMobile.Enums;
using ATISMobile.Models;
using ATISMobile.PublicProcedures;
using ATISMobile.HttpClientInstance;
using ATISMobile.SecurityAlgorithmsManagement.HashingAlgorithms;
using ATISMobile.SecurityAlgorithmsManagement;

namespace ATISMobile.ProvincesManagement
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProvinceSelectionPage : ContentPage
    {
        #region "General Properties"
        private Int64 _AHId;
        private Int64 _AHSGId;
        private LoadCapacitorLoadsListType _LoadCapacitorLoadsListType;

        private string _title;
        public new string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged(); }
        }

        #endregion

        #region "Subroutins And Functions"
        public ProvinceSelectionPage()
        {
            this.BindingContext = this;
            InitializeComponent();
        }

        public async void ViewInformation(Int64 YourAHId, Int64 YourAHSGId, LoadCapacitorLoadsListType YourLoadCapacitorLoadsListType)
        {
            _AHId = YourAHId; _AHSGId = YourAHSGId;
            _LoadCapacitorLoadsListType = YourLoadCapacitorLoadsListType;
            try
            {
                await Nonce.GetNonce();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/api/Provinces/GetProvinces");
                var Content = ATISMobileWebApiMClassManagement.GetMobileNumber() + ";" + Hashing.GetSHA256Hash(ATISMobileWebApiMClassManagement.GetApiKey() + Nonce.CurrentNonce + _AHId.ToString() + _AHSGId.ToString() + ((int)_LoadCapacitorLoadsListType).ToString()) + ";" + _AHId.ToString() + ";" + _AHSGId.ToString() + ";" + ((int)_LoadCapacitorLoadsListType).ToString();
                request.Content = new StringContent(JsonConvert.SerializeObject(Content), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await HttpClientOnlyInstance.HttpClientInstance().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    List<Province> _List = new List<Province>();
                    _List = JsonConvert.DeserializeObject<List<Province>>(content);
                    if (_List.Count == 0)
                    { _ListView.IsVisible = false; _StackLayoutEmptyProvince.IsVisible = true; }
                    else
                    { _ListView.ItemsSource = _List; }
                }
                else
                {
                    await DisplayAlert("ATISMobile-Failed", JsonConvert.DeserializeObject<string>(response.Content.ReadAsStringAsync().Result), "تایید");
                    _ListView.IsVisible = false; _StackLayoutEmptyProvince.IsVisible = true;
                }
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
        async void OnTapGestureRecognizerTapped(object sender, EventArgs args)
        {
            try
            {
                LoadCapacitorManagement.LoadsPage _LoadsPage = new LoadCapacitorManagement.LoadsPage();
                _LoadsPage.ViewLoads(_AHId, _AHSGId, Convert.ToInt64((((Label)sender).Parent.FindByName("_ProvinceId") as Label).Text.Split(':')[1]), (((Label)sender).Parent.FindByName("_ProvinceTitle") as Label).Text.Split(':')[0], _LoadCapacitorLoadsListType);
                await Navigation.PushAsync(_LoadsPage);
            }
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Diagnostics;
using System.Windows.Input;
using Microsoft.Win32;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using Xamarin.Forms.Internals;
using System.Net.Http.Headers;

using ATISMobile.Enums;
using ATISMobile.Models;
using ATISMobile.PublicProcedures;
using ATISMobile.HttpClientInstance;
using ATISMobile.SecurityAlgorithmsManagement.HashingAlgorithms;
using ATISMobile.SecurityAlgorithmsManagement;

namespace ATISMobile.LoadCapacitorManagement
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadsPage : ContentPage
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
        public LoadsPage()
        {
            this.BindingContext = this;
            InitializeComponent();
        }

        public async void ViewLoads(int YourAHId, int YourAHSGId)
        {
            try
            {
                await Nonce.GetNonce();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/api/LoadCapacitor/GetLoadCapacitorLoads");
                var Content = ATISMobileWebApiMClassManagement.GetMobileNumber() + ";" + Hashing.GetSHA256Hash(ATISMobileWebApiMClassManagement.GetApiKey() + Nonce.CurrentNonce + YourAHId.ToString() + YourAHSGId.ToString() + Int64.MinValue.ToString() + LoadCapacitorLoadsListType.NotSedimented.ToString()) + ";" + YourAHId.ToString() + ";" + YourAHSGId.ToString() + ";" + Int64.MinValue.ToString() + ";" + LoadCapacitorLoadsListType.NotSedimented.ToString();
                request.Content = new StringContent(JsonConvert.SerializeObject(Content), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await HttpClientOnlyInstance.HttpClientInstance().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    List<LoadCapacitorLoad> _List = new List<LoadCapacitorLoad>();
                    _List = JsonConvert.DeserializeObject<List<LoadCapacitorLoad>>(content);
                    if (_List.Count == 0)
                    { _ListView.IsVisible = false; _StackLayoutEmptyAnnounce.IsVisible = true; }
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

        public async void ViewLoads(Int64 YourAHId, Int64 YourAHSGId, Int64 YourProvinceId, string YourProvinceTitle, LoadCapacitorLoadsListType YourLoadCapacitorLoadsListType)
        {
            try
            {
                await Nonce.GetNonce();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/api/LoadCapacitor/GetLoadCapacitorLoads");
                var Content = ATISMobileWebApiMClassManagement.GetMobileNumber() + ";" + Hashing.GetSHA256Hash(ATISMobileWebApiMClassManagement.GetApiKey() + Nonce.CurrentNonce + YourAHId.ToString() + YourAHSGId.ToString() + YourProvinceId.ToString() + ((int)YourLoadCapacitorLoadsListType).ToString()) + ";" + YourAHId.ToString() + ";" + YourAHSGId.ToString() + ";" + YourProvinceId.ToString() + ";" + ((int)YourLoadCapacitorLoadsListType).ToString();
                request.Content = new StringContent(JsonConvert.SerializeObject(Content), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await HttpClientOnlyInstance.HttpClientInstance().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    List<LoadCapacitorLoad> _List = new List<LoadCapacitorLoad>();
                    _List = JsonConvert.DeserializeObject<List<LoadCapacitorLoad>>(content);
                    if (_List.Count == 0)
                    { _ListView.IsVisible = false; _StackLayoutEmptyAnnounce.IsVisible = true; }
                    else
                    { _LblProvinceTitle.Text = " استان " + YourProvinceTitle; _ListView.ItemsSource = _List; }
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
                var Action = await DisplayAlert("ATISMobile", "انتخاب بار را تایید می کنید؟", "بله", "خیر");
                if (Action)
                {
                    var nEstelamId = ((Label)((Button)sender).Parent.FindByName("LblnEstelamId")).Text.Split(':')[1].Trim();

                    await Nonce.GetNonce();
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/api/LoadAllocations/LoadAllocationAgent");
                    var Content = ATISMobileWebApiMClassManagement.GetMobileNumber() + ";" + Hashing.GetSHA256Hash(ATISMobileWebApiMClassManagement.GetApiKey() + Nonce.CurrentNonce + ATISMobileWebApiMClassManagement.UserLast5Digit + nEstelamId.ToString()) + ";" + nEstelamId.ToString();
                    request.Content = new StringContent(JsonConvert.SerializeObject(Content), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await HttpClientOnlyInstance.HttpClientInstance().SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                        await DisplayAlert("ATISMobile", "تخصیص بار انجام شد", "تایید");
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
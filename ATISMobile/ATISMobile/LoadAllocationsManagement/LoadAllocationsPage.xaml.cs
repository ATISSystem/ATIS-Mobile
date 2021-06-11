using System;
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

using ATISMobile.Models;
using ATISMobile.PublicProcedures;
using ATISMobile.HttpClientInstance;
using ATISMobile.SecurityAlgorithmsManagement.HashingAlgorithms;
using ATISMobile.SecurityAlgorithmsManagement;

namespace ATISMobile.LoadAllocationsManagement
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadAllocationsPage : ContentPage
    {
        #region "General Properties"
        #endregion

        #region "Subroutins And Functions"
        public LoadAllocationsPage()
        {
            InitializeComponent();
            ViewLoadAllocations();
        }

        public async void ViewLoadAllocations()
        {
            try
            {
                await Nonce.GetNonce();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/api/LoadAllocations/GetLoadAllocationsforTruckDriver");
                var Content = ATISMobileWebApiMClassManagement.GetMobileNumber() + ";" + Hashing.GetSHA256Hash(ATISMobileWebApiMClassManagement.GetApiKey() + Nonce.CurrentNonce );
                request.Content = new StringContent(JsonConvert.SerializeObject(Content), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await HttpClientOnlyInstance.HttpClientInstance().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    List<LoadAllocationsforTruckDriver> _List = new List<LoadAllocationsforTruckDriver>();
                    var content = await response.Content.ReadAsStringAsync();
                    _List = JsonConvert.DeserializeObject<List<LoadAllocationsforTruckDriver>>(content);
                    if (_List.Count == 0)
                    { _ListView.IsVisible = false; _StackLayoutEmptyAllocations.IsVisible = true; }
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

        async void OnClicked_DeleteLoadAllocation(object sender, EventArgs args)
        {
            try
            {
                var Action = await DisplayAlert("ATISMobile", "حذف تخصیص بار را تایید می کنید؟", "بله", "خیر");
                if (Action)
                {
                    var LoadAllocationId = (((StackLayout)((ImageButton)sender).Parent.Parent.FindByName("_StackLayoutInformation")).FindByName("_LabelLAId") as Label).Text.Split('-')[0].Split(':')[1].Trim();

                    await Nonce.GetNonce();
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/api/LoadAllocations/LoadAllocationCancelling");
                    var Content = ATISMobileWebApiMClassManagement.GetMobileNumber() + ";" + Hashing.GetSHA256Hash(ATISMobileWebApiMClassManagement.GetApiKey() + Nonce.CurrentNonce + ATISMobileWebApiMClassManagement.UserLast5Digit + LoadAllocationId) + ";" + LoadAllocationId;
                    request.Content = new StringContent(JsonConvert.SerializeObject(Content), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await HttpClientOnlyInstance.HttpClientInstance().SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                        ViewLoadAllocations();
                        await DisplayAlert("ATISMobile", "حذف تخصیص بار انجام شد", "تایید");
                    }
                    else
                    { await DisplayAlert("ATISMobile-Failed", JsonConvert.DeserializeObject<string>(response.Content.ReadAsStringAsync().Result), "تایید"); }
                }
            }
            catch (System.Net.WebException ex)
            { await DisplayAlert("ATISMobile-Error", ATISMobilePredefinedMessages.ATISWebApiNotReachedMessage, "OK"); }
            catch (Exception ex)
            { await DisplayAlert("ATISMobile-Error", ex.Message, "OK"); }
        }

        async void OnClicked_InreasePriority(object sender, EventArgs args)
        {
            try
            {
                var Action = await DisplayAlert("ATISMobile", "افزایش اولویت را تایید می کنید؟", "بله", "خیر");
                if (Action)
                {
                    var LoadAllocationId = (((StackLayout)((ImageButton)sender).Parent.Parent.FindByName("_StackLayoutInformation")).FindByName("_LabelLAId") as Label).Text.Split('-')[0].Split(':')[1].Trim();

                    await Nonce.GetNonce();
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/api/LoadAllocations/IncreasePriority");
                    var Content = ATISMobileWebApiMClassManagement.GetMobileNumber() + ";" + Hashing.GetSHA256Hash(ATISMobileWebApiMClassManagement.GetApiKey() + Nonce.CurrentNonce + ATISMobileWebApiMClassManagement.UserLast5Digit + LoadAllocationId) + ";" + LoadAllocationId;
                    request.Content = new StringContent(JsonConvert.SerializeObject(Content), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await HttpClientOnlyInstance.HttpClientInstance().SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    { ViewLoadAllocations(); await DisplayAlert("ATISMobile", "افزایش اولویت انجام شد", "تایید"); }
                    else
                    { await DisplayAlert("ATISMobile-Failed", JsonConvert.DeserializeObject<string>(response.Content.ReadAsStringAsync().Result), "تایید"); }
                }
            }
            catch (System.Net.WebException ex)
            { await DisplayAlert("ATISMobile-Error", ATISMobilePredefinedMessages.ATISWebApiNotReachedMessage, "OK"); }
            catch (Exception ex)
            { await DisplayAlert("ATISMobile-Error", ex.Message, "OK"); }

        }

        async void OnClicked_DecreasePriority(object sender, EventArgs args)
        {
            try
            {
                var Action = await DisplayAlert("ATISMobile", "کاهش اولویت را تایید می کنید؟", "بله", "خیر");
                if (Action)
                {
                    var LoadAllocationId = (((StackLayout)((ImageButton)sender).Parent.Parent.FindByName("_StackLayoutInformation")).FindByName("_LabelLAId") as Label).Text.Split('-')[0].Split(':')[1].Trim();

                    await Nonce.GetNonce();
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/api/LoadAllocations/DecreasePriority");
                    var Content = ATISMobileWebApiMClassManagement.GetMobileNumber() + ";" + Hashing.GetSHA256Hash(ATISMobileWebApiMClassManagement.GetApiKey() + Nonce.CurrentNonce + ATISMobileWebApiMClassManagement.UserLast5Digit + LoadAllocationId) + ";" + LoadAllocationId;
                    request.Content = new StringContent(JsonConvert.SerializeObject(Content), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await HttpClientOnlyInstance.HttpClientInstance().SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    { ViewLoadAllocations(); await DisplayAlert("ATISMobile", "کاهش اولویت انجام شد", "تایید"); }
                    else
                    { await DisplayAlert("ATISMobile-Failed", JsonConvert.DeserializeObject<string>(response.Content.ReadAsStringAsync().Result), "تایید"); ; }
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
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using ATISMobile.Models;
using ATISMobile.PublicProcedures;
using ATISMobile.HttpClientInstance;
using ATISMobile.SecurityAlgorithmsManagement.HashingAlgorithms;
using ATISMobile.SecurityAlgorithmsManagement;

namespace ATISMobile.Reports
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadPermissionsIssuedOrderByPriorityReportPage : ContentPage
    {
        #region "General Properties"
        #endregion

        #region "Subroutins And Functions"
        public LoadPermissionsIssuedOrderByPriorityReportPage()
        {
            InitializeComponent();
            _PickerAnnouncementHallSubGroups.SelectedIndexChanged += _PickerAnnouncementHallSubGroups_SelectedIndexChanged;
            Initialize();
        }

       
        public async void Initialize()
        {
            try
            {
                await Nonce.GetNonce();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/api/AnnouncementHalls/GetAnnouncementHallsAnnouncementhAllSubGroupsJOINT");
                var Content = ATISMobileWebApiMClassManagement.GetMobileNumber() + ";" + Hashing.GetSHA256Hash(ATISMobileWebApiMClassManagement.GetApiKey() + Nonce.CurrentNonce );
                request.Content = new StringContent(JsonConvert.SerializeObject(Content), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await HttpClientOnlyInstance.HttpClientInstance().SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var ListAnnouncementHalls = JsonConvert.DeserializeObject<List<KeyValuePair<string, string>>>(content);
                    if (ListAnnouncementHalls.Count == 0)
                    { }
                    else
                    {
                        List<string> Lst = new List<string>();
                        for (int Loopx = 0; Loopx <= ListAnnouncementHalls.Count - 1; Loopx++)
                        { Lst.Add(ListAnnouncementHalls[Loopx].Key + " " + ListAnnouncementHalls[Loopx].Value); }
                        _PickerAnnouncementHallSubGroups.ItemsSource = Lst;
                    }
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
        private async void _PickerAnnouncementHallSubGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                await Nonce.GetNonce();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/api/Reports/GetLoadPermissionsIssuedOrderByPriorityReport");
                var Content = ATISMobileWebApiMClassManagement.GetMobileNumber() + ";" + Hashing.GetSHA256Hash(ATISMobileWebApiMClassManagement.GetApiKey() + Nonce.CurrentNonce + _PickerAnnouncementHallSubGroups.SelectedItem.ToString().Split(' ')[0]) + ";" + _PickerAnnouncementHallSubGroups.SelectedItem.ToString().Split(' ')[0];
                request.Content = new StringContent(JsonConvert.SerializeObject(Content), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await HttpClientOnlyInstance.HttpClientInstance().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var Lst = JsonConvert.DeserializeObject<List<Models.PermissionsIssued>>(content);
                    if (Lst.Count == 0)
                    { _ListView.IsVisible = false; _StackLayoutEmptyPermissions.IsVisible = true; }
                    else
                    { _StackLayoutEmptyPermissions.IsVisible = false; _ListView.IsVisible = true; _ListView.ItemsSource = Lst; }
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



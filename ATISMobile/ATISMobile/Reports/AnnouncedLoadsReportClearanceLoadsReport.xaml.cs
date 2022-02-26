using ATISMobile.HttpClientInstance;
using ATISMobile.PublicProcedures;
using ATISMobile.SecurityAlgorithmsManagement;
using ATISMobile.SecurityAlgorithmsManagement.HashingAlgorithms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ATISMobile.Reports
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AnnouncedLoadsReportClearanceLoadsReport : TabbedPage
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
        public AnnouncedLoadsReportClearanceLoadsReport()
        {
            this.BindingContext = this;
            InitializeComponent();
            _PickerAnnouncementHallSubGroupsAnnouncedLoadsReport.SelectedIndexChanged += _PickerAnnouncementHallSubGroupsAnnouncedLoadsReport_SelectedIndexChanged;
            _PickerAnnouncementHallSubGroupsClearanceLoadsReport.SelectedIndexChanged += _PickerAnnouncementHallSubGroupsClearanceLoadsReport_SelectedIndexChanged;
            Initialize();
        }

        public async void Initialize()
        {
            try
            {
                await Nonce.GetNonce();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/api/AnnouncementHalls/GetAnnouncementHallsAnnouncementhAllSubGroupsJOINT");
                var Content = ATISMobileWebApiMClassManagement.GetMobileNumber() + ";" + Hashing.GetSHA256Hash(ATISMobileWebApiMClassManagement.GetApiKey() + Nonce.CurrentNonce);
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
                        _PickerAnnouncementHallSubGroupsAnnouncedLoadsReport.ItemsSource = Lst;
                        _PickerAnnouncementHallSubGroupsClearanceLoadsReport.ItemsSource = Lst;
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

        private async void _PickerAnnouncementHallSubGroupsAnnouncedLoadsReport_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                await Nonce.GetNonce();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/api/Reports/AnnouncedLoadsReport");
                var Content = ATISMobileWebApiMClassManagement.GetMobileNumber() + ";" + Hashing.GetSHA256Hash(ATISMobileWebApiMClassManagement.GetApiKey() + Nonce.CurrentNonce +_PickerAnnouncementHallSubGroupsAnnouncedLoadsReport .SelectedItem.ToString().Split(' ')[0]) + ";" + _PickerAnnouncementHallSubGroupsAnnouncedLoadsReport.SelectedItem.ToString().Split(' ')[0];
                request.Content = new StringContent(JsonConvert.SerializeObject(Content), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await HttpClientOnlyInstance.HttpClientInstance().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var Lst = JsonConvert.DeserializeObject<List<Models.PermissionsIssued>>(content);
                    if (Lst.Count == 0)
                    {_ListViewAnnouncedLoadsReport .IsVisible = false;_StackLayoutEmptyPermissionsAnnouncedLoadsReport .IsVisible = true; }
                    else
                    { _StackLayoutEmptyPermissionsAnnouncedLoadsReport.IsVisible = false; _ListViewAnnouncedLoadsReport.IsVisible = true; _ListViewAnnouncedLoadsReport.ItemsSource = Lst; }
                }
                else
                { await DisplayAlert("ATISMobile-Failed", JsonConvert.DeserializeObject<string>(response.Content.ReadAsStringAsync().Result), "تایید"); }
            }
            catch (System.Net.WebException ex)
            { await DisplayAlert("ATISMobile-Error", ATISMobilePredefinedMessages.ATISWebApiNotReachedMessage, "OK"); }
            catch (Exception ex)
            { await DisplayAlert("ATISMobile-Error", ex.Message, "OK"); }
        }

        private async void _PickerAnnouncementHallSubGroupsClearanceLoadsReport_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                await Nonce.GetNonce();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/api/Reports/ClearanceLoadsReport");
                var Content = ATISMobileWebApiMClassManagement.GetMobileNumber() + ";" + Hashing.GetSHA256Hash(ATISMobileWebApiMClassManagement.GetApiKey() + Nonce.CurrentNonce + _PickerAnnouncementHallSubGroupsClearanceLoadsReport.SelectedItem.ToString().Split(' ')[0]) + ";" + _PickerAnnouncementHallSubGroupsClearanceLoadsReport.SelectedItem.ToString().Split(' ')[0];
                request.Content = new StringContent(JsonConvert.SerializeObject(Content), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await HttpClientOnlyInstance.HttpClientInstance().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var Lst = JsonConvert.DeserializeObject<List<Models.PermissionsIssued>>(content);
                    if (Lst.Count == 0)
                    { _ListViewClearanceLoadsReport.IsVisible = false; _StackLayoutEmptyPermissionsClearanceLoadsReport.IsVisible = true; }
                    else
                    { _StackLayoutEmptyPermissionsClearanceLoadsReport.IsVisible = false; _ListViewClearanceLoadsReport.IsVisible = true; _ListViewClearanceLoadsReport.ItemsSource = Lst; }
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
using ATISMobile.HttpClientInstance;
using ATISMobile.Models;
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
    public partial class LoadPermissionsIssuedViaLicensePlateReport : ContentPage
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

        public LoadPermissionsIssuedViaLicensePlateReport()
        {
            this.BindingContext = this;
            InitializeComponent();
            _LPPelak.Focused += _LPPelak_Focused;
            _LPSerial.Focused += _LPSerial_Focused;
            BtnViewReport.Clicked += BtnViewReport_Clicked;
        }

        private void ClearandReady(Entry Sender)
        { Sender.Text = string.Empty; }


        #endregion

        #region "Events"
        #endregion

        #region "Event Handlers"

        private void _LPSerial_Focused(object sender, FocusEventArgs e)
        { ClearandReady((Entry)sender); }

        private void _LPPelak_Focused(object sender, FocusEventArgs e)
        { ClearandReady((Entry)sender); }

        private async void BtnViewReport_Clicked(object sender, EventArgs e)
        {
            try
            {
                ((Button)sender).IsEnabled = false;
                var LPPelak = _LPPelak.Text.Trim();
                var LPSerial = _LPSerial.Text.Trim();

                await Nonce.GetNonce();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/api/LoadAllocations/GetLoadPermissionsViaLicensePlate");
                var Content = ATISMobileWebApiMClassManagement.GetMobileNumber() + ";" + Hashing.GetSHA256Hash(ATISMobileWebApiMClassManagement.GetApiKey() + Nonce.CurrentNonce + LPPelak + LPSerial) + ";" + LPPelak + ";" + LPSerial;
                request.Content = new StringContent(JsonConvert.SerializeObject(Content), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await HttpClientOnlyInstance.HttpClientInstance().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    List<LoadAllocationsforTruckDriver> _List = new List<LoadAllocationsforTruckDriver>();
                    var content = await response.Content.ReadAsStringAsync();
                    _List = JsonConvert.DeserializeObject<List<LoadAllocationsforTruckDriver>>(content);
                    if (_List.Count == 0)
                    {
                        FrameSecond.BorderColor = Color.White;
                        _ListView.IsVisible = false;
                        await DisplayAlert("ATISMobile", "اطلاعاتی یافت نشد", "OK");
                    }
                    else
                    {
                        FrameSecond.BorderColor = Color.Green;
                        _ListView.IsVisible = true;
                        _ListView.ItemsSource = _List;
                    }
                }
                else
                { await DisplayAlert("ATISMobile-Failed", JsonConvert.DeserializeObject<string>(response.Content.ReadAsStringAsync().Result), "تایید"); }
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
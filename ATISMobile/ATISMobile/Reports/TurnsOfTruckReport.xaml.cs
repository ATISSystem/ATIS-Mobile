
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using ATISMobile.HttpClientInstance;
using ATISMobile.Models;
using ATISMobile.PublicProcedures;
using ATISMobile.SecurityAlgorithmsManagement;
using ATISMobile.SecurityAlgorithmsManagement.HashingAlgorithms;


namespace ATISMobile.Reports
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TurnsOfTruckReport : ContentPage
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

        public TurnsOfTruckReport()
        {
            this.BindingContext = this;
            InitializeComponent();
            _LPPelak.Focused += _LPPelak_Focused;
            _LPSerial.Focused += _LPSerial_Focused;
            BtnViewReport.Clicked += BtnViewReport_Clicked;
            BtnExit.Clicked += BtnExit_Clicked;
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
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/api/Turns/GetTurnsOfTruck");
                var Content = ATISMobileWebApiMClassManagement.GetMobileNumber() + ";" + Hashing.GetSHA256Hash(ATISMobileWebApiMClassManagement.GetApiKey() + Nonce.CurrentNonce + LPPelak + LPSerial) + ";" + LPPelak + ";" + LPSerial;
                request.Content = new StringContent(JsonConvert.SerializeObject(Content), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await HttpClientOnlyInstance.HttpClientInstance().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    _SLTurns.IsVisible = true; _SLTruckInf.IsVisible = false;
                    var content = await response.Content.ReadAsStringAsync();
                    var _List = JsonConvert.DeserializeObject<List<Turns>>(content);
                    if (_List.Count == 0)
                    { _ListView.IsVisible = false; }
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
            ((Button)sender).IsEnabled = true;
        }

        private void BtnExit_Clicked(object sender, EventArgs e)
        {
            try { _SLTurns.IsVisible = false; _SLTruckInf.IsVisible = true; }
            catch (Exception ex)
            { throw ex; }
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
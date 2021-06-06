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
using ATISMobile.SecurityAlgorithmsManagement.Hashing;
using ATISMobile.SecurityAlgorithmsManagement;

namespace ATISMobile.TruckDriverManagement
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TruckDriverPage : ContentPage
    {
        #region "General Properties"
        #endregion

        #region "Subroutins And Functions"

        public TruckDriverPage()
        {
            InitializeComponent();
            ViewInformation();
        }

        public async void ViewInformation()
        {
            try
            {
                var nonce = new Nonce();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post , "/api/TruckDrivers/GetTruckDriver");
                var Content = ATISMobileWebApiMClassManagement.GetMobileNumber() + ";" + Hashing.GetSHA256Hash(ATISMobileWebApiMClassManagement.GetApiKey() + nonce.CurrentNonce );
                HttpResponseMessage response = await HttpClientOnlyInstance.HttpClientInstance().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var myTruckDriver = JsonConvert.DeserializeObject<TruckDriver>(content);
                    LblNameFamily.Text = myTruckDriver.NameFamily;
                    LblFatherName.Text = myTruckDriver.FatherName;
                    LblSmartCardNo.Text = myTruckDriver.SmartCardNo;
                    LblNationalCode.Text = myTruckDriver.NationalCode;
                    LblTel.Text = myTruckDriver.Tel;
                    LblDriverLicenceNo.Text = myTruckDriver.DrivingLicenceNo;
                    LblDriverId.Text = myTruckDriver.DriverId;
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
        #endregion

        #region "Override Methods"
        #endregion

        #region "Abstract Methods"
        #endregion

        #region "Implemented Members"
        #endregion

    }
}
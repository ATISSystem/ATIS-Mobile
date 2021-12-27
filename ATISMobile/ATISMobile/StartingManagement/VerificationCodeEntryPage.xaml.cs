using System;
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
using ATISMobile.SecurityAlgorithmsManagement.HashingAlgorithms;
using ATISMobile.SecurityAlgorithmsManagement;

namespace ATISMobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VerificationCodeEntryPage : ContentPage
    {
        #region "General Properties"
        private Boolean _IsBackButtonActive = true;

        private string _title;
        public new string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged(); }
        }
        #endregion

        #region "Subroutins And Functions"
        public VerificationCodeEntryPage()
        {
            this.BindingContext = this;
            InitializeComponent();
        }

        public void SetInf(string YourVerificationCode, string YourMobileNumber)
        { _LabelMobileNumber.Text = YourMobileNumber; }


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

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/api/SoftwareUsers/LoginSoftwareUser");
                var Content = myMobileNumber + ";" + Hashing.GetSHA256Hash(myVerificationCode);
                request.Content = new StringContent(JsonConvert.SerializeObject(Content), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await HttpClientOnlyInstance.HttpClientInstance().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var AMUStatus = JsonConvert.DeserializeObject<string>(content);
                    String TargetPath = System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                    TargetPath = Path.Combine(TargetPath, "AMUStatus.txt");
                    if (System.IO.File.Exists(TargetPath) == false)
                    { await DisplayAlert("ATISMobile-Failed", "بانک اطلاعاتی موجود نیست", "تایید"); }
                    else
                    {
                        System.IO.File.WriteAllText(TargetPath, AMUStatus);
                        NavigationPage _MenuPage = new NavigationPage(new MenuPage(false));
                        NavigationPage.SetHasNavigationBar(_MenuPage, false);
                        _MenuPage.BarBackgroundColor = Color.Black;
                        await Navigation.PushAsync(_MenuPage);
                        return;
                    }
                }
                else
                { await DisplayAlert("ATISMobile-Failed", JsonConvert.DeserializeObject<string>(response.Content.ReadAsStringAsync().Result), "تایید"); }
            }
            catch (System.Net.WebException ex)
            { await DisplayAlert("ATISMobile-Error", ATISMobilePredefinedMessages.ATISWebApiNotReachedMessage, "OK"); }
            catch (Exception ex)
            { await DisplayAlert("ATISMobile-Error", ex.Message, "OK"); }
            _ButtonSend.IsEnabled = true; _ButtonSend.BackgroundColor = Color.Green;
        }

        #endregion

        #region "Override Methods"
        protected override bool OnBackButtonPressed()
        { return true; }

        #endregion

        #region "Abstract Methods"
        #endregion

        #region "Implemented Members"
        #endregion




    }
}
using System;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using ATISMobile.PublicProcedures;
using ATISMobile.Models;
using ATISMobile.HttpClientInstance;

namespace ATISMobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MobileEntryPage : ContentPage
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
        public MobileEntryPage()
        {
            this.BindingContext = this;
            InitializeComponent();
        }

        public MobileEntryPage(Boolean YourIsBackButtonActive)
        {
            InitializeComponent();
            _IsBackButtonActive = YourIsBackButtonActive;
        }

        #endregion

        #region "Events"
        #endregion

        #region "Event Handlers"
        private async void _ButtonSend_ClickedEvent(Object sender, EventArgs e)
        {
            try
            {
                if ((_EntryMobileNumber.Text.Trim() == string.Empty) || (_EntryNameFamily.Text.Trim() == string.Empty))
                { await DisplayAlert("ATISMobile-Error", "اطلاعات را به طور کامل وارد کنید", "تایید"); return; }

                _ButtonSend.IsEnabled = false; _ButtonSend.BackgroundColor = Color.Gray;
                string myMobileNumber = _EntryMobileNumber.Text.Trim();
                string myNameFamily = _EntryNameFamily.Text.Trim();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, new Uri("/api/SoftwareUsers/RegisterMobileNumber"));
                request.Content = new StringContent(JsonConvert.SerializeObject(myMobileNumber), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await HttpClientOnlyInstance.HttpClientInstance().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {

                    VerificationCodeEntryPage _VerificationCodeEntryPage = new VerificationCodeEntryPage();
                    _VerificationCodeEntryPage.SetInf(myMobileNumber, _EntryMobileNumber.Text);
                    _VerificationCodeEntryPage.Title = "ارسال کد تایید هویت";
                    await Navigation.PushAsync(_VerificationCodeEntryPage);
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
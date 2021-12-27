
using Newtonsoft.Json;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using ATISMobile.PublicProcedures;
using ATISMobile.HttpClientInstance;
using ATISMobile.SecurityAlgorithmsManagement.HashingAlgorithms;
using ATISMobile.SecurityAlgorithmsManagement;


namespace ATISMobile.SecurityManagement
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Last5DigitEntryPage : ContentPage
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
        public Last5DigitEntryPage()
        {
            this.BindingContext = this;
            InitializeComponent();
            _ButtonConfirmation.Clicked += _ButtonConfirmation_Clicked;
            _ButtonVisibleEnterInf.Clicked += _ButtonVisibleEnterInf_Clicked;
            CheckforLastEntry();
            LblMilladiDateTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            LblShamsiDateTime.Text = ATISMobileMClassPublicProcedures.GetPersianDate(DateTime.Now);
            FillCaptcha();
            _EntryLast5DigitUserShenaseh.Focus();
        }

        public class ImageRawData
        { public byte[] IRawData; }

        private async void FillCaptcha()
        {
            try
            {
                await Nonce.GetNonce();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/api/SoftwareUsers/GetCaptcha");
                var Content = ATISMobileWebApiMClassManagement.GetMobileNumber() + ";" + Hashing.GetSHA256Hash(ATISMobileWebApiMClassManagement.GetApiKey() + Nonce.CurrentNonce);
                request.Content = new StringContent(JsonConvert.SerializeObject(Content), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await HttpClientOnlyInstance.HttpClientInstance().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var IRawData = JsonConvert.DeserializeObject<ImageRawData>(content);
                    ImgCaptcha.Source = ImageSource.FromStream(() => new MemoryStream(IRawData.IRawData));
                }
                else
                { await DisplayAlert("ATISMobile-Failed", JsonConvert.DeserializeObject<string>(response.Content.ReadAsStringAsync().Result), "تایید"); }
            }
            catch (System.Net.WebException ex)
            { await DisplayAlert("ATISMobile-Error", ATISMobilePredefinedMessages.ATISWebApiNotReachedMessage, "OK"); }
            catch (Exception ex)
            { await DisplayAlert("ATISMobile-Error", ex.Message, "OK"); }
        }

        public void CheckforLastEntry()
        {
            if (ATISMobileWebApiMClassManagement.UserLast5Digit == string.Empty)
            { _ButtonConfirmation.BackgroundColor = Color.Red; }
            else
            { _ButtonConfirmation.BackgroundColor = Color.Green; }
        }

        #endregion

        #region "Events"
        #endregion

        #region "Event Handlers"
        private async void _ButtonConfirmation_Clicked(object sender, EventArgs e)
        {
            try
            {
                _ButtonConfirmation.IsEnabled = false;
                   await Nonce.GetNonce();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/api/SoftwareUsers/GetPersonalNonce");
                var Content = ATISMobileWebApiMClassManagement.GetMobileNumber() + ";" + Hashing.GetSHA256Hash(ATISMobileWebApiMClassManagement.GetApiKey() + Nonce.CurrentNonce + _EntryLast5DigitUserShenaseh.Text + _EntryLast5DigitUserPassword.Text + _EntryCaptcha.Text);
                request.Content = new StringContent(JsonConvert.SerializeObject(Content), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await HttpClientOnlyInstance.HttpClientInstance().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var PersonalNonce = JsonConvert.DeserializeObject<string>(content);
                    ATISMobileWebApiMClassManagement.UserLast5Digit = PersonalNonce;
                    _DocHolder.IsVisible = true;
                    _InfHolder.IsVisible = false;
                    _ButtonVisibleEnterInf.IsVisible = false;
                    await DisplayAlert("ATISMobile", "رمز شخصی با موفقیت ثبت گردید", "تایید");
                    return;
                }
                else
                { await DisplayAlert("ATISMobile-Failed", JsonConvert.DeserializeObject<string>(response.Content.ReadAsStringAsync().Result), "تایید"); }
            }
            catch (System.Net.WebException ex)
            { await DisplayAlert("ATISMobile-Error", ATISMobilePredefinedMessages.ATISWebApiNotReachedMessage, "OK"); }
            catch (Exception ex)
            { await DisplayAlert("ATISMobile-Error", ex.Message, "OK"); }
            ATISMobileWebApiMClassManagement.UserLast5Digit = string.Empty;
            FillCaptcha();
            _ButtonConfirmation.IsEnabled = true;
        }

        private void _ButtonVisibleEnterInf_Clicked(object sender, EventArgs e)
        {
            _InfHolder.IsVisible = true;
            _DocHolder.IsVisible = false;
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
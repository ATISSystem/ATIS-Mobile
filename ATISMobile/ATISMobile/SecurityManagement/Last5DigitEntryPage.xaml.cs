
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
using ATISMobile.SecurityAlgorithmsManagement.Hashing;
using ATISMobile.SecurityAlgorithmsManagement;


namespace ATISMobile.SecurityManagement
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Last5DigitEntryPage : ContentPage
    {
        #region "General Properties"
        #endregion

        #region "Subroutins And Functions"
        public Last5DigitEntryPage()
        {
            InitializeComponent();
            _ButtonConfirmation.Clicked += _ButtonConfirmation_Clicked;
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
                var nonce = new Nonce();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/api/SoftwareUsers/GetCaptcha");
                var Content = ATISMobileWebApiMClassManagement.GetMobileNumber() + ";" + Hashing.GetSHA256Hash(ATISMobileWebApiMClassManagement.GetApiKey() + nonce.CurrentNonce );
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
                var nonce = new Nonce();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/api/SoftwareUsers/GetPersonalNonce");
                var Content = ATISMobileWebApiMClassManagement.GetMobileNumber() + ";" + Hashing.GetSHA256Hash(ATISMobileWebApiMClassManagement.GetApiKey() + nonce.CurrentNonce  + _EntryLast5DigitUserShenaseh.Text  + _EntryLast5DigitUserPassword.Text + _EntryCaptcha.Text) +";"+ _EntryLast5DigitUserShenaseh.Text + ";"+ _EntryLast5DigitUserPassword.Text + ";"+ _EntryCaptcha.Text;
                HttpResponseMessage response = await HttpClientOnlyInstance.HttpClientInstance().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var PersonalNonce = JsonConvert.DeserializeObject<string>(content);
                    ATISMobileWebApiMClassManagement.UserLast5Digit = PersonalNonce;
                    _LblLast5DigitUserShenaseh.IsVisible = false;
                    _EntryLast5DigitUserShenaseh.IsVisible = false;
                    _LblLast5DigitUserPassword.IsVisible = false;
                    _EntryLast5DigitUserPassword.IsVisible = false;
                    _ButtonConfirmation.IsVisible = false;
                    _LblCaptcha.IsVisible = false;
                    _EntryCaptcha.IsVisible = false;
                    await DisplayAlert("ATISMobile", "رمز شخصی با موفقیت ثبت گردید", "تایید");
                }
                else
                {
                    ATISMobileWebApiMClassManagement.UserLast5Digit = string.Empty;
                    FillCaptcha();
                    await DisplayAlert("ATISMobile-Failed", JsonConvert.DeserializeObject<string>(response.Content.ReadAsStringAsync().Result), "تایید");
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
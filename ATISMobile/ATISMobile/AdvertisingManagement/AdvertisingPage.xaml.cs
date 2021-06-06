
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using ATISMobile.PublicProcedures;

namespace ATISMobile.AdvertisingManagement
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdvertisingPage : ContentPage
    {

        #region "General Properties"
        #endregion

        #region "Subroutins And Functions"

        public AdvertisingPage()
        {
            InitializeComponent();
            BtnDownloadATISMobileLastVersion.Clicked += BtnDownloadATISMobileLastVersion_Clicked;
            ImgWhatsAppGroup1.Clicked += ImgWhatsAppGroup1_Clicked;
            ImgWhatsAppGroup2.Clicked += ImgWhatsAppGroup2_Clicked;
            ImgWhatsAppGroup3.Clicked += ImgWhatsAppGroup3_Clicked;
            ImgWhatsAppGroup4.Clicked += ImgWhatsAppGroup4_Clicked;
            ImgWhatsAppGroup5.Clicked += ImgWhatsAppGroup5_Clicked;
            ImgWhatsAppGroup6.Clicked += ImgWhatsAppGroup6_Clicked;
            DownloadAndViewAdvertisingImg();
        }

        private void OnBtnWhastAppLink(string YourLink)
        { try { Device.OpenUri(new Uri(YourLink)); } catch (Exception ex) { throw ex; } }

        private async void DownloadAndViewAdvertisingImg()
        {
            try
            {
                HttpClient client = new HttpClient();
                client.MaxResponseContentBufferSize = 256000;
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer");
                Stream stream = await client.GetStreamAsync(Properties.Resources.RestfulWebServiceProtocol + "://" + Properties.Resources.RestfulWebServiceURLFirst + "/Advertising.jpg");
                Image img = new Image();
                img.Source = ImageSource.FromStream(() => stream);
                ImgAdvertising.Source = img.Source;
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
        private async void BtnDownloadATISMobileLastVersion_Clicked(object sender, EventArgs e)
        {
            try
            { await Browser.OpenAsync(Properties.Resources.DownloadLinkURL, BrowserLaunchMode.SystemPreferred); }
            catch (Exception ex)
            { await DisplayAlert("ATISMobile-Error", ex.Message, "OK"); }
        }

        private void ImgWhatsAppGroup6_Clicked(object sender, EventArgs e)
        {
            try { OnBtnWhastAppLink("https://chat.whatsapp.com/ES7zELExevM8O2nuwfYsgK"); }
            catch (Exception ex)
            { DisplayAlert("ATISMobile-Error", ex.Message, "OK"); }
        }
        private void ImgWhatsAppGroup5_Clicked(object sender, EventArgs e)
        {
            try { OnBtnWhastAppLink("https://chat.whatsapp.com/C01QNQhTUysKB405GYFKXx"); }
            catch (Exception ex)
            { DisplayAlert("ATISMobile-Error", ex.Message, "OK"); }
        }
        private void ImgWhatsAppGroup4_Clicked(object sender, EventArgs e)
        {
            try { OnBtnWhastAppLink("https://chat.whatsapp.com/Ef0iSG5KedAAHlgwRSZ6tQ"); }
            catch (Exception ex)
            { DisplayAlert("ATISMobile-Error", ex.Message, "OK"); }
        }
        private void ImgWhatsAppGroup3_Clicked(object sender, EventArgs e)
        {
            try { OnBtnWhastAppLink("https://chat.whatsapp.com/HO16u7VXTga6iGPR8hgOao"); }
            catch (Exception ex)
            { DisplayAlert("ATISMobile-Error", ex.Message, "OK"); }
        }
        private void ImgWhatsAppGroup2_Clicked(object sender, EventArgs e)
        {
            try { OnBtnWhastAppLink("https://chat.whatsapp.com/ENt7KpOmYyL8BjJBHfUeru"); }
            catch (Exception ex)
            { DisplayAlert("ATISMobile-Error", ex.Message, "OK"); }
        }
        private void ImgWhatsAppGroup1_Clicked(object sender, EventArgs e)
        {
            try { OnBtnWhastAppLink("https://chat.whatsapp.com/IY1f7COQ9YV62Mkynf4AdJ"); }
            catch (Exception ex)
            { DisplayAlert("ATISMobile-Error", ex.Message, "OK"); }
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
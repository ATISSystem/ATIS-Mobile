
//extern alias  MonoAndroid;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using Newtonsoft.Json;


using ATISMobile.PublicProcedures;
using ATISMobile.SecurityAlgorithmsManagement;
using ATISMobile.SecurityAlgorithmsManagement.HashingAlgorithms;
using ATISMobile.HttpClientInstance;


namespace ATISMobile.DriverSelfDeclaration
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DriverSelfDeclarationPage : ContentPage
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
        public DriverSelfDeclarationPage()
        {
            this.BindingContext = this;
            InitializeComponent();
            try
            { ViewDSDs(); }
            catch (Exception ex)
            { throw ex; }
        }

        public async void ViewDSDs()
        {
            try
            {
                await Nonce.GetNonce();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/api/DriverSelfDeclaration/GetDeclarationInf");
                var Content = ATISMobileWebApiMClassManagement.GetMobileNumber() + ";" + Hashing.GetSHA256Hash(ATISMobileWebApiMClassManagement.GetApiKey() + Nonce.CurrentNonce);
                request.Content = new StringContent(JsonConvert.SerializeObject(Content), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await HttpClientOnlyInstance.HttpClientInstance().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    List<Models.DriverSelfDeclaration> _List = new List<Models.DriverSelfDeclaration>();
                    var content = await response.Content.ReadAsStringAsync();
                    _List = JsonConvert.DeserializeObject<List<Models.DriverSelfDeclaration>>(content);
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
        }

        //async Task TakePhotoAsync(Object sender)
        //{
        //    try
        //    {
        //        var photo = await Xamarin.Essentials.MediaPicker.CapturePhotoAsync();
        //        await LoadPhotoAsync(photo, sender);
        //    }
        //    catch (FeatureNotSupportedException ex)
        //    { throw ex; }
        //    catch (PermissionException ex)
        //    { throw ex; }
        //    catch (Exception ex)
        //    { throw ex; }
        //}

        //async Task LoadPhotoAsync(FileResult Photo, Object sender)
        //{
        //    try
        //    {
        //        if (Photo == null) { throw new Exception("خطا در بارگذاری تصویر"); }
        //        var Stream = await Photo.OpenReadAsync();
        //        MemoryStream MS = new MemoryStream();
        //        Stream.CopyTo(MS);
        //        byte[] ImageDataTemp = MS.ToArray();
        //        MonoAndroid.Android.Graphics.Bitmap originalImage = MonoAndroid.Android.Graphics.BitmapFactory.DecodeByteArray(ImageDataTemp, 0, ImageDataTemp.Length);
        //        MonoAndroid.Android.Graphics.Bitmap resizedImage = MonoAndroid.Android.Graphics.Bitmap.CreateScaledBitmap(originalImage, (int)1000, (int)1000, true);
        //        MemoryStream MSTemp = new MemoryStream();
        //        resizedImage.Compress(MonoAndroid.Android.Graphics.Bitmap.CompressFormat.Jpeg, 50, MSTemp);

        //        var DSDImage = Convert.ToBase64String(MSTemp.ToArray());
        //        var DSDId = ((Label)((Button)sender).Parent.FindByName("_LblDSDId")).Text;

        //        await Nonce.GetNonce();
        //        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/api/DriverSelfDeclaration/SaveDSDImage");
        //        var Content = ATISMobileWebApiMClassManagement.GetMobileNumber() + ";" + Hashing.GetSHA256Hash(ATISMobileWebApiMClassManagement.GetApiKey() + Nonce.CurrentNonce + ATISMobileWebApiMClassManagement.UserLast5Digit) + ";" + DSDId + ";" + DSDImage;
        //        request.Content = new StringContent(JsonConvert.SerializeObject(Content), Encoding.UTF8, "application/json");
        //        HttpResponseMessage response = await HttpClientOnlyInstance.HttpClientInstance().SendAsync(request);
        //        if (response.IsSuccessStatusCode)
        //        { await DisplayAlert("ATISMobile", "بارگذاری تصویر با موفقیت انجام شد", "تایید"); }
        //        else
        //        { await DisplayAlert("ATISMobile-Failed", JsonConvert.DeserializeObject<string>(response.Content.ReadAsStringAsync().Result), "تایید"); }
        //    }
        //    catch (Exception ex)
        //    { throw ex; }
        //}

        #endregion

        #region "Events"
        #endregion

        #region "Event Handlers"


        private async void BtnSubmit_ClickedEvent(Object sender, EventArgs e)
        {
            try
            {
                ((Button)sender).IsEnabled = false;
                var Action = await DisplayAlert("ATISMobile", "مشخصات را تایید می کنید؟", "بله", "خیر");
                if (Action)
                {
                    IEnumerable<PropertyInfo> pInfos = (_ListView as ItemsView<Cell>).GetType().GetRuntimeProperties();
                    var templatedItems = pInfos.FirstOrDefault(info => info.Name == "TemplatedItems");
                    string DSDCompose = string.Empty;
                    if (templatedItems != null)
                    {
                        var cells = templatedItems.GetValue(_ListView);
                        //Xamarin.Forms.ITemplatedItemsList<Xamarin.Forms.Cell> listCell = cells as Xamarin.Forms.ITemplatedItemsList<Xamarin.Forms.Cell>;
                        foreach (ViewCell cell in cells as Xamarin.Forms.ITemplatedItemsList<Xamarin.Forms.Cell>)
                        {
                            string DSDValue = ((Entry)cell.FindByName("_EntryDSDValue")).Text;
                            Int64 DSDId = Convert.ToInt64(((Label)cell.FindByName("_LblDSDId")).Text);
                            DSDCompose += DSDId.ToString() + ":" + DSDValue + "|";
                        }
                        DSDCompose = DSDCompose.ToString().Substring(0, DSDCompose.Length - 1);
                    }
                    await Nonce.GetNonce();
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/api/DriverSelfDeclaration/SetDeclarationInf");
                    var Content = ATISMobileWebApiMClassManagement.GetMobileNumber() + ";" + Hashing.GetSHA256Hash(ATISMobileWebApiMClassManagement.GetApiKey() + Nonce.CurrentNonce + ATISMobileWebApiMClassManagement.UserLast5Digit + DSDCompose) + ";" + DSDCompose;
                    request.Content = new StringContent(JsonConvert.SerializeObject(Content), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await HttpClientOnlyInstance.HttpClientInstance().SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    { await DisplayAlert("ATISMobile", "اطلاعات با موفقیت ارسال شد", "تایید"); }
                    else
                    { await DisplayAlert("ATISMobile-Failed", JsonConvert.DeserializeObject<string>(response.Content.ReadAsStringAsync().Result), "تایید"); }
                }
            }
            catch (System.Net.WebException ex)
            { await DisplayAlert("ATISMobile-Error", ATISMobilePredefinedMessages.ATISWebApiNotReachedMessage, "OK"); }
            catch (Exception ex)
            { await DisplayAlert("ATISMobile-Error", ex.Message, "OK"); }
        }

        private async void BtnGetAllowedLoadingCapacity_ClickedEvent(Object sender, EventArgs e)
        {
            try
            {
                await Nonce.GetNonce();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/api/DriverSelfDeclaration/GetAllowedLoadingCapacity");
                var Content = ATISMobileWebApiMClassManagement.GetMobileNumber() + ";" + Hashing.GetSHA256Hash(ATISMobileWebApiMClassManagement.GetApiKey() + Nonce.CurrentNonce);
                request.Content = new StringContent(JsonConvert.SerializeObject(Content), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await HttpClientOnlyInstance.HttpClientInstance().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Int64 Allowed = JsonConvert.DeserializeObject<Int64>(content);
                    await DisplayAlert("ATISMobile", "ظرفیت مجاز بارگیری: " + Allowed.ToString() + " تن", "تایید");
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

        private async void BtnAttachement_ClickedEvent(Object sender, EventArgs e)
        {
            //((Button)sender).IsEnabled = false;
            //try
            //{ await TakePhotoAsync(sender); }
            //catch (System.Net.WebException ex)
            //{ await DisplayAlert("ATISMobile-Error", ATISMobilePredefinedMessages.ATISWebApiNotReachedMessage, "OK"); }
            //catch (Exception ex)
            //{ await DisplayAlert("ATISMobile-Error", ex.Message, "OK"); }
            //((Button)sender).IsEnabled = true;
        }

        private async void _EntryDSDValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var Entry = (Entry)sender;
                var PersianKeyboard = ((Label)((Entry)sender).Parent.FindByName("_LblPersianKeyboard")).Text == "True" ? true : false;
                var IsNumeric = ((Label)((Entry)sender).Parent.FindByName("_LblIsNumeric")).Text == "True" ? true : false;
                var DecimalPoint = ((Label)((Entry)sender).Parent.FindByName("_LblDecimalPoint")).Text == "True" ? true : false;
                var DSDId = ((Label)((Entry)sender).Parent.FindByName("_LblDSDId")).Text;
                if (DSDId is null) { return; }
                if (System.Text.Encoding.Default.GetBytes(Entry.Text).Count() < 1) { return; }
                //کنترل تایپ فارسی
                if (!PersianKeyboard)
                {
                    if (System.Text.Encoding.Default.GetBytes(Entry.Text)[0] > 128)
                    { ((Entry)sender).Text = string.Empty; throw new Exception("از صفحه کلید لاتین استفاده کنید" ); }
                }
                //تایپ اعداد
                if (IsNumeric)
                {
                    if (DecimalPoint) //نقطه اعشار
                    {
                        if (e.NewTextValue.ToCharArray().Where(x => (x != '.') && (x != '0') && (x != '1') && (x != '2') && (x != '3') && (x != '4') && (x != '5') && (x != '6') && (x != '7') && (x != '8') && (x != '9')).Count() > 0)
                        { ((Entry)sender).Text = string.Empty; throw new Exception("در این فیلد فقط از ارقام و نقطه اعشار استفاده نمایید" ); }
                    }
                    else
                    {
                        if (((Entry)sender).Text != string.Empty)
                        {
                            if (!(e.NewTextValue.ToCharArray().All(x => char.IsDigit(x))))
                            { ((Entry)sender).Text = string.Empty; throw new Exception("در این فیلد فقط از ارقام استفاده نمایید" ); }
                        }
                    }
                }
            }
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using ATISMobile.PublicProcedures;
using ATISMobile.SecurityAlgorithmsManagement;
using ATISMobile.SecurityAlgorithmsManagement.HashingAlgorithms;
using Newtonsoft.Json;
using ATISMobile.HttpClientInstance;
using System.Reflection;

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
            ViewDSDs();
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
                        Xamarin.Forms.ITemplatedItemsList<Xamarin.Forms.Cell> listCell = cells as Xamarin.Forms.ITemplatedItemsList<Xamarin.Forms.Cell>;
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
                    { await DisplayAlert("ATISMobile", "اطلاعات با موفقیت ارسال شد", "تایید");}
                    else
                    { await DisplayAlert("ATISMobile-Failed", JsonConvert.DeserializeObject<string>(response.Content.ReadAsStringAsync().Result), "تایید"); }
                }
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
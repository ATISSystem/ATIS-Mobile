using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using ATISMobile.Enums;

namespace ATISMobile.AnnouncementHallsManagement
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AnnouncementHallsSelectionPage : TabbedPage
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
        public AnnouncementHallsSelectionPage()
        {
            this.BindingContext = this;
            InitializeComponent();
        }

        private async void ViewProvinces(int YourAHId, int YourAHSGId)
        {
            try
            {
                LoadCapacitorManagement.LoadCapacitorLoadsListTypeSelectionPage _LoadCapacitorLoadsListTypeSelectionPage = new LoadCapacitorManagement.LoadCapacitorLoadsListTypeSelectionPage();
                _LoadCapacitorLoadsListTypeSelectionPage.ViewInformation(YourAHId, YourAHSGId);
                _LoadCapacitorLoadsListTypeSelectionPage.Title = string.Empty;
                await Navigation.PushAsync(_LoadCapacitorLoadsListTypeSelectionPage);
            }
            catch (Exception ex)
            { await DisplayAlert("ATISMobile-Error", ex.Message, "OK"); }
        }


        #endregion

        #region "Events"
        #endregion

        #region "Event Handlers"

        private void RoadIron_ClickedEvent(Object sender, EventArgs e)
        { try { ViewProvinces(2, 7); } catch (Exception ex) { DisplayAlert("ATISMobile-Error", ex.Message, "OK"); } }
        private void RoadIngots_ClickedEvent(Object sender, EventArgs e)
        { try { ViewProvinces(2, 8); } catch (Exception ex) { DisplayAlert("ATISMobile-Error", ex.Message, "OK"); } }
        private void RoadExport_ClickedEvent(Object sender, EventArgs e)
        { try { ViewProvinces(2, 9); } catch (Exception ex) { DisplayAlert("ATISMobile-Error", ex.Message, "OK"); } }
        private void RoadCoil_ClickedEvent(Object sender, EventArgs e)
        { try { ViewProvinces(2, 11); } catch (Exception ex) { DisplayAlert("ATISMobile-Error", ex.Message, "OK"); } }
        private void RoadWareHouse_ClickedEvent(Object sender, EventArgs e)
        { try { ViewProvinces(3, 14); } catch (Exception ex) { DisplayAlert("ATISMobile-Error", ex.Message, "OK"); } }
        private void UrbanIron_ClickedEvent(Object sender, EventArgs e)
        { try { ViewProvinces(5, 12); } catch (Exception ex) { DisplayAlert("ATISMobile-Error", ex.Message, "OK"); } }
        private void UrbanCoil_ClickedEvent(Object sender, EventArgs e)
        { try { ViewProvinces(5, 13); } catch (Exception ex) { DisplayAlert("ATISMobile-Error", ex.Message, "OK"); } }
        private void UrbanWareHouse_ClickedEvent(Object sender, EventArgs e)
        { try { ViewProvinces(3, 15); } catch (Exception ex) { DisplayAlert("ATISMobile-Error", ex.Message, "OK"); } }
        private void _RoomyKhavar_ClickedEvent(Object sender, EventArgs e)
        { try { ViewProvinces(4, 1); } catch (Exception ex) { DisplayAlert("ATISMobile-Error", ex.Message, "OK"); } }
        private void _Roomy6Wheel_ClickedEvent(Object sender, EventArgs e)
        { { try { ViewProvinces(4, 2); } catch (Exception ex) { DisplayAlert("ATISMobile-Error", ex.Message, "OK"); } } }
        private void _Roomy10Wheel_ClickedEvent(Object sender, EventArgs e)
        { { { try { ViewProvinces(4, 3); } catch (Exception ex) { DisplayAlert("ATISMobile-Error", ex.Message, "OK"); } } } }

        #endregion

        #region "Override Methods"
        #endregion

        #region "Abstract Methods"
        #endregion

        #region "Implemented Members"
        #endregion

    }
}
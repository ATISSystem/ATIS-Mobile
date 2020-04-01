﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Diagnostics;
using Newtonsoft.Json;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using ATISMobile.Models;

namespace ATISMobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadAllocationsPage : ContentPage
    {
        private Int64 _CurrentMUId; 

        public LoadAllocationsPage()
        {
            InitializeComponent();
        }

        public async void ViewLoadAllocations(Int64  YourMUId)
        {
            try
            {
                _CurrentMUId = YourMUId; 
                List<LoadAllocationsforTruckDriver> _List = new List<LoadAllocationsforTruckDriver>();
                HttpClient _Client = new HttpClient();
                var response = await _Client.GetAsync(Properties.Resources.RestfulWebServiceURL + "/api/LoadAllocations/GetLoadAllocationsforTruckDriver/?YourMUId=" + YourMUId + "");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    _List = JsonConvert.DeserializeObject<List<LoadAllocationsforTruckDriver>>(content);
                    if (_List.Count == 0)
                    {
                        _ListView.IsVisible = false;
                        _StackLayoutEmptyAllocations.IsVisible = true;
                    }
                    else
                    {
                        _ListView.ItemsSource = _List;
                    }
                }

            }
            catch (Exception ex)
            { Debug.WriteLine("\t\tERROR {0}", ex.Message); }
        }

        async void OnClicked_DeleteLoadAllocation(object sender, EventArgs args)
        {
            try
            {
                var LoadAllocationId = (((StackLayout)((ImageButton)sender).Parent.Parent.FindByName("_StackLayoutInformation")).FindByName("_LabelLAId") as Label).Text.Split('-')[0].Split(':')[1].Trim();
                HttpClient _Client = new HttpClient();
                var response = await _Client.GetAsync(Properties.Resources.RestfulWebServiceURL + "/api/LoadAllocations/LoadAllocationCancelling/?YourLoadAllocationId=" + LoadAllocationId + "");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var myMS = JsonConvert.DeserializeObject<MessageStruct>(content);
                    ViewLoadAllocations(_CurrentMUId);
                    await DisplayAlert("ATISMobile", myMS.Message1, "OK");

                }
            }
            catch (Exception ex)
            { Debug.WriteLine("\t\tERROR {0}", ex.Message); }
        }

        async void OnClicked_InreasePriority(object sender, EventArgs args)
        {
            try
            {
                var LoadAllocationId = (((StackLayout)((ImageButton)sender).Parent.Parent.FindByName("_StackLayoutInformation")).FindByName("_LabelLAId") as Label).Text.Split('-')[0].Split(':')[1].Trim();
                HttpClient _Client = new HttpClient();
                var response = await _Client.GetAsync(Properties.Resources.RestfulWebServiceURL + "/api/LoadAllocations/IncreasePriority/?YourLoadAllocationId=" + LoadAllocationId + "");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var myMS = JsonConvert.DeserializeObject<MessageStruct>(content);
                    ViewLoadAllocations(_CurrentMUId);
                    await DisplayAlert("ATISMobile", myMS.Message1, "OK");
                }
            }
            catch (Exception ex)
            { Debug.WriteLine("\t\tERROR {0}", ex.Message); }

        }

        async void OnClicked_DecreasePriority(object sender, EventArgs args)
        {
            try
            {
                var LoadAllocationId = (((StackLayout)((ImageButton)sender).Parent.Parent.FindByName("_StackLayoutInformation")).FindByName("_LabelLAId") as Label).Text.Split('-')[0].Split(':')[1].Trim();
                HttpClient _Client = new HttpClient();
                var response = await _Client.GetAsync(Properties.Resources.RestfulWebServiceURL + "/api/LoadAllocations/DecreasePriority/?YourLoadAllocationId=" + LoadAllocationId + "");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var myMS = JsonConvert.DeserializeObject<MessageStruct>(content);
                    ViewLoadAllocations(_CurrentMUId);
                    await DisplayAlert("ATISMobile", myMS.Message1, "OK");
                }
            }
            catch (Exception ex)
            { Debug.WriteLine("\t\tERROR {0}", ex.Message); }
        }



        


    }
}
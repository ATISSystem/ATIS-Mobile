﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ATISMobile.Models;
using ATISMobile.PublicProcedures;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ATISMobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MoneyWalletTransactionsPage : ContentPage
    {
        public MoneyWalletTransactionsPage()
        {
            InitializeComponent();
        }

        public async void ViewInformation(Int64 YourMUId)
        {
            try
            {
                List<MoneyWalletAccounting> _List = new List<MoneyWalletAccounting>();
                HttpClient _Client = new HttpClient();
                var response = await _Client.GetAsync(Properties.Resources.RestfulWebServiceURL + "/api/MoneyWalletAccounting/GetMoneyWalletAccounting/?YourMUId=" + YourMUId + "");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    _List = JsonConvert.DeserializeObject<List<MoneyWalletAccounting>>(content);
                    if (_List.Count == 0)
                    { _ListView.IsVisible = false; }
                    else
                    { _ListView.ItemsSource = _List; }
                }
                else { Debug.WriteLine(response.RequestMessage); }
            }
            catch (Exception ex)
            { Debug.WriteLine("\t\tERROR {0}", ex.Message); }
        }


        private void _BtnRefreshTransactions_ClickedEvent(object sender, EventArgs e)
        {
            try
            { ViewInformation(ATISMobileMClassPublicProcedures.GetCurrentMobileUserId()); }
            catch (Exception ex)
            { Debug.WriteLine("\t\tERROR {0}", ex.Message); }
        }

    }
}
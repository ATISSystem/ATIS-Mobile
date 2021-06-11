using System;
using System.Text;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.IO;
using System.Security.Cryptography;
using System.Globalization;
using System.Drawing;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


using ATISMobile.Exceptions;
using System.Net;
using System.Net.Http.Headers;
using ATISMobile.PublicProcedures;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ATISMobile.HttpClientInstance;
using ATISMobile.SecurityAlgorithmsManagement.HashingAlgorithms;


namespace ATISMobile
{
    namespace Enums
    {
        public enum LoadCapacitorLoadsListType
        {
            None = 0,
            NotSedimented = 1,
            Sedimented = 2,
            TommorowLoad = 3
        }

    }

    namespace PublicProcedures
    {
        public class ATISMobilePredefinedMessages
        {
            public static readonly string ATISWebApiNotReachedMessage = "ارتباط با سرور آتیس برقرار نیست";
        }

        public class ATISMobileMClassPublicProcedures
        {
            public static bool IsThisIPAvailable(String YourIP)
            {
                try
                {
                    Ping p = new Ping();
                    if (p.Send(YourIP).Status == IPStatus.Success)
                    { return true; }
                    else
                    { return false; }
                }
                catch (Exception ex)
                { return false; }
            }

            public static bool IsInternetAvailable()
            {
                try
                { return IsThisIPAvailable("www.google.com"); }
                catch (Exception ex)
                { return false; }
            }

            public static string GetPersianDate(DateTime YourDateTime)
            {
                try
                {
                    PersianCalendar PersianCalendar1 = new PersianCalendar();
                    return string.Format(@"{0}/{1}/{2}", PersianCalendar1.GetYear(YourDateTime), PersianCalendar1.GetMonth(YourDateTime), PersianCalendar1.GetDayOfMonth(YourDateTime));
                }
                catch (Exception ex)
                { throw ex; }
            }


        }

        public class ATISMobileWebApiMClassManagement
        {
            private static string _Last5Digit = String.Empty;
            public static string UserLast5Digit
            { get { return _Last5Digit; } set { _Last5Digit = value; } }

            public static string GetATISMobileWebApiHostUrlFirstWithoutPortNumber()
            { return Properties.Resources.RestfulWebServiceURLFirst; }

            public static string GetATISMobileWebApiHostUrlSecondWithoutPortNumber()
            { return Properties.Resources.RestfulWebServiceURLSecond; }

            private static string GetATISMobileWebApiHostUrlFirst()
            { return Properties.Resources.RestfulWebServiceProtocol + "://" + Properties.Resources.RestfulWebServiceURLFirst + ":" + Properties.Resources.RestfulWebServicePortNumber; }
            private static string GetATISMobileWebApiHostUrlSecond()
            { return Properties.Resources.RestfulWebServiceProtocol + "://" + Properties.Resources.RestfulWebServiceURLSecond + ":" + Properties.Resources.RestfulWebServicePortNumber; }

            public static string ATISMobileWebApiHostUrlHolder = string.Empty;
            public static async Task<HttpResponseMessage> SetATISMobileWebApiHostUrl()
            {
                try
                {
                    HttpClient _HttpClient = new HttpClient();
                    HttpResponseMessage response = await _HttpClient.GetAsync(GetATISMobileWebApiHostUrlFirst() + "/api/ATISMobileWebApi/ISWebApiLive");
                    if (response.IsSuccessStatusCode)
                    {
                        ATISMobileWebApiHostUrlHolder = GetATISMobileWebApiHostUrlFirst();
                        return response;
                    }
                    else
                    { throw new Exception(); }
                }
                catch (Exception ex)
                {
                    try
                    {
                        HttpClient _HttpClient = new HttpClient();
                        HttpResponseMessage response = await _HttpClient.GetAsync(GetATISMobileWebApiHostUrlSecond() + "/api/ATISMobileWebApi/ISWebApiLive");
                        if (response.IsSuccessStatusCode)
                        {
                            ATISMobileWebApiHostUrlHolder = GetATISMobileWebApiHostUrlSecond();
                            return response;
                        }
                        else
                        { throw new Exception(); }
                    }
                    catch (Exception exx)
                    { throw new Exception(ATISMobilePredefinedMessages.ATISWebApiNotReachedMessage); }
                }
            }

            public static string GetApiKey()
            {
                try
                {
                    String TargetPath = System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                    TargetPath = Path.Combine(TargetPath, "AMUStatus.txt");
                    if (System.IO.File.Exists(TargetPath) == false)
                    { throw new AMUStatusFileNotFoundException(null); }
                    else
                    { return System.IO.File.ReadAllText(TargetPath).Split(';')[1]; }
                }
                catch (AMUStatusFileNotFoundException ex)
                { throw ex; }
                catch (Exception ex)
                { throw ex; }
            }

            public static string GetMobileNumber()
            {
                try
                {
                    String TargetPath = System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                    TargetPath = Path.Combine(TargetPath, "AMUStatus.txt");
                    if (System.IO.File.Exists(TargetPath) == false)
                    { throw new AMUStatusFileNotFoundException(null); }
                    else
                    { return System.IO.File.ReadAllText(TargetPath).Split(';')[0]; }
                }
                catch (AMUStatusFileNotFoundException ex)
                { throw ex; }
                catch (Exception ex)
                { throw ex; }
            }

            public static string GetTargetPath()
            {
                String TargetPath = System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                TargetPath = Path.Combine(TargetPath, "AMUStatus.txt");
                return TargetPath;
            }

        }

    }

    namespace Updating
    {
        public class ATISMobileMClassUpdating
        {
            public async void UpdateAPP()
            {
                //try
                //{
                //    HttpClient _Client = new HttpClient();
                //    Xamarin.Essentials.VersionTracking.Track();
                //    string VersionNumber = Xamarin.Essentials.VersionTracking.CurrentVersion;
                //    string VersionName = Xamarin.Essentials.VersionTracking.CurrentBuild;

                //    var responseVersion = await _Client.GetAsync(ATISMobileMClassPublicProcedures.ATISHostURL() + "/api/VersionControl?YourVersionNumber=" + VersionNumber + "&YourVersionName=" + VersionName);
                //    if (responseVersion.IsSuccessStatusCode)
                //    {
                //        var content = await responseVersion.Content.ReadAsStringAsync();
                //        if (!JsonConvert.DeserializeObject<bool>(content)) return;
                //    }
                //    bool answer = await DisplayAlert("بروز رسانی آتیس موبایل", "آتیس موبایل اخیرا تغییراتی داشته است", "OK");

                //    ATISMobile.Droid.PublicProcedures.ATISMobileMClassPublicProcedures.ViewMessage(this, "آپدیت اپلیکیشن - لطفا تا پایان آپدیت ورژن جدید اپلیکیشن منتظر بمانید");

                //    String RemoteFtpPath = ATISMobile.Properties.Resources.APKFtpURL;
                //    //String TargetPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "ATISMobile.apk");
                //    String TargetPath = Path.Combine(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).ToString(), "ATISMobile.apk");
                //    //String TargetPath = System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                //    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(RemoteFtpPath);
                //    request.Method = WebRequestMethods.Ftp.DownloadFile;
                //    request.KeepAlive = false; request.UsePassive = true; request.UseBinary = true;
                //    request.Credentials = new NetworkCredential(string.Empty, string.Empty);
                //    FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                //    Stream responseStream = response.GetResponseStream();
                //    using (FileStream writer = new FileStream(TargetPath, FileMode.Create))
                //    {
                //        int bufferSize = 2048;
                //        int readCount;
                //        byte[] buffer = new byte[2048];

                //        readCount = responseStream.Read(buffer, 0, bufferSize);
                //        while (readCount > 0)
                //        {
                //            writer.Write(buffer, 0, readCount);
                //            readCount = responseStream.Read(buffer, 0, bufferSize);
                //        }
                //    }
                //    response.Close();
                //    responseStream.Close();

                //    //var destination = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments ), "ATISMobile.apk");
                //    var destination = Path.Combine(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).ToString(), "ATISMobile.apk");
                //    Intent install = new Intent(Intent.ActionInstallPackage);
                //    install.SetDataAndType(Android.Net.Uri.FromFile(new Java.IO.File(destination)), "application/vnd.android.package-archive");
                //    install.SetFlags(ActivityFlags.ClearTask | ActivityFlags.NewTask);
                //    install.AddFlags(ActivityFlags.GrantReadUriPermission);
                //    this.StartActivity(install);

                //    //var activity = (Activity)this;
                //    //activity.FinishAffinity();

                //}
                //catch (Exception ex)
                //{ await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("خطا در فرآیند آپدیت اپلیکیشن", ex.Message, "تایید"); }

            }

        }


    }

    namespace SecurityAlgorithmsManagement
    {
        namespace HashingAlgorithms
        {
            public class Hashing
            {
                public static string GetSHA256Hash(String input)
                {
                    SHA256 shaM = new SHA256Managed();
                    byte[] data = shaM.ComputeHash(Encoding.UTF8.GetBytes(input));
                    StringBuilder sBuilder = new StringBuilder();
                    for (int i = 0; i < data.Length; i++)
                    { sBuilder.Append(data[i].ToString("x2")); }
                    input = sBuilder.ToString();
                    return (input);
                }
            }
        }

        public class ImageRawData
        { public byte[] IRawData; }

        public class Nonce
        {
            public Nonce()
            { }

            private static string _CurrentNonce = string.Empty;
            public static string CurrentNonce
            { get { return _CurrentNonce; } }

            public static async Task<HttpResponseMessage> GetNonce()
            {
                try
                {
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/api/SoftwareUsers/GetNonce");
                    var Content = ATISMobileWebApiMClassManagement.GetMobileNumber() + ";" + Hashing.GetSHA256Hash(ATISMobileWebApiMClassManagement.GetApiKey());
                    request.Content = new StringContent(JsonConvert.SerializeObject(Content), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await HttpClientOnlyInstance.HttpClientInstance().SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        _CurrentNonce = JsonConvert.DeserializeObject<string>(content);
                    }
                    else
                    { throw new Exception(JsonConvert.DeserializeObject<string>(response.Content.ReadAsStringAsync().Result)); }
                    return response;
                }
                catch (Exception ex)
                { throw ex; }
            }

        }


    }

    namespace Exceptions
    {
        public class AMUStatusFileNotFoundException : Exception
        {
            public AMUStatusFileNotFoundException(string message) : base(message)
            {
                message = "خطای بانک اطلاعاتی - مجددا تلاش نمایید";
            }
        }
    }

    namespace HttpClientInstance
    {
        public class HttpClientOnlyInstance
        {
            private static HttpClient _HttpClient = null;
            public static HttpClient HttpClientInstance()
            {
                try
                {

                    if (_HttpClient is null)
                    {
                        Uri baseUri = new Uri(ATISMobileWebApiMClassManagement.ATISMobileWebApiHostUrlHolder);
                        _HttpClient = new HttpClient();
                        _HttpClient.BaseAddress = baseUri;
                        _HttpClient.DefaultRequestHeaders.Clear();
                        _HttpClient.DefaultRequestHeaders.ConnectionClose = false;
                        _HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        try
                        {
                            ServicePointManager.FindServicePoint(baseUri).ConnectionLeaseTimeout = 60 * 1000;
                        }
                        catch (Exception ex)
                        {; }
                        ServicePointManager.DnsRefreshTimeout = 100;
                        return _HttpClient;
                    }
                    else
                    { return _HttpClient; }
                }
                catch (Exception ex)
                { throw ex; }
            }
        }
    }
}

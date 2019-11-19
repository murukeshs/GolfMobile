using Acr.UserDialogs;
using Golf.Models;
using Golf.Services;
using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Net.Http;
using Xamarin.Forms;

namespace Golf.ViewModel
{
    public class OTPVerificationViewModel : BaseViewModel
    {
        public string OTP
        {
            get
            {
                return _OTP;
            }
            set
            {
                _OTP = value;
                OnPropertyChanged();
            }
        }
        private string _OTP = string.Empty;

        public OTPVerificationViewModel()
        {
            generateOTP();
        }
        #region generateOTP
        public async void generateOTP()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading();
                    var RestURL = App.User.BaseUrl + "User/generateEmailOTP";
                    Uri requestUri = new Uri(RestURL);

                    var data = new GenerateOTPEmail
                    {
                        email = "swathit@apptomate.co",
                        type = "Email Verify"
                    };

                    string json = JsonConvert.SerializeObject(data);
                    var httpClient = new HttpClient();
                    HttpResponseMessage response = await httpClient.PostAsync(requestUri, new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
                    var content = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        UserDialogs.Instance.HideLoading();
                        UserDialogs.Instance.Alert("OTP Successfully Generated", "Alert", "Ok");
                    }
                    else
                    {
                        var error = JsonConvert.DeserializeObject<error>(content);
                        UserDialogs.Instance.HideLoading();
                        UserDialogs.Instance.Alert(error.errorMessage, "Alert", "Ok");
                    }
                }
                else
                {
                    DependencyService.Get<IToast>().Show("Please check internet connection");
                }
            }
            catch (Exception ex)
            {
                var a = ex.Message;
                UserDialogs.Instance.HideLoading();
                DependencyService.Get<IToast>().Show("Something went wrong, please try again later");
            }
        }
        #endregion


        #region validateOTP
        public async void verifyOTP()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading();
                    var RestURL = App.User.BaseUrl + "User/verifyOTP";
                    Uri requestUri = new Uri(RestURL);

                    var data = new VerifyOTP
                    {
                        otpValue=OTP,
                        emailorPhone="swathit@apptomate.co",
                        type= "Email Verify",
                        sourceType="Email"
                    };

                    string json = JsonConvert.SerializeObject(data);
                    var httpClient = new HttpClient();
                    HttpResponseMessage response = await httpClient.PostAsync(requestUri, new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
                    var content = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        UserDialogs.Instance.HideLoading();
                        UserDialogs.Instance.Alert("OTP Validated", "Alert", "Ok");
                    }
                    else
                    {
                        var error = JsonConvert.DeserializeObject<error>(content);
                        UserDialogs.Instance.HideLoading();
                        UserDialogs.Instance.Alert(error.errorMessage, "Alert", "Ok");
                    }
                }
                else
                {
                    DependencyService.Get<IToast>().Show("Please check internet connection");
                }
            }
            catch (Exception ex)
            {
                var a = ex.Message;
                UserDialogs.Instance.HideLoading();
                DependencyService.Get<IToast>().Show("Something went wrong, please try again later");
            }
        }
        #endregion
    }
}

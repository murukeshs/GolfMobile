using Acr.UserDialogs;
using Golf.Models;
using Golf.Services;
using Golf.Utils;
using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
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
        public string Email
        {
            get
            {
                return _Email;
            }
            set
            {
                _Email = value;
                OnPropertyChanged();
            }
        }
        private string _Email = string.Empty;

        public OTPVerificationViewModel()
        {
            generateOTP();
            Email = App.User.OtpEmail;
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
                        email = "swathit@apptomate.co",  // Email
                        type = "Email Verify"
                    };

                    string json = JsonConvert.SerializeObject(data);
                    var httpClient = new HttpClient();
                    HttpResponseMessage response = await httpClient.PutAsync(requestUri, new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
                    var content = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        UserDialogs.Instance.HideLoading();
                        UserDialogs.Instance.Alert("OTP Successfully Generated", "OTP Verification", "Ok");
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

        public ICommand VerifyOtpCommand => new AsyncCommand(verifyOTP);
        public async Task verifyOTP()
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
                        emailorPhone="swathit@apptomate.co", // Email
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
                        UserDialogs.Instance.Alert("OTP Verified successfully.", "Success", "Ok");
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

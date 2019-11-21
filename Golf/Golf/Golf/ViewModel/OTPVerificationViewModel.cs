using Acr.UserDialogs;
using Golf.Models;
using Golf.Services;
using Golf.Utils;
using Golf.Views.MenuView;
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

        public string FirstDigit
        {
            get
            {
                return _FirstDigit;
            }
            set
            {
                _FirstDigit = value;
                OnPropertyChanged("FirstDigit");
            }
        }
        private string _FirstDigit = string.Empty;

        public string SecondDigit
        {
            get
            {
                return _SecondDigit;
            }
            set
            {
                _SecondDigit = value;
                OnPropertyChanged("SecondDigit");
            }
        }
        private string _SecondDigit = string.Empty;

        public string ThirdDigit
        {
            get
            {
                return _ThirdDigit;
            }
            set
            {
                _ThirdDigit = value;
                OnPropertyChanged("ThirdDigit");
            }
        }
        private string _ThirdDigit = string.Empty;

        public string FourthDigit
        {
            get
            {
                return _FourthDigit;
            }
            set
            {
                _FourthDigit = value;
                OnPropertyChanged("FourthDigit");
            }
        }
        private string _FourthDigit = string.Empty;


        public string FifthDigit
        {
            get
            {
                return _FifthDigit;
            }
            set
            {
                _FifthDigit = value;
                OnPropertyChanged("FifthDigit");
            }
        }
        private string _FifthDigit = string.Empty;

        public string SixthDigit
        {
            get
            {
                return _SixthDigit;
            }
            set
            {
                _SixthDigit = value;
                OnPropertyChanged("SixthDigit");
            }
        }
        private string _SixthDigit = string.Empty;

        public string Email
        {
            get
            {
                return _Email;
            }
            set
            {
                _Email = value;
                OnPropertyChanged(nameof(Email));
            }
        }
        private string _Email = string.Empty;

        public OTPVerificationViewModel()
        {
            Email = App.User.OtpEmail;
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
                        email = Email,
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
                        otpValue=FirstDigit + SecondDigit + ThirdDigit + FourthDigit + FifthDigit + SixthDigit,
                        emailorPhone=Email,
                        type= "Email Verify",
                        sourceType="Email"
                    };

                    string json = JsonConvert.SerializeObject(data);
                    var httpClient = new HttpClient();
                    HttpResponseMessage response = await httpClient.PutAsync(requestUri, new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
                    var content = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        UserDialogs.Instance.HideLoading();
                        await UserDialogs.Instance.AlertAsync("OTP Verified successfully.", "Success", "Ok");
                        var view = new RegistrationPageAdmin();
                        var navigationPage = ((NavigationPage)App.Current.MainPage);
                        await navigationPage.PushAsync(view);
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

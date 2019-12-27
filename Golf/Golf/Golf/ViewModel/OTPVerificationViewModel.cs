using Acr.UserDialogs;
using Golf.Models;
using Golf.Services;
using Golf.Utils;
using Golf.Views;
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
        public OTPVerificationViewModel()
        {
            Email = App.User.OtpEmail;
            GenerateOTP();
        }

        #region Property Declaration

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

        #endregion

        #region Resend OTP Command Functionality
        public ICommand ResendOTPCommand => new AsyncCommand(ResendOTP);

        async Task ResendOTP()
        {
            GenerateOTP();
        }

        #endregion

        #region generateOTP
        public async void GenerateOTP()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading();

                    var data = new GenerateOTPEmail
                    {
                        emailorphone = Email,
                        type = "Email Verify",
                        sourceType = "Email"
                    };

                    var result = await App.ApiClient.GenerateOTP(data);

                    if (result != null)
                    {
                        UserDialogs.Instance.Alert("OTP Successfully Generated", "OTP Verification", "Ok");
                    }

                    UserDialogs.Instance.HideLoading();

                    //var RestURL = App.User.BaseUrl + "User/generateOTP";
                    //Uri requestUri = new Uri(RestURL);
                    //string json = JsonConvert.SerializeObject(data);
                    //var httpClient = new HttpClient();
                    //HttpResponseMessage response = await httpClient.PutAsync(requestUri, new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
                    //var content = await response.Content.ReadAsStringAsync();
                    //if (response.IsSuccessStatusCode)
                    //{
                    //    UserDialogs.Instance.HideLoading();
                    //    UserDialogs.Instance.Alert("OTP Successfully Generated", "OTP Verification", "Ok");
                    //}
                    //else
                    //{
                    //    var error = JsonConvert.DeserializeObject<error>(content);
                    //    UserDialogs.Instance.HideLoading();
                    //    UserDialogs.Instance.Alert(error.errorMessage, "Alert", "Ok");
                    //}
                }
                else
                {
                    DependencyService.Get<IToast>().Show("Please check internet connection");
                }
            }
            catch (Exception ex)
            {
                var a = ex.Message;
                if (a == "System.Net.WebException")
                {
                    UserDialogs.Instance.HideLoading();
                    DependencyService.Get<IToast>().Show("Please check internet connection");
                }
                else
                {
                    UserDialogs.Instance.HideLoading();
                    DependencyService.Get<IToast>().Show("Something went wrong, please try again later");
                }
            }
        }
        #endregion

        #region validateOTP

        public ICommand VerifyOtpCommand => new AsyncCommand(VerifyOTP);
        public async Task VerifyOTP()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading();
                    
                    var data = new VerifyOTP
                    {
                        otpValue=FirstDigit + SecondDigit + ThirdDigit + FourthDigit + FifthDigit + SixthDigit,
                        emailorPhone=Email,
                        type= "Email Verify",
                        sourceType="Email"
                    };

                    var result = await App.ApiClient.VerifyOTP(data);

                    if (result != null)
                    {
                        await UserDialogs.Instance.AlertAsync("OTP Verified successfully.", "Success", "Ok");
                        if (App.User.FromEmailNotValid)
                        {
                            var view = new LoginPage();
                            var navigationPage = ((NavigationPage)App.Current.MainPage);
                            await navigationPage.PushAsync(view);
                        }
                        else
                        {
                            var view = new RegistrationPageAdmin();
                            var navigationPage = ((NavigationPage)App.Current.MainPage);
                            await navigationPage.PushAsync(view);
                        }
                    }

                    UserDialogs.Instance.HideLoading();

                    //var RestURL = App.User.BaseUrl + "User/verifyOTP";
                    //Uri requestUri = new Uri(RestURL);
                    //string json = JsonConvert.SerializeObject(data);
                    //var httpClient = new HttpClient();
                    //HttpResponseMessage response = await httpClient.PutAsync(requestUri, new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
                    //var content = await response.Content.ReadAsStringAsync();
                    //if (response.IsSuccessStatusCode)
                    //{
                    //    UserDialogs.Instance.HideLoading();
                    //    await UserDialogs.Instance.AlertAsync("OTP Verified successfully.", "Success", "Ok");
                    //    if(App.User.FromEmailNotValid)
                    //    {
                    //        var view = new LoginPage();
                    //        var navigationPage = ((NavigationPage)App.Current.MainPage);
                    //        await navigationPage.PushAsync(view);
                    //    }
                    //    else
                    //    {
                    //        var view = new RegistrationPageAdmin();
                    //        var navigationPage = ((NavigationPage)App.Current.MainPage);
                    //        await navigationPage.PushAsync(view);
                    //    }
                    //}
                    //else
                    //{
                    //    var error = JsonConvert.DeserializeObject<error>(content);
                    //    UserDialogs.Instance.HideLoading();
                    //    UserDialogs.Instance.Alert(error.errorMessage, "Alert", "Ok");
                    //}
                }
                else
                {
                    DependencyService.Get<IToast>().Show("Please check internet connection");
                }
            }
            catch (Exception ex)
            {
                var a = ex.Message;
                if (a == "System.Net.WebException")
                {
                    UserDialogs.Instance.HideLoading();
                    DependencyService.Get<IToast>().Show("Please check internet connection");
                }
                else
                {
                    UserDialogs.Instance.HideLoading();
                    DependencyService.Get<IToast>().Show("Something went wrong, please try again later");
                }
            }
        }
        #endregion
    }
}

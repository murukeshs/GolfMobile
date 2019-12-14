using Acr.UserDialogs;
using Golf.Controls;
using Golf.Models;
using Golf.Services;
using Golf.Views;
using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Golf.ViewModel
{
    public class ForgotPasswordViewModel : BaseViewModel
    {
        #region Property Declaration

        public bool IsValid
        {
            get
            {
                return _IsValid;
            }
            set
            {
                _IsValid = value;
                OnPropertyChanged(nameof(IsValid));
            }
        }
        private bool _IsValid = false;
        public string EmailOrPhone
        {
            get
            {
                return _EmailOrPhone;
            }
            set
            {
                _EmailOrPhone = value;
                OnPropertyChanged(nameof(EmailOrPhone));
            }
        }
        private string _EmailOrPhone = string.Empty;

        public string Otp
        {
            get
            {
                return _Otp;
            }
            set
            {
                _Otp = value;
                OnPropertyChanged(nameof(Otp));
            }
        }
        private string _Otp = string.Empty;

        public string Password
        {
            get
            {
                return _Password;
            }
            set
            {
                _Password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        private string _Password = string.Empty;

        public string ConfirmPassword
        {
            get
            {
                return _ConfirmPassword;
            }
            set
            {
                _ConfirmPassword = value;
                OnPropertyChanged(nameof(ConfirmPassword));
            }
        }
        private string _ConfirmPassword = string.Empty;

        public bool IsEmail
        {
            get
            {
                return _IsEmail;
            }
            set
            {
                _IsEmail = value;
                OnPropertyChanged(nameof(IsEmail));
            }
        }
        private bool _IsEmail = false;

        public bool IsSms
        {
            get
            {
                return _IsSms;
            }
            set
            {
                _IsSms = value;
                OnPropertyChanged(nameof(IsSms));
            }
        }
        private bool _IsSms = false;
         
        public bool OtpGenerateLayout
        {
            get
            {
                return _OtpGenerateLayout;
            }
            set
            {
                _OtpGenerateLayout = value;
                OnPropertyChanged(nameof(OtpGenerateLayout));
            }
        }
        private bool _OtpGenerateLayout = true;

        public bool UpdatePasswordLayout
        {
            get
            {
                return _UpdatePasswordLayout;
            }
            set
            {
                _UpdatePasswordLayout = value;
                OnPropertyChanged(nameof(UpdatePasswordLayout));
            }
        }
        private bool _UpdatePasswordLayout = false;

        #endregion

        #region OtpVia checkbox changed
        public ICommand OtpViaCheckedChangedCommand => new Command<CustomCheckBox>(OtpViaOnChange);

        public void OtpViaOnChange(CustomCheckBox OtpViaValues)
        {
            var Type = OtpViaValues.DefaultValue;
            var Value = OtpViaValues.Checked;
            if (Type == "1")
            {
                IsEmail = Value;
                IsSms = !Value;
            }
            else
            {
                IsSms = Value;
                IsEmail = !Value;
            }
        }
        #endregion

        #region Resend OTP Command Functionalit

        public ICommand ResendOTPCommand => new Command(ContinueAsync);

        #endregion

        #region Continue Clicked Command

        public ICommand ContinueClickedCommand => new Command(ContinueAsync);

        async void ContinueAsync()
        {
            IsValid = Validate();
            if (IsValid)
            {
                await ContinueOnClick();
            }
        }

        bool Validate()
        { 
            if (!IsEmail && !IsSms)
            {
                UserDialogs.Instance.AlertAsync("Select Anyone Checkbox", "Alert", "Ok");
                return false;
            }
            else if (IsEmail && string.IsNullOrEmpty(EmailOrPhone))
            {
                UserDialogs.Instance.AlertAsync("Email should not be empty.", "Alert", "Ok");
                return false;
            }
            else if (IsEmail && !Regex.IsMatch(EmailOrPhone, "^\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*$"))
            {
                UserDialogs.Instance.AlertAsync("Email format is not valid.", "Alert", "Ok");
                return false;
            }
            else if (IsSms && string.IsNullOrEmpty(EmailOrPhone))
            {
                UserDialogs.Instance.AlertAsync("PhoneNumber should not be empty.", "Alert", "Ok");
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task ContinueOnClick()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading();
                    var RestURL = App.User.BaseUrl + "User/generateOTP";
                    Uri requestUri = new Uri(RestURL);

                    var data = new GenerateOTPEmail
                    {
                        emailorphone = EmailOrPhone,
                        type = "Forgot Password",
                        sourceType = IsEmail ? "Email" : "Sms"
                    };

                    string json = JsonConvert.SerializeObject(data);
                    var httpClient = new HttpClient();
                    HttpResponseMessage response = await httpClient.PutAsync(requestUri, new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
                    var content = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        UserDialogs.Instance.HideLoading();
                        await UserDialogs.Instance.AlertAsync("OTP Successfully Generated", "OTP Verification", "Ok");
                        OtpGenerateLayout = false;
                        UpdatePasswordLayout = true;
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

        #region Validate Otp Command

        public ICommand UpdatePasswordCommand => new Command(UpdatePasswordAsync);

        async void UpdatePasswordAsync()
        {
            IsValid = UpdatePasswordValidate();
            if (IsValid)
            {
                await UpdatePassword();
            }
        }

        bool UpdatePasswordValidate()
        {
            if(string.IsNullOrEmpty(Otp))
            {
                UserDialogs.Instance.AlertAsync("Otp should not be empty", "Alert", "Ok");
                return false;
            }
            if (string.IsNullOrEmpty(Password))
            {
                UserDialogs.Instance.AlertAsync("Password should not be empty", "Alert", "Ok");
                return false;
            }
            else if (string.IsNullOrEmpty(ConfirmPassword))
            {
                UserDialogs.Instance.AlertAsync("Confirm Password should not be empty.", "Alert", "Ok");
                return false;
            }
            else if (Password != ConfirmPassword)
            {
                UserDialogs.Instance.AlertAsync("Password must be same as Confirm Password", "Alert", "Ok");
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task UpdatePassword()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading();
                    var RestURL = App.User.BaseUrl + "User/updatePassword";
                    Uri requestUri = new Uri(RestURL);

                    var data = new UpdatePassword
                    {
                        otpValue = Otp,
                        emailorPhone = EmailOrPhone,
                        sourceType = IsEmail ? "Email" : "sms",
                        password = Password
                    };

                    string json = JsonConvert.SerializeObject(data);
                    var httpClient = new HttpClient();
                    HttpResponseMessage response = await httpClient.PutAsync(requestUri, new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
                    var content = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        UserDialogs.Instance.HideLoading();
                        await UserDialogs.Instance.AlertAsync("Password Updated Successfully.", "Success", "Ok");
                        var view = new LoginPage();
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

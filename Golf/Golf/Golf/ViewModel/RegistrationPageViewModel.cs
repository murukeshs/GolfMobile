using Acr.UserDialogs;
using Golf.Models;
using Golf.Models.userModel;
using Golf.Services;
using Golf.Utils;
using Golf.Views;
using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Golf.ViewModel
{
    public class RegistrationPageViewModel : BaseViewModel
    {

        public RegistrationPageViewModel()
        {
            GenderList = new List<string>(new[] { "Male", "Female"});
        }

        #region Property Declaration

        public IList<string> GenderList { get; set; }

        public string FirstNameText
        {
            get
            {
                return _FirstNameText;
            }
            set
            {
                _FirstNameText = value;
                OnPropertyChanged();
            }
        }
        private string _FirstNameText = string.Empty;

        public string LastNameText
        {
            get
            {
                return _LastNameText;
            }
            set
            {
                _LastNameText = value;
                OnPropertyChanged();
            }
        }
        private string _LastNameText = string.Empty;

        public string NickNameText
        {
            get
            {
                return _NickNameText;
            }
            set
            {
                _NickNameText = value;
                OnPropertyChanged(nameof(NickNameText));
            }
        }
        private string _NickNameText = string.Empty;

        public string EmailText
        {
            get
            {
                return _EmailText;
            }
            set
            {
                _EmailText = value;
                OnPropertyChanged();
            }
        }
        private string _EmailText = string.Empty;

        public string GenderText
        {
            get
            {
                return _GenderText;
            }
            set
            {
                _GenderText = value;
                OnPropertyChanged();
            }
        }
        private string _GenderText = string.Empty;

        public DateTime? dob
        {
            get
            {
                return _dob;
            }
            set
            {
                _dob = value;
                OnPropertyChanged(nameof(dob));
            }
        }
        private DateTime? _dob = null;

        public DateTime? Nullabledob
        {
            get
            {
                return _Nullabledob;
            }
            set
            {
                _Nullabledob = value;
                OnPropertyChanged(nameof(Nullabledob));
            }
        }
        private DateTime? _Nullabledob = null;

        public string PhoneNumber
        {
            get
            {
                return _PhoneNumber;
            }
            set
            {
                _PhoneNumber = value;
                OnPropertyChanged();
            }
        }
        private string _PhoneNumber = string.Empty;

        public string Password
        {
            get
            {
                return _Password;
            }
            set
            {
                _Password = value;
                OnPropertyChanged();
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
                OnPropertyChanged();
            }
        }
        private string _ConfirmPassword = string.Empty;

        #endregion

        #region Register Command Functionality

        public bool IsValid { get; set; }

        public ICommand RegisterCommand => new AsyncCommand(RegisterAsync);

        async Task RegisterAsync()
        {
            IsValid = Validate();
            if (IsValid)
            {
                await Register();
            }
        }

        bool Validate()
        {
            if (string.IsNullOrEmpty(FirstNameText))
            {
                //First Name Is Empty
                UserDialogs.Instance.AlertAsync("FirstName should not be empty.", "Alert", "Ok");
                return false;
            }
            else if (string.IsNullOrEmpty(LastNameText))
            {
                //LastName Is Empty
                UserDialogs.Instance.AlertAsync("LastName cannot be empty.", "Alert", "Ok");
                return false;
            }
            else if (string.IsNullOrEmpty(NickNameText))
            {
                //NickName Is Empty
                UserDialogs.Instance.AlertAsync("NickName cannot be empty.", "Alert", "Ok");
                return false;
            }
            else if (string.IsNullOrEmpty(EmailText))
            {
                //Email Is Empty
                UserDialogs.Instance.AlertAsync("Email cannot be empty.", "Alert", "Ok");
                return false;
            }
            else if (!Regex.IsMatch(EmailText, "^\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*$"))
            {
                UserDialogs.Instance.AlertAsync("Email format is not valid.", "Alert", "Ok");
                return false;
            }
            else if (string.IsNullOrEmpty(GenderText))
            {
                //Gender Is Empty
                UserDialogs.Instance.AlertAsync("Gender cannot be empty.", "Alert", "Ok");
                return false;
            }
            else if (dob == null)
            {
                //dob Is Empty
                UserDialogs.Instance.AlertAsync("DOB cannot be empty.", "Alert", "Ok");
                return false;
            }
            else if (string.IsNullOrEmpty(PhoneNumber))
            {
                //PhoneNo Is Empty
                UserDialogs.Instance.AlertAsync("Phone Number cannot be empty.", "Alert", "Ok");
                return false;
            }
            else if (string.IsNullOrEmpty(Password))
            {
                //Password Is Empty
                UserDialogs.Instance.AlertAsync("Password cannot be empty.", "Alert", "Ok");
                return false;
            }
            else if (Password != ConfirmPassword)
            {
                //Password check with confirm password
                UserDialogs.Instance.AlertAsync("Confirm password must be same as password.", "Alert", "Ok");
                return false;
            }
            else
            {
                //Validation Is Success
                return true;
            }
        }
        async Task Register()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    var DateofBirth = dob.Value.ToString("yyyy/MM/dd");
                    UserDialogs.Instance.ShowLoading();
                    string RestURL = App.User.BaseUrl + "User/createUser";
                    Uri requestUri = new Uri(RestURL);

                    var data = new createUser
                    {
                        userId= 0,
                        firstName= FirstNameText,
                        lastName= LastNameText,
                        nickName = NickNameText,
                        email = EmailText,
                        gender= GenderText,
                        dob= DateofBirth,
                        phoneNumber= PhoneNumber,
                        password= Password,
                        userTypeId = "1"           //Player
                    };

                    string json = JsonConvert.SerializeObject(data);
                    var httpClient = new HttpClient();
                    HttpResponseMessage response = await httpClient.PostAsync(requestUri, new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
                    string responJsonText = await response.Content.ReadAsStringAsync();
                    
                    if (response.IsSuccessStatusCode)
                    {
                        RegisterResponse res = JsonConvert.DeserializeObject<RegisterResponse>(responJsonText);
                        App.User.UserId = res.userId;
                        App.User.OtpEmail = EmailText;
                        var view = new OtpVerificationPage();
                        var navigationPage = ((NavigationPage)App.Current.MainPage);
                        await navigationPage.PushAsync(view);
                        resetFormValues();
                        UserDialogs.Instance.HideLoading();
                    }
                    else
                    {
                        var error = JsonConvert.DeserializeObject<error>(responJsonText);
                        UserDialogs.Instance.HideLoading();
                        UserDialogs.Instance.Alert(error.errorMessage, "Alert", "Ok");
                    }
                }
                else
                {
                    UserDialogs.Instance.HideLoading();
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

        public void resetFormValues()
        {
            FirstNameText = string.Empty;
            LastNameText = string.Empty;
            EmailText = string.Empty;
            GenderText = string.Empty ;
            dob = null;
            PhoneNumber = string.Empty;
            Password = string.Empty;
            ConfirmPassword = string.Empty;
        }
        #endregion Register Command Functionality

        #region GenderOnChange Command

        public ICommand GenderOnChangeCommand => new Command<string>(GenderOnChange);

        public void GenderOnChange(string gender)
        {
            GenderText = gender;
        }

        #endregion

        #region DateOnChange Command

        public ICommand DOBSelectedCommand => new Command<string>(DOBSelected);

        public void DOBSelected(string value)
        {
            dob = Convert.ToDateTime(value);
        }

        #endregion
    }
}

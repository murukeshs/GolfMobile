using Acr.UserDialogs;
using Golf.Models;
using Golf.Models.userModel;
using Golf.Services;
using Golf.Utils;
using Golf.Views;
using Golf.Views.MenuView;
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
        public bool IsValid { get; set; }
        public IList<string> GenderList { get; set; }
        public RegistrationPageViewModel()
        {
            GenderList = new List<string>();
            GenderList.Add("Male");
            GenderList.Add("Female");
        }
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

        #region Register Command Functionality
        public ICommand RegisterCommand => new AsyncCommand(RegisterAsync);
        async Task RegisterAsync()
        {
            var view = new OtpVerificationPage();
            var navigationPage = ((NavigationPage)App.Current.MainPage);
            await navigationPage.PushAsync(view);
            //IsValid = Validate();
            //if (IsValid)
            //{
            //    await Register();
            //}
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
            else if(UserTypeList.Count == 0){
                //UserType Is Empty
                UserDialogs.Instance.AlertAsync("User Type cannot be empty.", "Alert", "Ok");
                return false;
            }
            else if (string.IsNullOrEmpty(Password))
            {
                //Password Is Empty
                UserDialogs.Instance.AlertAsync("Password cannot be empty.", "Alert", "Ok");
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
                    var UserTypeId = string.Join(",", UserTypeList);
                    UserDialogs.Instance.ShowLoading();
                    string RestURL = App.User.BaseUrl + "User/createUser";
                    Uri requestUri = new Uri(RestURL);

                    var data = new createUser
                    {
                        userId= 0,
                        firstName= FirstNameText,
                        lastName= LastNameText,
                        email= EmailText,
                        gender= GenderText,
                        dob= dob.ToString(),
                        phoneNumber= PhoneNumber,
                        password= Password,
                        userTypeId= UserTypeId
                    };

                    string json = JsonConvert.SerializeObject(data);
                    var httpClient = new HttpClient();
                    HttpResponseMessage response = await httpClient.PostAsync(requestUri, new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
                    string responJsonText = await response.Content.ReadAsStringAsync();
                    
                    if (response.IsSuccessStatusCode)
                    {
                        RegisterResponse res = JsonConvert.DeserializeObject<RegisterResponse>(responJsonText);
                        App.User.UserId = res.userId;

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
            UserTypeList.Clear();
        }
        #endregion Register Command Functionality

        public List<string> UserTypeList = new List<string>();
        public ICommand UserTypeCheckBoxCommand => new Command(UserTypeCheckBox);
        public void UserTypeCheckBox(object parameter)
        {
            var item = parameter as string;

            if (UserTypeList.Count > 0)
            {
                bool UserIdAleradyExists = UserTypeList.Contains(item);
                if (UserIdAleradyExists)
                {
                    UserTypeList.Remove(item);
                }
                else
                {
                    UserTypeList.Add(item);
                }
            }
            else
            {
                UserTypeList.Add(item);
            }
        }
    }
}

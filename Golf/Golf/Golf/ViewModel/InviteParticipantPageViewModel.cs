using Acr.UserDialogs;
using Golf.Models;
using Golf.Services;
using Golf.Utils;
using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Golf.ViewModel
{
    public class InviteParticipantPageViewModel : BaseViewModel
    {
        public bool IsValid { get; set; }
        public IList<string> GenderList { get; set; }
        public InviteParticipantPageViewModel()
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

        #region Send Invite Button Command Functionality
        public ICommand InviteParticipantCommand => new AsyncCommand(SendInviteAsync);

        async Task SendInviteAsync()
        {
            IsValid = Validate();
            if (IsValid)
            {
                await SendInviteByEmail();
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
            else if (string.IsNullOrEmpty(PhoneNumber))
            {
                //PhoneNo Is Empty
                UserDialogs.Instance.AlertAsync("Phone Number cannot be empty.", "Alert", "Ok");
                return false;
            }
            else if (UserTypeList.Count == 0)
            {
                //UserType Is Empty
                UserDialogs.Instance.AlertAsync("User Type cannot be empty.", "Alert", "Ok");
                return false;
            }
            else
            {
                //Validation Is Success
                return true;
            }
        }

        async Task SendInviteByEmail()
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
                        email = EmailText,
                        type = "Email Verify"
                    };

                    string json = JsonConvert.SerializeObject(data);
                    var httpClient = new HttpClient();
                    HttpResponseMessage response = await httpClient.PostAsync(requestUri, new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
                    var content = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        UserDialogs.Instance.HideLoading();
                        UserDialogs.Instance.Alert("Invitation Sent", "Alert", "Ok");
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
        #endregion Send Invite Button Command Functionality

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

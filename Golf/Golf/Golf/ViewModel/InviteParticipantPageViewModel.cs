using Acr.UserDialogs;
using Golf.Models;
using Golf.Models.userModel;
using Golf.Services;
using Golf.Utils;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Golf.ViewModel
{
    public class InviteParticipantPageViewModel : BaseViewModel
    {
        public InviteParticipantPageViewModel()
        {
            GenderList = new List<string>(new[] {"Male", "Female"});
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
                OnPropertyChanged();
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

        #endregion

        #region Send Invite Button Command Functionality

        public bool IsValid { get; set; }

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
            else if (IsEmailNotification == false && IsSMSNotification == false)
            {
                //Invitation Via Not Selected
                UserDialogs.Instance.AlertAsync("The Mode of Invitation cannot be empty.", "Alert", "Ok");
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
                    var RestURL = App.User.BaseUrl + "User/inviteParticipant";
                    Uri requestUri = new Uri(RestURL);

                    var data = new createUser
                    {
                        firstName = FirstNameText,
                        lastName = LastNameText,
                        nickName = NickNameText,
                        email = EmailText,
                        gender = GenderText,
                        phoneNumber = PhoneNumber,
                        isEmailNotification = IsEmailNotification,
                        isSMSNotification = IsSMSNotification,
                        userTypeId = "1"
                    };

                    string json = JsonConvert.SerializeObject(data);
                    var httpClient = new HttpClient();
                    HttpResponseMessage response = await httpClient.PostAsync(requestUri, new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
                    var content = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        UserDialogs.Instance.HideLoading();
                        await UserDialogs.Instance.AlertAsync("Invitation Sent", "Invites", "Ok");
                        await PopupNavigation.Instance.PopAsync();
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
        #endregion Send Invite Button Command Functionality

        #region CommunicationVia Functionality

        public bool IsEmailNotification
        {
            get { return _IsEmailNotification; }
            set
            {
                _IsEmailNotification = value;
                OnPropertyChanged(nameof(IsEmailNotification));
            }
        }
        private bool _IsEmailNotification = false;

        public bool IsSMSNotification
        {
            get { return _IsSMSNotification; }
            set
            {
                _IsSMSNotification = value;
                OnPropertyChanged(nameof(IsSMSNotification));
            }
        }
        private bool _IsSMSNotification = false;

        public ICommand CommunicationViaCheckBoxCommand => new Command<string>(CommunicationViaCheckBox);

        public void CommunicationViaCheckBox(string type)
        {
            if(type == "Email")
            {
                IsEmailNotification = true;
            }
            else
            {
                IsSMSNotification = true;
            }
        }

        #endregion

        #region GenderOnchange Command

        public ICommand GenderOnChangeCommand => new Command<string>(GenderOnChange);

        public void GenderOnChange(string gender)
        {
                GenderText = gender;
        }

        #endregion
    }
}

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
using System.Threading.Tasks; 
using System.Windows.Input;
using Xamarin.Forms;

namespace Golf.ViewModel
{
    public class RegistrationPageAdminViewModel : BaseViewModel
    {

        public RegistrationPageAdminViewModel()
        {
            loadCountry();
        }

        #region Property Declaration

        public string Address
        {
            get
            {
                return _Address;
            }
            set
            {
                _Address = value;
                OnPropertyChanged(nameof(Address));
            }
        }
        private string _Address = string.Empty;
        public List<Country> CountryList
        {
            get { return _CountryList; }
            set
            {
                _CountryList = value;
                OnPropertyChanged(nameof(CountryList));
            }
        }
        private List<Country> _CountryList = null;

        public List<State> StateList
        {
            get { return _StateList; }
            set
            {
                _StateList = value;
                OnPropertyChanged(nameof(StateList));
            }
        }
        private List<State> _StateList = null;

        public string City
        {
            get
            {
                return _City;
            }
            set
            {
                _City= value;
                OnPropertyChanged(nameof(City));
            }
        }
        private string _City = string.Empty;

        public int CountryID
        {
            get { return _CountryID; }
            set
            {
                _CountryID = value;
                OnPropertyChanged(nameof(CountryID));
            }
        }
        private int _CountryID = 0;

        public int StateID
        {
            get { return _StateID; }
            set
            {
                _StateID = value;
                OnPropertyChanged(nameof(StateID));
            }
        }
        private int _StateID = 0;

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

        public bool IsPublicProfile
        {
            get { return _IsPublicProfile; }
            set
            {
                _IsPublicProfile = value;
                OnPropertyChanged(nameof(IsPublicProfile));
            }
        }
        private bool _IsPublicProfile = true;

        #endregion

        #region loadcountry
        public async void loadCountry()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading();

                    var result = await App.ApiClient.GetCountryList();

                    if (result != null)
                    {
                        CountryList = result;
                    }

                    UserDialogs.Instance.HideLoading();

                    //var RestURL = App.User.BaseUrl + "Country/GetCountryList";
                    //HttpClient client = new HttpClient();
                    //client.BaseAddress = new Uri(RestURL);
                    //HttpResponseMessage response = await client.GetAsync(RestURL);
                    //var content = await response.Content.ReadAsStringAsync();
                    //if (response.IsSuccessStatusCode)
                    //{
                    //    CountryList = JsonConvert.DeserializeObject<List<Country>>(content);
                    //    UserDialogs.Instance.HideLoading();
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

        #region loadstate

        public ICommand CountryChangedCommand => new Command<Country>(CountryOnChange);

        public async void CountryOnChange(Country country)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading();
                    CountryID = country.countryId;
                    var result = await App.ApiClient.GetStateList(country.countryId);
                    if (result != null)
                    {
                        StateList = result;
                        StateID = 0;
                    }

                    UserDialogs.Instance.HideLoading();

                    //var RestURL = App.User.BaseUrl + "Country/GetStateList/"+ country.countryId;
                    //HttpClient client = new HttpClient();
                    //client.BaseAddress = new Uri(RestURL);
                    //HttpResponseMessage response = await client.GetAsync(RestURL);
                    //var content = await response.Content.ReadAsStringAsync();
                    //if (response.IsSuccessStatusCode)
                    //{
                    //    StateID = 0;
                    //    StateList = JsonConvert.DeserializeObject<List<State>>(content);
                    //    UserDialogs.Instance.HideLoading();
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

        #region RegisterCommunicationInfo

        public bool IsValid { get; set; }

        public ICommand RegisterCommunicationInfoCommand => new AsyncCommand(RegisterCommunicationInfoCommandAsync);

        async Task RegisterCommunicationInfoCommandAsync()
        {
            IsValid = Validate();
            if (IsValid)
            {
                await Register();
            }
        }

        bool Validate()
        {
            if (string.IsNullOrEmpty(Address))
            {
                //Address Is Empty
                UserDialogs.Instance.AlertAsync("Address should not be empty.", "Alert", "Ok");
                return false;
            }
            else if (CountryID == 0)
            {
                //Country Is Empty
                UserDialogs.Instance.AlertAsync("Country cannot be empty.", "Alert", "Ok");
                return false;
            }
            else if (StateID == 0)
            {
                //State Is Empty
                UserDialogs.Instance.AlertAsync("State cannot be empty.", "Alert", "Ok");
                return false;
            }
            else if (string.IsNullOrEmpty(City))
            {
                //City Is Empty
                UserDialogs.Instance.AlertAsync("City cannot be empty.", "Alert", "Ok");
                return false;
            }
            else if (!IsEmailNotification && !IsSMSNotification )
            {
                //Notification Type Is Empty
                UserDialogs.Instance.AlertAsync("Notification Type cannot be empty.", "Alert", "Ok");
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
                    UserDialogs.Instance.ShowLoading();

                    var data = new createUser
                    {
                        userId = App.User.UserId,
                        address=Address,
                        stateId= StateID,
                        countryId= CountryID,
                        city = City,
                        isEmailNotification= IsEmailNotification,
                        isSMSNotification=IsSMSNotification,
                        isPublicProfile= IsPublicProfile
                    };

                    var result = await App.ApiClient.UpdateUserCommunicationInfo(data);

                    if (result != null)
                    {
                        UserDialogs.Instance.Alert("Communication Info Updated Successfully", "Success", "Ok");
                    }

                    UserDialogs.Instance.HideLoading();

                    //string RestURL = App.User.BaseUrl + "User/updateUserCommunicationinfo";
                    //Uri requestUri = new Uri(RestURL);
                    //string json = JsonConvert.SerializeObject(data);
                    //var httpClient = new HttpClient();
                    //HttpResponseMessage response = await httpClient.PutAsync(requestUri, new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
                    //string responJsonText = await response.Content.ReadAsStringAsync();

                    //if (response.IsSuccessStatusCode)
                    //{
                    //    await UserDialogs.Instance.AlertAsync("Registration successfully completed.","Success","Ok");
                    //    var view = new LoginPage();
                    //    var navigationPage = ((NavigationPage)App.Current.MainPage);
                    //    await navigationPage.PushAsync(view);
                    //    resetFormValues();
                    //    UserDialogs.Instance.HideLoading();
                    //}
                    //else
                    //{
                    //    var error = JsonConvert.DeserializeObject<error>(responJsonText);
                    //    UserDialogs.Instance.HideLoading();
                    //    UserDialogs.Instance.Alert(error.errorMessage, "Alert", "Ok");
                    //}
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
            Address = string.Empty;
            StateID = 0;
            CountryID = 0;
            City = string.Empty;
            IsSMSNotification = false;
            IsEmailNotification = false;
        }
        #endregion

        #region StateChangedCommand 

        public ICommand StateChangedCommand => new Command<State>(StateChanged);

        public void StateChanged(State state)
        {
            StateID = state.stateId;
        }

        #endregion

        #region NotificationTypeChangedCommand

        public ICommand NotificationTypeChangedCommand => new Command<string>(NotificationTypeChanged);

        public void NotificationTypeChanged(string item)
        {
            if (item == "Email")
            {
                IsEmailNotification = true;
            }
            if (item == "SMS")
            {
                IsSMSNotification = true;
            }
        }

        #endregion

    }
}

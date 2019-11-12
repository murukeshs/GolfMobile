﻿using Acr.UserDialogs;
using Golf.Models.userModel;
using Golf.Services;
using Golf.Utils;
using Golf.Views;
using Golf.Views.MenuView;
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
    public class LoginPageViewModel : BaseViewModel
    {
        //readonly IAuthenticationServices _authenticationServices;

        public bool IsValid { get; set; }
        public string UserNameText
        {
            get
            {
                return _UserNameText;
            }
            set
            {
                _UserNameText = value;
                OnPropertyChanged();
            }
        }
        private string _UserNameText = string.Empty;

        public string PasswordText
        {
            get
            {
                return _PasswordText;
            }
            set
            {
                _PasswordText = value;
                OnPropertyChanged();
            }
        }
        private string _PasswordText = string.Empty;

        //Constructor
        public LoginPageViewModel()
        {

        }

        #region LogIn Command Functionality
        public ICommand LoginInCommand => new AsyncCommand(SignInAsync);

        async Task SignInAsync()
        {
            IsValid = Validate();
            if (IsValid)
            {
               await LoginAuthentication();
            }
        }

        bool Validate()
        {
            if(string.IsNullOrEmpty(UserNameText))
            {
                //User Name Is Empty
                UserDialogs.Instance.AlertAsync("Email Address should not be empty.","Alert","Ok");
                return false;
            }
            else if (!Regex.IsMatch(UserNameText, "^\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*$"))
            {
                UserDialogs.Instance.AlertAsync("Email format is not valid.", "Alert", "Ok");
                return false;
            }
            else if (string.IsNullOrEmpty(PasswordText))
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


        /// <summary>
        /// Login Authentication Api Functionality
        /// Login URL - 
        /// </summary>
        /// <returns></returns>
        async Task LoginAuthentication()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    //App.User.TokenCreateDateTimeUtc = DateTime.UtcNow;
                    //App.SessionManager.EnableMonitor();
                    UserDialogs.Instance.ShowLoading();
                    string RestURL = App.User.BaseUrl + "/JWTAuthentication/login";
                    Uri requestUri = new Uri(RestURL);

                    var data = new Login
                    {
                        email = UserNameText,
                        password = PasswordText,
                        userTypeid = 1
                    };

                    string json = "";
                    json = JsonConvert.SerializeObject(data);
                    var httpClient = new HttpClient();
                    HttpResponseMessage response = await httpClient.PostAsync(requestUri, new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
                    string responJsonText = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        var view = new MenuPage();
                        var navigationPage = ((NavigationPage)App.Current.MainPage);
                        await navigationPage.PushAsync(view);
                    }
                    else
                    {
                        UserDialogs.Instance.HideLoading();
                        UserDialogs.Instance.Alert("Login failed", "Alert", "Ok");
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


        #endregion LogIn Command Functionality



        #region Register Command Functionality
        public ICommand RegisterCommand => new AsyncCommand(RegisterAsync);
        async Task RegisterAsync()
        {
            try
            {
                UserDialogs.Instance.ShowLoading();
                var view = new RegistrationPage();
                var navigationPage = ((NavigationPage)App.Current.MainPage);
                await navigationPage.PushAsync(view);
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                var a = ex.Message;
            }
        }
        #endregion Register Command Functionality
    }
}
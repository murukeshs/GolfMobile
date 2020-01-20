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
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Golf.ViewModel
{
    public class LoginPageViewModel : BaseViewModel
    {
        //Google Login Authentication 
        public IGoogleManager _googleManager;
        public GoogleUser GoogleUser;
        public bool IsLogedIn;

        //Facebook Authentication
        private readonly IFacebookManager _facebookManager;
        private FacebookUser _facebookUser;
        public FacebookUser FacebookUser
        {
            get { return _facebookUser; }
            set { SetProperty(ref _facebookUser, value); }
        }

        public LoginPageViewModel()
        {
            try
            {
                IFacebookManager facebookManager = DependencyService.Get<IFacebookManager>();
                _facebookManager = facebookManager;
            }
            catch(Exception ex)
            {
                var Error = ex.Message;
            }
        }

        #region Facebook Login Command

        public ICommand FacebookSignInCommand => new Command(FacebookLogin);

        private void FacebookLogout()
        {
            _facebookManager.Logout();
            IsLogedIn = false;
        }

        private void FacebookLogin()
        {
            _facebookManager.Login(OnLoginComplete);
        }

        private async void OnLoginComplete(FacebookUser facebookUser, string message)
        {
            if (facebookUser != null)
            {
                FacebookUser = facebookUser;
                IsLogedIn = true;
                FacebookLogout();
                var view = new MenuPage();
                var navigationPage = ((NavigationPage)App.Current.MainPage);
                await navigationPage.PushAsync(view);
            }
            else
            {
                UserDialogs.Instance.Alert(message, "Error", "Ok");
            }
        }
        #endregion

        #region Google Sign In Command
        public ICommand GoogleSignInCommand => new AsyncCommand(GoogleSignInAsync);

        async Task GoogleSignInAsync()
        {
            IGoogleManager googlemanager = DependencyService.Get<IGoogleManager>();
            _googleManager = googlemanager;
            GoogleLogout();
            if (_googleManager == null)
            {
                _googleManager = DependencyService.Get<IGoogleManager>();

                _googleManager.Login(OnLoginComplete);
            }
            else
            {
                _googleManager = DependencyService.Get<IGoogleManager>();

                _googleManager.Login(OnLoginComplete);
            }
        }

        private async void OnLoginComplete(GoogleUser googleUser, string message)
        {
            if (googleUser != null)
            {
                GoogleUser = googleUser;
                var id = googleUser.Id;
                IsLogedIn = true;
                // UserDialogs.Instance.ShowLoading();
                //await Task.Delay(5);
                GoogleLogout();
                //UserDialogs.Instance.HideLoading();
                var view = new MenuPage();
                var navigationPage = ((NavigationPage)App.Current.MainPage);
                await navigationPage.PushAsync(view);
            }
            else
            {
               UserDialogs.Instance.Alert(message, "Error", "Ok");
            }
        }

        private void GoogleLogout()
        {
            _googleManager.Logout();
            IsLogedIn = false;
        }
        #endregion

        #region Property Declaration

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
                OnPropertyChanged(nameof(UserNameText));
            }
        }
        private string _UserNameText = "swathit@apptomate.co";

        public string PasswordText
        {
            get
            {
                return _PasswordText;
            }
            set
            {
                _PasswordText = value;
                OnPropertyChanged(nameof(PasswordText));
            }
        }
        private string _PasswordText = "123";

        #endregion

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

        /// Login Authentication Api Functionality
        async Task LoginAuthentication()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading();
                    var data = new Login
                    {
                        email = UserNameText,
                        password = PasswordText
                    };

                    var result = await App.ApiClient.Login(data);

                    if (result != null)
                    {
                        var loginResponse = result;
                        App.User.AccessToken = loginResponse.token;
                        App.User.UserProfileImage = loginResponse.user.profileImage;
                        var lastname = (string.IsNullOrEmpty(loginResponse.user.lastName)) ? "" : " " + loginResponse.user.lastName;
                        App.User.UserName = loginResponse.user.firstName + lastname;
                        App.User.UserId = loginResponse.user.userId;
                        App.User.UserEmail = loginResponse.user.email;
                        App.User.IsModerator = loginResponse.user.isModerator;
                        var view = new MenuPage();
                        var navigationPage = ((NavigationPage)App.Current.MainPage);
                        await navigationPage.PushAsync(view);
                        resetFormValues();
                    }
                    UserDialogs.Instance.HideLoading();
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
            UserNameText = string.Empty;
            PasswordText = string.Empty;
        }

        #endregion LogIn Command Functionality

        #region Register Command Functionality
        public ICommand RegisterCommand => new AsyncCommand(RegisterAsync);

        async Task RegisterAsync()
        {
            UserDialogs.Instance.ShowLoading();
            try
            {
                var view = new RegistrationPage();
                var navigationPage = ((NavigationPage)App.Current.MainPage);
                await navigationPage.PushAsync(view);
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                var a = ex.Message;
            }
        }
        #endregion Register Command Functionality

    }
}

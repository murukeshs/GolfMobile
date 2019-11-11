using Acr.UserDialogs;
using Golf.Utils;
using Golf.Views;
using Golf.Views.MenuView;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Golf.ViewModel
{
    public class LoginPageViewModel : BaseViewModel
    {
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
            try
            {
                IsValid = Validate();
                if (IsValid)
                {
                    UserDialogs.Instance.ShowLoading();
                    var view = new MenuPage();
                    var navigationPage = ((NavigationPage)App.Current.MainPage);
                    await navigationPage.PushAsync(view);
                    UserDialogs.Instance.HideLoading();
                }
            }
            catch (Exception ex)
            {
                var a = ex.Message;
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
            //else if (!Regex.IsMatch(UserNameText, "^\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*$"))
            //{
            //    UserDialogs.Instance.AlertAsync("Email format is not valid.", "Alert", "Ok");
            //    return false;
            //}
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

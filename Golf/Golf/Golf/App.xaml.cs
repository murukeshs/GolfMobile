using Golf.Models;
using Golf.Services;
using Golf.Views;
using Golf.Views.MenuView;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Golf
{
    public partial class App : Application
    {
        internal static UserModel User { get; set; } = new UserModel();

        internal static IAPIClient ApiClient { get; set; } = new APIClient();

        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new LoginPage());
            //MainPage = new LoginPage();
            if (Device.RuntimePlatform == Device.iOS)
            {
                MainPage.SetValue(NavigationPage.BarBackgroundColorProperty, Color.FromRgb(52, 165, 116));
            }
            if (Device.RuntimePlatform == Device.Android)
            {
                MainPage.SetValue(NavigationPage.BarBackgroundColorProperty, Color.FromHex("#34a574"));
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        private void SetLoginPageAsRootPage(object sender)
        {
            MainPage = new NavigationPage(new LoginPage());
        }

        private void SetMainPageAsRootPage(object sender)
        {
            MainPage = new NavigationPage(new MenuPage());
        }
    }
}

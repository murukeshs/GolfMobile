using Acr.UserDialogs;
using Golf.Services;
using Golf.Views.MenuView;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Golf.Views.CreateMatchView
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SendInvitePoppup : PopupPage
    {
		public SendInvitePoppup ()
		{
			InitializeComponent ();
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            // PopupNavigation.Instance.PushAsync(this);
        }

        // Invoked when a hardware back button is pressed
        protected override bool OnBackButtonPressed()
        {
            // Return true if you don't want to close this popup page when a back button is pressed
            base.OnBackButtonPressed();
            return true;
        }

        // Invoked when background is clicked
        protected override bool OnBackgroundClicked()
        {
            // Return false if you don't want to close this popup page when a background of the popup page is clicked
            base.OnBackgroundClicked();
            return false;
        }

        private async void RoundedButton_Clicked(object sender, System.EventArgs e)
        {
            try
            {
                UserDialogs.Instance.ShowLoading();
            await PopupNavigation.Instance.RemovePageAsync(this);
            var view = new MenuPage();
            var navigationPage = ((NavigationPage)App.Current.MainPage);
            await navigationPage.PushAsync(view);
            UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                var a = ex.Message;
                UserDialogs.Instance.HideLoading();
            }
        }
    }
}
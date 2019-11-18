using Acr.UserDialogs;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Golf.Views.CreateMatchView
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MatchCodePoppup : PopupPage
    {
		public MatchCodePoppup ()
		{
			InitializeComponent ();
		}

        protected override Task OnAppearingAnimationEndAsync()
        {
            return Content.FadeTo(0.5);
        }

        protected override Task OnDisappearingAnimationBeginAsync()
        {
            return Content.FadeTo(1);
        }

        // Invoked when a hardware back button is pressed
        protected override bool OnBackButtonPressed()
        {
            // Return true if you don't want to close this popup page when a back button is pressed
            base.OnBackButtonPressed();
            return false;
        }

        // Invoked when background is clicked
        protected override bool OnBackgroundClicked()
        {
            // Return false if you don't want to close this popup page when a background of the popup page is clicked
            base.OnBackgroundClicked();
            return true;
        }

        private async void RoundedButton_Clicked(object sender, System.EventArgs e)
        {
            try
            {
                CloseAllPopup();
                UserDialogs.Instance.ShowLoading();
                var view = new MatchTeamListPage();
                var navigationPage = ((NavigationPage)App.Current.MainPage);
                await navigationPage.PushAsync(view);
                //await PopupNavigation.Instance.PushAsync(new SendInvitePoppup());
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                var a = ex.Message;
            }
        }

        private async void CloseAllPopup()
        {
            await PopupNavigation.Instance.PopAllAsync();
        }
    }
}
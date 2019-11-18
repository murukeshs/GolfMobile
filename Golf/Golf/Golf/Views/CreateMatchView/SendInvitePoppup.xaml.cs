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
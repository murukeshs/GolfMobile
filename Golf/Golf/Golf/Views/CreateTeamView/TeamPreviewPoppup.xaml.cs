using Golf.Models;
using Golf.Services;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Golf.Views.PoppupView
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TeamPreviewPage : PopupPage
    {
        public TeamPreviewPage ()
		{
			InitializeComponent ();
            ListView.ItemsSource = App.User.TeamPreviewList;
        }

        private void ImageButton_Clicked(object sender, System.EventArgs e)
        {
            try
            {
                var item = (sender as ImageButton).BindingContext as AddPlayersList;

                var userId = item.UserId;

                App.User.TeamPreviewList.Remove(item);

                ListView.ItemsSource = App.User.TeamPreviewList;
            }
            catch(Exception ex)
            {
                var a = ex.Message;
            }
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

        private async void CloseButton_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }
    }
}
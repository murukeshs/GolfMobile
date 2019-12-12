using Golf.Models;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Golf.Views.PoppupView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PreviewPlayersPage : PopupPage
    {
        public PreviewPlayersPage()
        {
            InitializeComponent();
            BindingContext = this;
            ListView.ItemsSource = App.User.PlayersPreviewList;
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

        private async void RemoveButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                var result = await DisplayAlert("", "Are sure you want to Remove?.", "Yes", "No");

                if (!result)
                {
                    return;
                }
                else
                {
                    var item = (sender as ImageButton).BindingContext as AddPlayersList;

                    var userId = item.UserId;

                    App.User.PlayersPreviewList.Remove(item);

                    ListView.ItemsSource = App.User.PlayersPreviewList;

                    MessagingCenter.Send<App, string>((App)Application.Current, App.User.ISPLAYERLISTREFRESH, userId.ToString());
                }

            }
            catch (Exception ex)
            {
                var a = ex.Message;
            }
        }
    }
}
using Golf.Models;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Golf.Views.PoppupView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PreviewPlayersPage : PopupPage
    {
        ObservableCollection<AllParticipantsResponse> PlayersList;

        public PreviewPlayersPage(ObservableCollection<AllParticipantsResponse> ParticipantsList)
        {
            InitializeComponent();
            BindingContext = this;
            PlayersList = ParticipantsList;
            if (PlayersList.Count == 0)
            {
                NoPlayersCard.IsVisible = true;
                PlayersListView.IsVisible = false;
            }
            else
            {
                PlayersListView.ItemsSource = PlayersList;
                NoPlayersCard.IsVisible = false;
                PlayersListView.IsVisible = true;
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
                    var item = (sender as ImageButton).BindingContext as AllParticipantsResponse;

                    var userId = item.userId;

                    PlayersList.Remove(item);

                    PlayersListView.ItemsSource = PlayersList;

                    if (PlayersList.Count == 0)
                    {
                        NoPlayersCard.IsVisible = true;
                        PlayersListView.IsVisible = false;
                    }

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
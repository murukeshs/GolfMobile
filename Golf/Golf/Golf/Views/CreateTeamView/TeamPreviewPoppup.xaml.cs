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
	public partial class TeamPreviewPage : PopupPage
    {
        public string TeamPreviewName
        {
            get
            {
                return _TeamPreviewName;
            }
            set
            {
                _TeamPreviewName = value;
                OnPropertyChanged(nameof(TeamPreviewName));
            }
        }
        private string _TeamPreviewName = string.Empty;

        public string ScoreKeeperName
        {
            get
            {
                return _ScoreKeeperName;
            }
            set
            {
                _ScoreKeeperName = value;
                OnPropertyChanged(nameof(ScoreKeeperName));
            }
        }
        private string _ScoreKeeperName = string.Empty;

        public string ScoreKeeperProfileImage
        {
            get
            {
                return _ScoreKeeperProfileImage;
            }
            set
            {
                _ScoreKeeperProfileImage = value;
                OnPropertyChanged(nameof(ScoreKeeperProfileImage));
            }
        }
        private string _ScoreKeeperProfileImage = string.Empty;

        ObservableCollection<AllParticipantsResponse> PlayersList;

        public TeamPreviewPage (ObservableCollection<AllParticipantsResponse> ParticipantsList)
		{
			InitializeComponent ();
            BindingContext = this;
            TeamPreviewName = App.User.TeamName;
            ScoreKeeperName = App.User.TeamPreviewScoreKeeperName;
            PlayersList = ParticipantsList;
            if (!string.IsNullOrEmpty(ScoreKeeperName))
            {
                ScoreKeeperCard.IsVisible = true;
                NoScoreKeeperCard.IsVisible = false;
            }
            else
            {
                ScoreKeeperCard.IsVisible = false;
                NoScoreKeeperCard.IsVisible = true;
            }
            if (!string.IsNullOrEmpty(App.User.TeamPreviewScoreKeeperProfilePicture))
            {
                ScoreKeeperProfileImage = App.User.TeamPreviewScoreKeeperProfilePicture;
            }
            else
            {
                ScoreKeeperProfileImage = "profile_defalut_pic.png";
            }
            if(ParticipantsList.Count == 0)
            {
                NoPlayerCard.IsVisible = true;
                ListView.IsVisible = false;
            }
            else
            {
                ListView.ItemsSource = PlayersList;
                ListView.IsVisible = true;
                NoPlayerCard.IsVisible = false;
            }

        }

        private async void ImageButton_Clicked(object sender, System.EventArgs e)
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

                    ListView.ItemsSource = PlayersList;

                    if (PlayersList.Count == 0)
                    {
                        NoPlayerCard.IsVisible = true;
                        ListView.IsVisible = false;
                    }

                    MessagingCenter.Send<App, string>((App)Application.Current, App.User.ISPARTICIPANTLISTREFRESH, userId.ToString());
                }
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
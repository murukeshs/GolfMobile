using Acr.UserDialogs;
using Golf.Models;
using Golf.Services;
using Golf.ViewModel.Match;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Golf.Views.CreateMatchView
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MatchTeamListPage : ContentPage
	{
		public MatchTeamListPage ()
		{
			InitializeComponent ();
		}

        private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            var item = (sender as CheckBox).BindingContext as MatchTeamWithPlayers;
            var noofplayers = Convert.ToUInt32(item.noOfPlayers);
            ((MatchTeamListPageViewModel)BindingContext).CheckBoxSelectedCommand.Execute(item);
        }

        #region screen adjusting
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (Device.RuntimePlatform == Device.Android)
            {
                DependencyService.Get<IAdjustScreenSize>().AdjustScreen();
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if (Device.RuntimePlatform == Device.Android)
            {
                DependencyService.Get<IAdjustScreenSize>().UnAdjustScreen();
            }
        }
        #endregion

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;
            var item = (MatchTeamWithPlayers)e.Item;

            if (item.teamplayerList != null)
            {
                //TeamItemsTabbedCommand
                ((MatchTeamListPageViewModel)BindingContext).TeamItemsTabbedCommand.Execute(e.Item as MatchTeamWithPlayers);
            }
            else
            {
                UserDialogs.Instance.Alert("No Players available for your selected team.","Alert","Ok");
            }

            //Deselect the selected Item in the listview
            ((ListView)sender).SelectedItem = null;
        }
    }
}
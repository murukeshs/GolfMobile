using Acr.UserDialogs;
using Golf.Models;
using Golf.Services;
using Golf.ViewModel.Round;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Golf.Views.CreateRoundView
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RoundTeamListPage : ContentPage
	{
		public RoundTeamListPage ()
		{
			InitializeComponent ();
		}

        private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            var item = (sender as CheckBox).BindingContext as RoundTeamWithPlayers;
            var noofplayers = Convert.ToUInt32(item.noOfPlayers);
            ((RoundTeamListPageViewModel)BindingContext).CheckBoxSelectedCommand.Execute(item);
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
            var item = (RoundTeamWithPlayers)e.Item;

            if (item.teamplayerList != null)
            {
                //TeamItemsTabbedCommand
                ((RoundTeamListPageViewModel)BindingContext).TeamItemsTabbedCommand.Execute(e.Item as RoundTeamWithPlayers);
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
using Golf.Models;
using Golf.Services;
using Golf.ViewModel.Match;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Golf.Views.MatchDetailsView
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MatchDetailsPage : ContentPage
	{

        public MatchDetailsPage ()
		{
			InitializeComponent ();
        }

        private void TeamListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;
            ((MatchDetailsPageViewModel)BindingContext).TeamItemsTabbedCommand.Execute(e.Item as MatchDetailsListTeamList);

            //Deselect the selected Item in the listview
            ((ListView)sender).SelectedItem = null;
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

        private void CustomPicker_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            Picker picker = sender as Picker;
            var Item = (CompetitionType)picker.SelectedItem;
            ((MatchDetailsPageViewModel)BindingContext).CompetitionTypeSelectedCommand.Execute(Item);
        }

        private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            var item = (sender as CheckBox).BindingContext as MatchRules;
            ((MatchDetailsPageViewModel)BindingContext).CheckBoxSelectedCommand.Execute(item);
        }
    }
}
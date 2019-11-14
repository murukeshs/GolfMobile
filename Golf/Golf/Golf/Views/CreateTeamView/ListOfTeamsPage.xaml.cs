using Golf.Models;
using Golf.Services;
using Golf.ViewModel;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Golf.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ListOfTeamsPage : ContentPage
	{
        public ObservableCollection<TeamList> TeamItems { get; set; }
        public ListOfTeamsPage ()
		{
			InitializeComponent ();
        }

        private void TeamsListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListOfTeamsPageViewModel)BindingContext).TeamListItemsTabbedCommand.Execute(e.Item as MatchTeamItems);
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
    }
}
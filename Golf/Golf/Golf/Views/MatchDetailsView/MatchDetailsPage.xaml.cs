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
            ((MatchDetailsPageViewModel)BindingContext).TeamItemsTabbedCommand.Execute(e.Item as getMatchesDetailsById);
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
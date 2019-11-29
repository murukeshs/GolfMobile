using Golf.Models;
using Golf.Services;
using Golf.ViewModel.Round;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Golf.Views.RoundDetailsView
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RoundListPage : ContentPage
	{
		public RoundListPage()
		{
			InitializeComponent ();
		}

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;
            ((RoundListPageViewModel)BindingContext).ListItemTabbedCommand.Execute(e.Item as RoundList);
            //Deselect Item
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
    }
}
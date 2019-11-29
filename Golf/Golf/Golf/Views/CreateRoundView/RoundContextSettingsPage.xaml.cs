using Golf.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Golf.Views.CreateRoundView
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RoundContextSettingsPage : ContentPage
	{
		public RoundContextSettingsPage ()
		{
			InitializeComponent ();
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
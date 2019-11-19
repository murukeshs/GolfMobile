using Golf.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Golf.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProfilePage : ContentPage
	{
		public ProfilePage ()
		{
			InitializeComponent ();
		}

        private void CommunicationInfoButton_Clicked(object sender, EventArgs e)
        {
            CommunicationInfoStackLayout.IsVisible = true;
            PersonalInfoStackLayout.IsVisible = false;
            CommunicationInfoButton.BackgroundColor = (Color)App.Current.Resources["LightGreenColor"];
            CommunicationInfoButton.TextColor = (Color)App.Current.Resources["WhiteColor"];

            PersonalInfoButton.BackgroundColor = (Color)App.Current.Resources["WhiteColor"];
            PersonalInfoButton.TextColor = (Color)App.Current.Resources["LightGreenColor"];
        }

        private void PersonalInfoButton_Clicked(object sender, EventArgs e)
        {
            CommunicationInfoStackLayout.IsVisible = false;
            PersonalInfoStackLayout.IsVisible = true;

            PersonalInfoButton.BackgroundColor = (Color)App.Current.Resources["LightGreenColor"];
            PersonalInfoButton .TextColor = (Color)App.Current.Resources["WhiteColor"];

            CommunicationInfoButton.BackgroundColor = (Color)App.Current.Resources["WhiteColor"];
            CommunicationInfoButton.TextColor = (Color)App.Current.Resources["LightGreenColor"];
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

        private void CustomCheckBox_CheckedChanged(object sender, bool e)
        {

        }
    }
}
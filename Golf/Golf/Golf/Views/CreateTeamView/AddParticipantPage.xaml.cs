using Golf.Models;
using Golf.Services;
using Golf.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Golf.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddParticipantPage : ContentPage
	{
        public AddParticipantPage ()
		{
			InitializeComponent ();
        }

        private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            var item = (sender as CheckBox).BindingContext as user;
            ((AddParticipantPageViewModel)BindingContext).CheckBoxSelectedCommand.Execute(item);
        }

        private void Switch_Toggled(object sender, ToggledEventArgs e)
        {
            var item = (sender as Switch).BindingContext as user;
            ((AddParticipantPageViewModel)BindingContext).ToggleSelectedCommand.Execute(item);
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
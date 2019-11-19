using Golf.Controls;
using Golf.Services;
using Golf.ViewModel;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Golf.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class InviteParticipantPage : PopupPage
    {
        InviteParticipantPageViewModel vm;
        public InviteParticipantPage ()
		{
			InitializeComponent ();
            vm = BindingContext as InviteParticipantPageViewModel;
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

        private void GenderOnchange(object sender, EventArgs e)
        {
            Picker picker = sender as Picker;
            vm.GenderText = picker.SelectedItem.ToString();
        }

        private void UserTypeChanged(object sender, Xamarin.Forms.Internals.EventArg<bool> e)
        {
            var value = (CustomCheckBox)sender;
            var item = value.DefaultValue;
            ((InviteParticipantPageViewModel)BindingContext).UserTypeCheckBoxCommand.Execute(item);
        }

        private async void ImageButton_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }
    }
}
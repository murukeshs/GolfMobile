using Golf.Controls;
using Golf.Models;
using Golf.Services;
using Golf.ViewModel;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Golf.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProfilePage : ContentPage
	{
        ProfilePageViewModel vm;
		public ProfilePage ()
		{
            vm = BindingContext as ProfilePageViewModel;
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

        private void UserTypeChanged(object sender, Xamarin.Forms.Internals.EventArg<bool> e)
        {
            var value = (CustomCheckBox)sender;
            var item = value.DefaultValue;
            ((ProfilePageViewModel)BindingContext).UserTypeCheckBoxCommand.Execute(item);
        }

        private void NotificationTypeChanged(object sender, bool e)
        {
            var value = (CustomCheckBox)sender;
            var item = value.DefaultValue;
            if (item == "Email")
            {
                vm.IsEmailNotification = true;
            }
            if (item == "SMS")
            {
                vm.IsSmsNotification = true;
            }
        }
        private void GenderOnchange(object sender, EventArgs e)
        {
            Picker picker = sender as Picker;
            var index = GenderPicker.SelectedIndex;
            ((ProfilePageViewModel)BindingContext).PickerSelectedCommand.Execute(index);
            
        }

        private void CountryIndexChanged(object sender, EventArgs e)
        {
            Picker picker = sender as Picker;
            Country country = (Country)picker.SelectedItem;
            vm.CountryID = country.countryId;
            ((ProfilePageViewModel)BindingContext).CountryChangedCommand.Execute(country.countryId);
        }

        private void StateIndexChanged(object sender, EventArgs e)
        {
            Picker picker = sender as Picker;
            State state = (State)picker.SelectedItem;
            vm.StateID = state.stateId;
        }

        private void UpdateCommunicationInfo_Clicked(object sender, EventArgs e)
        {

        }

        private void UpdateUserInfo_Clicked(object sender, EventArgs e)
        {

        }

        private void DOB_DateSelected(object sender, DateChangedEventArgs e)
        {
            DOB.Date = Convert.ToDateTime(vm.Dob);
        }

        private void UserTypeChanged(object sender, bool e)
        {
            var value = (CustomCheckBox)sender;
            var item = value.DefaultValue;
            ((ProfilePageViewModel)BindingContext).UserTypeCheckBoxCommand.Execute(item);
        }
    }
}
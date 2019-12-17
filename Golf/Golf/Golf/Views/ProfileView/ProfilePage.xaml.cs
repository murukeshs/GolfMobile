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
            ((ProfilePageViewModel)BindingContext).CountryChangedCommand.Execute(country.countryId);
        }

        private void StateIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Picker picker = sender as Picker;
                State state = (State)picker.SelectedItem;
                ((ProfilePageViewModel)BindingContext).StateChangedCommand.Execute(state.stateId);
            }
            catch(Exception ex)
            {

            }
        }
       
        private void DOBDatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            var date = DOBDatePicker.Date.ToString("yyyy/MM/dd");
            ((ProfilePageViewModel)BindingContext).DateChangedEventCommand.Execute(date);
        }
    }
}
using Acr.UserDialogs;
using Golf.Controls;
using Golf.Models;
using Golf.Services;
using Golf.ViewModel;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Golf.Views.MenuView
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RegistrationPageAdmin : ContentPage
	{
        RegistrationPageAdminViewModel vm;
        public RegistrationPageAdmin ()
		{
			InitializeComponent ();
            vm = BindingContext as RegistrationPageAdminViewModel;
        }

        private async void RegisterButton_Clicked(object sender, EventArgs e)
        {
            UserDialogs.Instance.ShowLoading();
            var view = new OtpVerificationPage();
            var navigationPage = ((NavigationPage)App.Current.MainPage);
            await navigationPage.PushAsync(view);
            UserDialogs.Instance.HideLoading();
        }

        private void CountryIndexChanged(object sender, EventArgs e)
        {
            Picker picker = sender as Picker;
            Country country = (Country)picker.SelectedItem;
            vm.CountryID = country.countryId;
            ((RegistrationPageAdminViewModel)BindingContext).CountryChangedCommand.Execute(country);
        }

        private void StateIndexChanged(object sender, EventArgs e)
        {
            Picker picker = sender as Picker;
            State state = (State)picker.SelectedItem;
            vm.StateID = state.stateId;
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
            if(item == "Email")
            {
                vm.IsEmailNotification = true;
            }
            if(item == "SMS")
            {
                vm.IsSMSNotification = true;
            }
        }
    }
}
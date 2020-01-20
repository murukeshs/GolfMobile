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
        public RegistrationPageAdmin ()
		{
			InitializeComponent ();
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
            ((RegistrationPageAdminViewModel)BindingContext).CountryChangedCommand.Execute(country);
        }

        private void StateIndexChanged(object sender, EventArgs e)
        {
            Picker picker = sender as Picker;
            State state = (State)picker.SelectedItem;
            ((RegistrationPageAdminViewModel)BindingContext).StateChangedCommand.Execute(state);
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
            ((RegistrationPageAdminViewModel)BindingContext).NotificationTypeChangedCommand.Execute(value.DefaultValue);
        }
    }
}
using Acr.UserDialogs;
using Golf.ViewModel;
using Golf.Services;
using Golf.Views.MenuView;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Golf.Controls;

namespace Golf.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RegistrationPage : ContentPage
	{
        public RegistrationPage ()
		{
			InitializeComponent ();
        }

        private void GenderOnchange(object sender, EventArgs e)
        {
            Picker picker = sender as Picker;
            ((RegistrationPageViewModel)BindingContext).GenderOnChangeCommand.Execute(picker.SelectedItem.ToString());
        }

        private void DobPickerSelected(object sender, DateChangedEventArgs e)
        {
            ((RegistrationPageViewModel)BindingContext).DOBSelectedCommand.Execute(DobPicker.Date.ToString());
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
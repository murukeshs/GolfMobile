using Acr.UserDialogs;
using Golf.Models;
using Golf.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
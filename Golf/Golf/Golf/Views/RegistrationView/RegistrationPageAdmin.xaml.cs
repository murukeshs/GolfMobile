using Acr.UserDialogs;
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
    }
}
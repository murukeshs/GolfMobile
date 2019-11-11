using Acr.UserDialogs;
using Golf.Views.MenuView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Golf.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RegistrationPage : ContentPage
	{
		public RegistrationPage ()
		{
			InitializeComponent ();
		}

        

        private async void RegisterAdminButton_Clicked(object sender, EventArgs e)
        {
            UserDialogs.Instance.ShowLoading();
            var view = new RegistrationPageAdmin();
            var navigationPage = ((NavigationPage)App.Current.MainPage);
            await navigationPage.PushAsync(view);
            UserDialogs.Instance.HideLoading();
        }
    }
}
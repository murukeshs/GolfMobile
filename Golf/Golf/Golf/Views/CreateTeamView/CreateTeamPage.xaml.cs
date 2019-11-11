using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Golf.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CreateTeamPage : ContentPage
	{
		public CreateTeamPage ()
		{
			InitializeComponent ();
		}

        private void ProceedButton_Clicked(object sender, EventArgs e)
        {

        }
    }
}
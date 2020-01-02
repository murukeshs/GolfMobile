using Golf.Models;
using Golf.ViewModel.Round;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Golf.Views.CreateRoundView
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ParticipantSelection : ContentPage
	{
		public ParticipantSelection ()
		{
			InitializeComponent ();
		}

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            ((ParticipantSelectionViewModel)BindingContext).SearchCommand.Execute(e.NewTextValue);
        }

    }
}
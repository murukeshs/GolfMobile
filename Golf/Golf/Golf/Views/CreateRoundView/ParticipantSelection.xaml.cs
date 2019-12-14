using Golf.Models;
using Golf.ViewModel.Round;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            var item = (sender as CheckBox).BindingContext as AllParticipantsResponse;
            ((ParticipantSelectionViewModel)BindingContext).CheckBoxSelectedCommand.Execute(item);
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            ((ParticipantSelectionViewModel)BindingContext).SearchCommand.Execute(e.NewTextValue);
        }
    }
}
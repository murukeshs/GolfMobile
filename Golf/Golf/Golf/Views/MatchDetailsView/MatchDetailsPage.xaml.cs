using Golf.Models;
using Golf.ViewModel.Match;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PancakeView;
using Xamarin.Forms.Xaml;

namespace Golf.Views.MatchDetailsView
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MatchDetailsPage : ContentPage
	{

        public MatchDetailsPage ()
		{
			InitializeComponent ();
        }

        private void PlayersListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {

        }

        private void TeamListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((MatchDetailsPageViewModel)BindingContext).TeamItemsTabbedCommand.Execute(e.Item as MatchTeamList);
        }

       
    }
}
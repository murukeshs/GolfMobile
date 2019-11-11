using Golf.Models;
using Golf.ViewModel.Match;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Golf.Views.MatchDetailsView
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MatchListPage : ContentPage
	{
		public MatchListPage()
		{
			InitializeComponent ();
		}

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((MatchListPageViewModel)BindingContext).ListItemTabbedCommand.Execute(e.Item as MatchList);
        }
    }
}
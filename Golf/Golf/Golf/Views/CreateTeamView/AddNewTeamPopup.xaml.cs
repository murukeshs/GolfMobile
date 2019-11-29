using Golf.Models;
using Golf.ViewModel;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Golf.Views.CreateTeamView
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddNewTeamPopup : PopupPage
	{
		public AddNewTeamPopup ()
		{
			InitializeComponent ();
		}

        private void TeamsListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;
            ((AddNewTeamViewModel)BindingContext).TeamListItemsTabbedCommand.Execute(e.Item as RoundTeamItems);
            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
    }
}
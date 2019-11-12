using Golf.Models;
using Golf.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Golf.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ListOfTeamsPage : ContentPage
	{
        public ObservableCollection<TeamList> TeamItems { get; set; }
        public ListOfTeamsPage ()
		{
			InitializeComponent ();
        }

        private void TeamsListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListOfTeamsPageViewModel)BindingContext).TeamListItemsTabbedCommand.Execute(e.Item as TeamList);
        }
    }
}
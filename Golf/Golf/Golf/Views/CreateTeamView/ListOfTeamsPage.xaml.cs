using Golf.Models;
using Golf.Services;
using Golf.ViewModel;
using System.Collections.ObjectModel;
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
            if (e.Item == null)
                return;
            ((ListOfTeamsPageViewModel)BindingContext).TeamListItemsTabbedCommand.Execute(e.Item as RoundTeamItems);
            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
    }
}
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
    public partial class FinalTeamsForRound : ContentPage
    {
        public FinalTeamsForRound()
        {
            InitializeComponent();
        }

        private void TeamListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;
            ((FinalTeamsForRoundViewModel)BindingContext).TeamItemsTabbedCommand.Execute(e.Item as RoundDetailsListTeamList);

            //Deselect the selected Item in the listview
            ((ListView)sender).SelectedItem = null;
        }
    }
}
using Golf.Models;
using Golf.Views.PoppupView;
using Rg.Plugins.Popup.Services;
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
	public partial class AddParticipantPage : ContentPage
	{
        public ObservableCollection<AddPlayersList> PlayersList { get; set; }
        public AddParticipantPage ()
		{
			InitializeComponent ();

            PlayersList = new ObservableCollection<AddPlayersList>(new[]
             {
                 new AddPlayersList { PlayerName = "Player 1" ,PlayerHCP = "5",PlayerType= "P",IsStoreKeeper=false },
                 new AddPlayersList { PlayerName = "Player 2" ,PlayerHCP = "5",PlayerType= "P",IsStoreKeeper=false },
                 new AddPlayersList { PlayerName = "Player 3" ,PlayerHCP = "5",PlayerType= "P",IsStoreKeeper=false },
                 new AddPlayersList { PlayerName = "Player 4" ,PlayerHCP = "5",PlayerType= "P",IsStoreKeeper=true },
                 new AddPlayersList { PlayerName = "Player 5" ,PlayerHCP = "5",PlayerType= "P",IsStoreKeeper=false },
                 new AddPlayersList { PlayerName = "Player 6" ,PlayerHCP = "5",PlayerType= "P",IsStoreKeeper=false },
                 new AddPlayersList { PlayerName = "Player 7" ,PlayerHCP = "5",PlayerType= "P",IsStoreKeeper=false },
                 new AddPlayersList { PlayerName = "Player 8" ,PlayerHCP = "5",PlayerType= "P",IsStoreKeeper=false },
                 new AddPlayersList { PlayerName = "Player 9" ,PlayerHCP = "5",PlayerType= "P",IsStoreKeeper=false },
                 new AddPlayersList { PlayerName = "Player 10" ,PlayerHCP = "5",PlayerType= "P",IsStoreKeeper=false }
              });
            ListView.ItemsSource = PlayersList;
        }

        private async void RoundedButton_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new TeamPreviewPage());
        }
    }
}
using Golf.Models;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Golf.Views.PoppupView
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TeamPreviewPage : PopupPage
    {
        public ObservableCollection<AddPlayersList> TeamPreviewList { get; set; }
        public TeamPreviewPage ()
		{
			InitializeComponent ();

            TeamPreviewList = new ObservableCollection<AddPlayersList>(new[]
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
            ListView.ItemsSource = TeamPreviewList;
        }
	}
}
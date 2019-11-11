using Golf.Models;
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

            TeamItems = new ObservableCollection<TeamList>(new[]
               {
                    new TeamList { Profile = "profile_pic.png",TeamName="Chitra",Noofplayers = "10",Created = "Chitra"},
                    new TeamList { Profile = "profile_pic.png",TeamName="Swathi",Noofplayers = "10",Created = "Swathi"},
                    new TeamList { Profile = "profile_pic.png",TeamName="Allywn",Noofplayers = "10",Created = "Allywn"},
                    new TeamList { Profile = "profile_pic.png",TeamName="Karthi",Noofplayers = "10",Created = "Karthi"},
                    new TeamList { Profile = "profile_pic.png",TeamName="Elango",Noofplayers = "10",Created = "Elango"},
                    new TeamList { Profile = "profile_pic.png",TeamName="Ganesh",Noofplayers = "10",Created = "Ganesh"},
                    new TeamList { Profile = "profile_pic.png",TeamName="Murukesh",Noofplayers = "10",Created = "Murukesh"},
                    new TeamList { Profile = "profile_pic.png",TeamName="Kaja",Noofplayers = "10",Created = "Kaja"},
                    new TeamList { Profile = "profile_pic.png",TeamName="Varun",Noofplayers = "10",Created = "Varun"},
                    new TeamList { Profile = "profile_pic.png",TeamName="Sandy",Noofplayers = "10",Created = "Sandy"}
                });

            DataGridTable.ItemsSource = TeamItems;
        }
	}
}
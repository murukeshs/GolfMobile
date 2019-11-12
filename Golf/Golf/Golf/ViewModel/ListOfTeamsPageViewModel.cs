using Acr.UserDialogs;
using Golf.Models;
using Golf.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Golf.ViewModel
{
    public class ListOfTeamsPageViewModel : BaseViewModel
    {
        public ObservableCollection<TeamList> TeamItems
        {
            get { return _TeamItem; }
            set
            {
                _TeamItem = value;
                OnPropertyChanged(nameof(TeamItems));
            }
        }
        private ObservableCollection<TeamList> _TeamItem= null;
        public ListOfTeamsPageViewModel()
        {
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
        }

        #region Register Command Functionality
        public ICommand TeamListItemsTabbedCommand => new Command(TeamListItemsTabbed);
        async void TeamListItemsTabbed(object parameter)
        {
            try
            {
                UserDialogs.Instance.ShowLoading();
                var view = new AddParticipantPage();
                var navigationPage = ((NavigationPage)App.Current.MainPage);
                await navigationPage.PushAsync(view);
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                var a = ex.Message;
            }
        }
        #endregion Register Command Functionality
    }
}

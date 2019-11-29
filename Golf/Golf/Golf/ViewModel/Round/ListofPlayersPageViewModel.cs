using Acr.UserDialogs;
using Golf.Models;
using Golf.Utils;
using Golf.Views.CreateRoundView;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Golf.ViewModel.Round
{
    public class ListofPlayersPageViewModel : BaseViewModel
    {
        public ObservableCollection<PlayersList> ParticipantItems
        {
            get { return _ParticipantItems; }
            set
            {
                _ParticipantItems = value;
                OnPropertyChanged(nameof(ParticipantItems));
            }
        }
        private ObservableCollection<PlayersList> _ParticipantItems = null;

        public ListofPlayersPageViewModel()
        {

            ParticipantItems = new ObservableCollection<PlayersList>(new[]
              {
                    new PlayersList { PlayerProfile = "profile_pic.jpg", PlayerName = "John Mic" ,PlayerGmail = "johnmic@gmail.com",HCP= "5" ,Type = "P",Gender = "man_gray.png"},
                    new PlayersList { PlayerProfile = "profile_pic.jpg", PlayerName = "Charlotte Mia" ,PlayerGmail = "Charlotte@gmail.com",HCP= "5" ,Type = "P",Gender = "woman_gray.png" },
                    new PlayersList { PlayerProfile = "profile_pic.jpg", PlayerName = "MsDhoni" ,PlayerGmail = "msd@gmail.com",HCP= "8" ,Type = "P",Gender = "man_gray.png"},
                    new PlayersList { PlayerProfile = "profile_pic.jpg", PlayerName = "John Mic" ,PlayerGmail = "johnmic@gmail.com",HCP= "5" ,Type = "P",Gender = "man_gray.png"},
                    new PlayersList { PlayerProfile = "profile_pic.jpg", PlayerName = "Charlotte Mia" ,PlayerGmail = "Charlotte@gmail.com",HCP= "10" ,Type = "P",Gender = "woman_gray.png" },
                    new PlayersList { PlayerProfile = "profile_pic.jpg", PlayerName = "MsDhoni" ,PlayerGmail = "msd@gmail.com",HCP= "7" ,Type = "P",Gender = "man_gray.png"}
              });
        }

        #region Next ImageButton Command Functionality
        public ICommand NextImageButtonCommand => new AsyncCommand(NextImageButtonAsync);
        async Task NextImageButtonAsync()
        {
            try
            {
                UserDialogs.Instance.ShowLoading();
                var view = new SendInvitePoppup();
                var navigationPage = ((NavigationPage)App.Current.MainPage);
                await navigationPage.PushAsync(view);
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                var a = ex.Message;
            }
        }
        #endregion Next ImageButton Command Functionality
    }
}
